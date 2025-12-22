# Non-blocking verification: emits GitHub Actions warnings and exits 0.
# Verifies that vendored shared artifacts match the pinned tag in .loopwish/shared.ref.

$ErrorActionPreference = 'Continue'

function Warn([string]$Message) {
    Write-Host "::warning::$Message"
}

$Root = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$RefFile = Join-Path $Root '.loopwish/shared.ref'

if (-not (Test-Path $RefFile)) {
    Warn 'Missing .loopwish/shared.ref (expected single line tag, e.g. v0.1.0)'
    exit 0
}

$Tag = (Get-Content $RefFile -TotalCount 1).Trim()
if ([string]::IsNullOrWhiteSpace($Tag)) {
    Warn '.loopwish/shared.ref is empty'
    exit 0
}

$BaseUrl = "https://raw.githubusercontent.com/loopwish/shared/$Tag"

$Files = @(
    'design-tokens/tokens.json',
    'assets/logos/loopwish-logo.svg',
    'assets/logos/loopwish-banner.svg'
)

foreach ($Rel in $Files) {
    $LocalPath = Join-Path $Root ("vendor/shared/$Rel")
    if (-not (Test-Path $LocalPath)) {
        Warn "Missing vendored file: vendor/shared/$Rel"
        continue
    }

    $Temp = New-TemporaryFile
    try {
        $Url = "$BaseUrl/$Rel"
        try {
            Invoke-WebRequest -Uri $Url -OutFile $Temp.FullName -UseBasicParsing | Out-Null
        } catch {
            Warn "Could not fetch $Url (tag missing or network issue)"
            continue
        }

        $LocalHash = (Get-FileHash $LocalPath -Algorithm SHA256).Hash
        $RemoteHash = (Get-FileHash $Temp.FullName -Algorithm SHA256).Hash
        if ($LocalHash -ne $RemoteHash) {
            Warn "Vendored file differs from shared@${Tag}: vendor/shared/$Rel"
        }
    } finally {
        Remove-Item $Temp.FullName -ErrorAction SilentlyContinue
    }
}

exit 0

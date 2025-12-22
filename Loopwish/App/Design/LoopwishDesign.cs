using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace Loopwish.App.Design;

public enum LoopwishTheme
{
    Light,
    Dark,
}

public enum LoopwishColorRole
{
    TextPrimary,
    TextSecondary,
    TextOnAccent,

    SurfaceCanvas,
    SurfaceElevated,
    SurfaceAccent,

    BorderDefault,
    BorderFocus,

    ActionPrimaryBackground,
    ActionPrimaryForeground,
    ActionSecondaryForeground,
}

public static class LoopwishDesign
{
    public const string BrushTextPrimaryKey = "Loopwish.Brush.Text.Primary";
    public const string BrushTextSecondaryKey = "Loopwish.Brush.Text.Secondary";
    public const string BrushSurfaceCanvasKey = "Loopwish.Brush.Surface.Canvas";
    public const string BrushSurfaceElevatedKey = "Loopwish.Brush.Surface.Elevated";
    public const string BrushBorderDefaultKey = "Loopwish.Brush.Border.Default";
    public const string BrushActionPrimaryBackgroundKey = "Loopwish.Brush.Action.Primary.Background";
    public const string BrushActionPrimaryForegroundKey = "Loopwish.Brush.Action.Primary.Foreground";

    public static void Apply(Application app, LoopwishTheme theme = LoopwishTheme.Light)
    {
        var tokens = TokensCache.Load();

        app.Resources[BrushTextPrimaryKey] = new SolidColorBrush(ParseHex(GetHex(tokens, "color.text.primary", theme)));
        app.Resources[BrushTextSecondaryKey] = new SolidColorBrush(ParseHex(GetHex(tokens, "color.text.secondary", theme)));
        app.Resources[BrushSurfaceCanvasKey] = new SolidColorBrush(ParseHex(GetHex(tokens, "color.surface.canvas", theme)));
        app.Resources[BrushSurfaceElevatedKey] = new SolidColorBrush(ParseHex(GetHex(tokens, "color.surface.elevated", theme)));
        app.Resources[BrushBorderDefaultKey] = new SolidColorBrush(ParseHex(GetHex(tokens, "color.border.default", theme)));
        app.Resources[BrushActionPrimaryBackgroundKey] = new SolidColorBrush(ParseHex(GetHex(tokens, "color.action.primary.bg", theme)));
        app.Resources[BrushActionPrimaryForegroundKey] = new SolidColorBrush(ParseHex(GetHex(tokens, "color.action.primary.fg", theme)));

        FreezeBrushes(app.Resources);
    }

    private static void FreezeBrushes(ResourceDictionary resources)
    {
        foreach (var key in resources.Keys)
        {
            if (resources[key] is SolidColorBrush brush && brush.CanFreeze)
            {
                brush.Freeze();
            }
        }
    }

    private static string GetHex(TokensFile tokens, string semanticKey, LoopwishTheme theme)
    {
        var raw = theme == LoopwishTheme.Dark
            ? tokens.Semantic.Dark.GetValueOrDefault(semanticKey)
            : tokens.Semantic.Light.GetValueOrDefault(semanticKey);

        if (string.IsNullOrWhiteSpace(raw))
        {
            return DefaultHex(semanticKey);
        }

        var resolved = Resolve(raw, tokens);
        return resolved ?? DefaultHex(semanticKey);
    }

    private static string? Resolve(string raw, TokensFile tokens)
    {
        if (raw.StartsWith('#'))
        {
            return raw;
        }

        // Supports aliases like: {primitives.colors.textDark}
        if (raw.StartsWith('{') && raw.EndsWith('}'))
        {
            var path = raw[1..^1];
            const string prefix = "primitives.colors.";
            if (!path.StartsWith(prefix, StringComparison.Ordinal))
            {
                return null;
            }

            var key = path[prefix.Length..];
            return tokens.Primitives.Colors.GetValueOrDefault(key);
        }

        return null;
    }

    private static Color ParseHex(string hex)
    {
        var cleaned = hex.Trim().TrimStart('#');
        if (cleaned.Length != 6)
        {
            return Colors.Black;
        }

        var rgb = int.Parse(cleaned, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var r = (byte)((rgb >> 16) & 0xFF);
        var g = (byte)((rgb >> 8) & 0xFF);
        var b = (byte)(rgb & 0xFF);
        return Color.FromArgb(0xFF, r, g, b);
    }

    private static string DefaultHex(string semanticKey)
    {
        return semanticKey switch
        {
            "color.text.primary" => "#2C3E50",
            "color.text.secondary" => "#7F8C8D",
            "color.surface.canvas" => "#FFFFFF",
            "color.surface.elevated" => "#FFFFFF",
            "color.border.default" => "#7F8C8D",
            "color.action.primary.bg" => "#5DADE2",
            "color.action.primary.fg" => "#FFFFFF",
            _ => "#000000",
        };
    }

    private static class TokensCache
    {
        public static TokensFile Load()
        {
            var path = System.IO.Path.Combine(
                AppContext.BaseDirectory,
                "vendor",
                "shared",
                "design-tokens",
                "tokens.json"
            );

            if (!System.IO.File.Exists(path))
            {
                return TokensFile.Fallback();
            }

            try
            {
                var json = System.IO.File.ReadAllText(path);
                var tokens = JsonSerializer.Deserialize<TokensFile>(json);
                return tokens ?? TokensFile.Fallback();
            }
            catch
            {
                return TokensFile.Fallback();
            }
        }
    }

    private sealed class TokensFile
    {
        public PrimitivesFile Primitives { get; init; } = new();
        public SemanticFile Semantic { get; init; } = new();
        public ContentFile Content { get; init; } = new();

        public static TokensFile Fallback() => new();

        public sealed class PrimitivesFile
        {
            public Dictionary<string, string> Colors { get; init; } = new();
        }

        public sealed class SemanticFile
        {
            public Dictionary<string, string> Light { get; init; } = new();
            public Dictionary<string, string> Dark { get; init; } = new();
        }

        public sealed class ContentFile
        {
            public string Tagline { get; init; } = "Ønsk. Del. Få. Sammen.";
        }
    }
}

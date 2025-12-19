using Loopwish.Core;

namespace Loopwish.Tests;

public class TaglineTests
{
    [Xunit.Fact]
    public void Tagline_is_expected()
    {
        Xunit.Assert.Equal("Ønsk. Del. Få. Sammen.", Tagline.Value);
    }
}

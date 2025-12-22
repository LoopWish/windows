using System.Windows;
using Loopwish.App.Design;

namespace Loopwish.App;

public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		LoopwishDesign.Apply(this, LoopwishTheme.Light);
	}
}

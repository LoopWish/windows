using System.Windows;
using Loopwish.Core;

namespace Loopwish.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Title = $"Loopwish — {Tagline.Value}";
    }
}

using EventAggregator.ViewModels;
using EventAggregator.Views;
using Prism.Ioc;
using System.Windows;

namespace EventAggregator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<TextDialog, TextDialogViewModel>("TextDialog");   
        }
    }
}

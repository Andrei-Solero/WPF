using Prism.Ioc;
using PrismWpf.Services;
using PrismWpf.Views;
using System.Windows;

namespace PrismWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<ICustomerStore, CustomerStore>();
        }
    }
}

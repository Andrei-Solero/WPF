using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPrismUnity.ViewModels;
using TestPrismUnity.Views;

namespace TestPrismUnity
{
    public class ModuleMain : IModule
    {
        private readonly IRegionManager regionManager;

        public ModuleMain(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindowViewA>("ViewA");
            containerRegistry.RegisterForNavigation<MainWindowViewB>("ViewB");
        }
    }
}

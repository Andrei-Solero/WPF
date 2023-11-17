using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPrismUnity.Views;

namespace TestPrismUnity
{
    public class ModuleViewAContent : IModule
    {
        private readonly IRegionManager regionManager;

        public ModuleViewAContent(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<View1>("View1");
            containerRegistry.RegisterForNavigation<View2>("View2");
            containerRegistry.RegisterForNavigation<View3>("View3");
        }
    }
}

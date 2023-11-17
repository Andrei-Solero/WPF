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
    public class ModuleViewA : IModule
    {
        private readonly IRegionManager regionManager;

        public ModuleViewA(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion("ViewARegion", typeof(ViewADialogContent));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ViewADialog, ViewADialogViewModel>("ViewADialog");
        }
    }
}

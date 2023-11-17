using IMTE.ViewModels;
using IMTE.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE
{
    public class IMTEModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SplashScreen>("");
            containerRegistry.RegisterForNavigation<MeasuringDeviceList>("List");
            containerRegistry.RegisterForNavigation<MeasuringDeviceForm>("Create");

            containerRegistry.RegisterForNavigation<EmployeeConfigLookupList>("EmpConfigList");

            containerRegistry.RegisterDialog<EmployeeConfigLookup, EmployeeConfigLookupViewModel>("EmpConfig");
        }
    }
}

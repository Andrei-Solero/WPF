using Prism.Ioc;
using Prism.Modularity;
using PRISM_Structured.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRISM_Structured
{
    public class IMTEModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AddMeasuringDevice>("Create");
            containerRegistry.RegisterForNavigation<MeasuringDevicesList>("Read");
        }
    }
}

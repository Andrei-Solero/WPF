using IMTE.ViewModels;
using IMTE.Views;
using Prism.Events;
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
            containerRegistry.RegisterDialog<MDLookup_EmployeeConfig, MDLookup_EmployeeConfigViewModel>("EmpConfig");
            containerRegistry.RegisterDialog<MDLookup_DepartmentConfig, MDLookup_DepartmentConfigViewModel>("DeptConfig");
            containerRegistry.RegisterDialog<MDLookup_PlantConfig, MDLookup_PlantConfigViewModel>("PlantConfig");
            containerRegistry.RegisterDialog<MDLookup_ItemDescriptionConfig, MDLookup_ItemDescriptionConfigViewModel>("ItemDescriptionConfig");
            containerRegistry.RegisterDialog<MDLookup_ItemConfig, MDLookup_ItemConfigViewModel>("ItemConfig");
            containerRegistry.RegisterDialog<MD_EquipmentFormLookup, MD_EquipmentFormLookupViewModel>("EquipmentConfig");
            containerRegistry.RegisterDialog<MD_MachineToolFormLookup, MD_MachineToolFormLookupViewModel>("MachineToolConfig");
            containerRegistry.RegisterDialog<MDLookup_UnitConfig, MDLookup_UnitConfigViewModel>("UnitConfig");

            containerRegistry.RegisterForNavigation<SplashScreen>("");
            containerRegistry.RegisterForNavigation<MeasuringDeviceList>("List");
            containerRegistry.RegisterForNavigation<MeasuringDeviceForm>("Create");

            containerRegistry.RegisterForNavigation<MDLookup_EmployeeConfigList>("EmpConfigList");
            containerRegistry.RegisterForNavigation<MDLookup_EmployeeConfigCreate>("EmpConfigCreate");

            containerRegistry.RegisterForNavigation<MDLookup_DepartmentConfigList>("DeptConfigList");
            containerRegistry.RegisterForNavigation<MDLookup_DepartmentConfigCreate>("DeptConfigCreate");

            containerRegistry.RegisterForNavigation<MDLookup_ItemDescriptionConfigList>("ItemDescriptionConfigList");

            containerRegistry.RegisterForNavigation<MDLookup_ItemConfigList>("ItemConfigList");

            containerRegistry.RegisterForNavigation<MD_EquipmentForm>("EquipmentFormForMeasuringDevice");
            containerRegistry.RegisterForNavigation<MD_MachineToolForm>("MachineToolForMeasuringDevice");

            containerRegistry.RegisterForNavigation<MD_EquipmentForLookupList>("EquipmentList");
            containerRegistry.RegisterForNavigation<MD_MachineToolFormLookupList>("MachineToolList");

            containerRegistry.RegisterForNavigation<MDLookup_UnitConfigList>("UnitConfigList");
            containerRegistry.RegisterForNavigation<MDLookup_UnitConfigCreate>("UnitConfigCreate");

        }
    }
}

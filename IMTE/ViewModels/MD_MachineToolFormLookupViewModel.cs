using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MD_MachineToolFormLookupViewModel : DialogAwareBase
    {
        private readonly IRegionManager regionManager;

        public MD_MachineToolFormLookupViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            regionManager.RegisterViewWithRegion("MachineToolRegion", "MachineToolList");
        }
    }
}

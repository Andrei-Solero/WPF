using IMTE.EventAggregator.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MDLookup_UnitConfigViewModel : DialogAwareBase
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand PassSelectedObjToMDFormCommand { get; private set; }
        public DelegateCommand OpenCreateCommand { get; private set; }
        public DelegateCommand OpenUnitListCommand { get; private set; }

        public MDLookup_UnitConfigViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            regionManager.RegisterViewWithRegion("UnitConfigRegion", "UnitConfigList");

            OpenCreateCommand = new DelegateCommand(OpenCreate);
            OpenUnitListCommand = new DelegateCommand(OpenList);
        }

        private void OpenList()
        {
            regionManager.RequestNavigate("UnitConfigRegion", "UnitConfigList");
        }

        private void OpenCreate()
        {
            regionManager.RequestNavigate("UnitConfigRegion", "UnitConfigCreate");
        }
    }
}

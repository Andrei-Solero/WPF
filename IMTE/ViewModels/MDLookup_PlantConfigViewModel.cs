using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MDLookup_PlantConfigViewModel : DialogAwareBase
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand OpenCreateCommand { get; private set; }
        public DelegateCommand OpenListCommand { get; private set; }

        public MDLookup_PlantConfigViewModel(IRegionManager plantRegionManager)
        {
            this.regionManager = plantRegionManager;
            plantRegionManager.RegisterViewWithRegion("PlantConfigRegion", "PlantConfigList");

            OpenCreateCommand = new DelegateCommand(OpenCreate);
            OpenListCommand = new DelegateCommand(OpenList);

        }

        private void OpenList()
        {
            regionManager.RequestNavigate("PlantConfigRegion", "PlantConfigList");
        }

        private void OpenCreate()
        {
            regionManager.RequestNavigate("PlantConfigRegion", "PlantConfigCreate");
        }

        #region Full Property

        private Plant _selectedPlant = new Plant();

        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set { SetProperty(ref _selectedPlant, value); }
        }


        #endregion

    }
}

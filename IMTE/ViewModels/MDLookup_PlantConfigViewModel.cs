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
    public class MDLookup_PlantConfigViewModel : BindableBase, IDialogAware
    {
        private readonly IRegionManager plantRegionManager;


        public MDLookup_PlantConfigViewModel(IRegionManager plantRegionManager)
        {
            this.plantRegionManager = plantRegionManager;
            plantRegionManager.RegisterViewWithRegion("PlantConfigRegion", "PlantConfigList");

        }

        #region Full Property

        private Plant _selectedPlant = new Plant();

        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set { SetProperty(ref _selectedPlant, value); }
        }


        #endregion

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        private void OpenDialog()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}

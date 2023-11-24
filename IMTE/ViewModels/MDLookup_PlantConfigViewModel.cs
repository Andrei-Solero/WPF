using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MDLookup_PlantConfigViewModel : IDialogAware
    {
        public IRegionManager PlantRegionManager { get; set; }

        public MDLookup_PlantConfigViewModel(IRegionManager plantRegionManager)
        {
            PlantRegionManager = plantRegionManager;
            
        }


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
            PlantRegionManager.CreateRegionManager();
        }
    }
}

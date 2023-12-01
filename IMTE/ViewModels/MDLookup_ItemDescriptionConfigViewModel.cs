using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MDLookup_ItemDescriptionConfigViewModel : IDialogAware
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand OpenCreateDescriptionCommand { get; private set; }
        public DelegateCommand OpenListDescriptionCommand { get; private set; }

        public MDLookup_ItemDescriptionConfigViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            regionManager.RegisterViewWithRegion("ItemDescriptionRegionMain", "ItemDescriptionConfigList");

            OpenCreateDescriptionCommand = new DelegateCommand(OpenCreateDescription);
            OpenListDescriptionCommand = new DelegateCommand(OpenListDescription);
        }

        private void OpenListDescription()
        {
            regionManager.RequestNavigate("ItemDescriptionRegionMain", "ItemDescriptionConfigList");
        }

        private void OpenCreateDescription()
        {
            regionManager.RequestNavigate("ItemDescriptionRegionMain", "ItemDescriptionConfigCreate");
        }

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}

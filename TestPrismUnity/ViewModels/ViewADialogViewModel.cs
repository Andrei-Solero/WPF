using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPrismUnity.ViewModels
{
    public class ViewADialogViewModel : IDialogAware
    {
        private readonly IRegionManager regionManager;

        public ViewADialogViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
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

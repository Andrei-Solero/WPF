using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPrismUnity.ViewModels
{
    public class ViewADialogContentViewModel
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand Open1Command { get; set; }

        public ViewADialogContentViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            Open1Command = new DelegateCommand(OpenOne);
            regionManager.CreateRegionManager();
        }

        private void OpenOne()
        {
            regionManager.RequestNavigate("ViewAContentRegion", "View1");
        }
    }
}

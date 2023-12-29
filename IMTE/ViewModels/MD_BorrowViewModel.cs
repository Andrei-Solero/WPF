using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MD_BorrowViewModel
    {
        private readonly IRegionManager regionManager;

        public MD_BorrowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            regionManager.RegisterViewWithRegion("BorrowMainRegion", "BorrowList");
        }

        
    }
}

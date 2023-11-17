using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPrismUnity.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly IRegionManager regionManager;


        public DelegateCommand OpenWindowACommand { get; private set; }
        public DelegateCommand OpenWindowBCommand { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            OpenWindowACommand = new DelegateCommand(OpenWindowA);
            OpenWindowBCommand = new DelegateCommand(OpenWindowB);
        }

        private void OpenWindowB()
        {
            regionManager.RequestNavigate("MainRegion", "ViewB");
        }

        private void OpenWindowA()
        {
            regionManager.RequestNavigate("MainRegion", "ViewA");
        }
    }
}

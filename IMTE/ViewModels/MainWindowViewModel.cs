using IMTE.IMTEEntity.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand<string> NavigateCommand { get; set; }

        private MeasuringDevice _measuringDevice;
        private readonly IRegionManager regionManager;

        public MeasuringDevice MeasuringDevice
        {
            get { return _measuringDevice; }
            set { SetProperty(ref _measuringDevice, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string uriKey)
        {
            regionManager.RequestNavigate("MainRegion", uriKey);
        }
    }
}

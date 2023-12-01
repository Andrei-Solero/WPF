using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MeasuringDeviceListViewModel : BindableBase
    {
        private readonly MeasuringDeviceDA measuringDeviceDA;
        private readonly IDialogService dialogService;
        private readonly IRegionManager regionManager;

        private ObservableCollection<MeasuringDevice> _measuringDeviceList;
        public ObservableCollection<MeasuringDevice> MeasuringDeviceList
        {
            get { return _measuringDeviceList; }
            set { SetProperty(ref _measuringDeviceList, value); }
        }

        private MeasuringDevice _selectedMeasuringDevice = new MeasuringDevice();
        public MeasuringDevice SelectedMeasuringDevice
        {
            get { return _selectedMeasuringDevice; }
            set { SetProperty(ref _selectedMeasuringDevice, value); }
        }

        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand OpenFormBySelectedObjectCommand { get; }
        public DelegateCommand NavigateToFormCommand { get; set; }

        public MeasuringDeviceListViewModel(IDialogService dialogService, IRegionManager regionManager)
        {
            measuringDeviceDA = new MeasuringDeviceDA();

            OpenFormBySelectedObjectCommand = new DelegateCommand(OpenFormBySelectedObject);
            NavigateToFormCommand = new DelegateCommand(Navigate);
            RefreshCommand = new DelegateCommand(Refresh);

            MeasuringDeviceList = new ObservableCollection<MeasuringDevice>(measuringDeviceDA.GetAllMeasuringDevices());
            this.dialogService = dialogService;
            this.regionManager = regionManager;
        }

        private void Refresh()
        {
            MeasuringDeviceList = new ObservableCollection<MeasuringDevice>(measuringDeviceDA.GetAllMeasuringDevices());
            SetProperty(ref _measuringDeviceList, MeasuringDeviceList);
        }

        private void Navigate()
        {
            regionManager.RequestNavigate("MainRegion", "Create");
        }

        public MeasuringDeviceListViewModel()
        {
            measuringDeviceDA = new MeasuringDeviceDA();
            MeasuringDeviceList = new ObservableCollection<MeasuringDevice>(measuringDeviceDA.GetAllMeasuringDevices());
        }

        public void OpenFormBySelectedObject()
        {
            var dialogParameters = new NavigationParameters();
            dialogParameters.Add("measuringDeviceObj", _selectedMeasuringDevice);

            regionManager.RequestNavigate("MainRegion", "Create", dialogParameters);
        }
    }
}

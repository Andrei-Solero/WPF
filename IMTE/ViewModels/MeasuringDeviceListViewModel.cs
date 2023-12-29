using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Views;
using MaterialDesignThemes.Wpf;
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

            this.dialogService = dialogService;
            this.regionManager = regionManager;

            Task.Run(async () => await LoadMDToListAsync());
        }

        private async Task LoadMDToListAsync()
        {
            MeasuringDeviceList = new ObservableCollection<MeasuringDevice>(await measuringDeviceDA.GetAllMeasuringDevices());
        }

        private void Refresh()
        {
            Task.Run(async () => await LoadMDToListAsync());
        }

        private void Navigate()
        {
            regionManager.RequestNavigate("MainIMTERegion", "Create");
        }

        public void OpenFormBySelectedObject()
        {
            var dialogParameters = new NavigationParameters();
            dialogParameters.Add("measuringDeviceObj", SelectedMeasuringDevice);

            regionManager.RequestNavigate("MainIMTERegion", "Create", dialogParameters);
        }
    }
}

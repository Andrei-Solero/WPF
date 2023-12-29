using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Models.IMTEEntity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.ViewModels
{
    public class MD_BorrowListViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogService dialogService;
        private readonly MeasuringDeviceLedgerDA measuringDeviceLedgerDA;
        private readonly MeasuringDeviceDA measuringDeviceDA;


        public DelegateCommand NavigateToFormCommand { get; private set; }
        public DelegateCommand OpenLedgerTransactionCommand { get; private set; }

        public MD_BorrowListViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            measuringDeviceLedgerDA = new MeasuringDeviceLedgerDA();
            measuringDeviceDA = new MeasuringDeviceDA();

            NavigateToFormCommand = new DelegateCommand(NavigateToForm);
            OpenLedgerTransactionCommand = new DelegateCommand(OpenLedgerTransaction);

            this.regionManager = regionManager;
            this.dialogService = dialogService;
            Task.Run(async () => await LoadMeasuringDeviceLedgers());
        }

        private async void OpenLedgerTransaction()
        {
            var dialogParameters = new DialogParameters();

            await Task.Run(async () => {
                var result = await measuringDeviceLedgerDA.GetMeasuringDeviceLedgers(SelectedMeasuringDevice);
                MeasuringDeviceLedgers = new ObservableCollection<MeasuringDeviceLedger>(result);

                dialogParameters.Add("measuringDeviceLedgerObj", MeasuringDeviceLedgers);
                dialogParameters.Add("measuringDeviceObj", SelectedMeasuringDevice);

                if (MeasuringDeviceLedgers.Count < 1)
                    MessageBox.Show("No Ledgers found");

            });

            dialogService.ShowDialog("MD_Borrow_LedgerTransaction", dialogParameters, null);

        }

        private async Task LoadMeasuringDeviceLedgers()
        {
            try
            {
                var measuringDevices = await measuringDeviceDA.GetAllMeasuringDevices();
                MeasuringDevices = new ObservableCollection<MeasuringDevice>(measuringDevices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void NavigateToForm()
        {
            regionManager.RequestNavigate("BorrowMainRegion", "BorrowForm");
        }

        #region Full Properties

        private MeasuringDeviceLedger _selectedMeasuringDeviceLedger = new MeasuringDeviceLedger();
        public MeasuringDeviceLedger SelectedMeasuringDeviceLedger
        {
            get { return _selectedMeasuringDeviceLedger; }
            set { SetProperty(ref _selectedMeasuringDeviceLedger, value); }
        }

        private MeasuringDevice _selectedMeasuringDevice = new MeasuringDevice();
        public MeasuringDevice SelectedMeasuringDevice
        {
            get { return _selectedMeasuringDevice; }
            set { SetProperty(ref _selectedMeasuringDevice, value); }
        }


        #endregion

        #region Observable Collections

        private ObservableCollection<MeasuringDevice> _measuringDevices;
        public ObservableCollection<MeasuringDevice> MeasuringDevices
        {
            get { return _measuringDevices; }
            set { SetProperty(ref _measuringDevices, value); }
        }

        private ObservableCollection<MeasuringDeviceLedger> _measuringDeviceLedgers;
        public ObservableCollection<MeasuringDeviceLedger> MeasuringDeviceLedgers
        {
            get { return _measuringDeviceLedgers; }
            set { SetProperty(ref _measuringDeviceLedgers, value); }
        }

        #endregion
    }
}

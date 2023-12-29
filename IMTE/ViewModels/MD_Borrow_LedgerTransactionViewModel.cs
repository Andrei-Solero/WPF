using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.IMTEEntity;
using IMTE.ViewModels.FieldBindings;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MD_Borrow_LedgerTransactionViewModel : MeasuringDeviceFieldBindingDialog
    {
        private string _deviceTypeName;
        public string DeviceTypeName
        {
            get { return _deviceTypeName; }
            set { SetProperty(ref _deviceTypeName, value); }
        }


        public MD_Borrow_LedgerTransactionViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            string regionName = "MeasuringDeviceToolRegion";

            MeasuringDevice = parameters.GetValue<MeasuringDevice>("measuringDeviceObj");
            MeasuringDeviceLedgers = new ObservableCollection<MeasuringDeviceLedger>( parameters.GetValue<IEnumerable<MeasuringDeviceLedger>>("measuringDeviceLedgerObj"));

            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("EquipmentSerialObj", MeasuringDevice.EquipmentSerial);
            navigationParameters.Add("InstrumentSerialObj", MeasuringDevice.InstrumentSerial);

            if (MeasuringDevice.EquipmentSerial != null)
            {
                regionManager.RequestNavigate(regionName, "LedgerEquipment", navigationParameters);
                DeviceTypeName = "Equipment";
            }
                
            else if (MeasuringDevice.InstrumentSerial != null)
            {
                regionManager.RequestNavigate(regionName, "LedgerInstrument", navigationParameters);
                DeviceTypeName = "Instrument";
            }

            SetFieldBinding(MeasuringDevice);
        }

        #region Helper

        private void SetFieldBinding(MeasuringDevice measuringDeviceObj)
        {
            SerialNo = measuringDeviceObj.SerialNo;
            Department = measuringDeviceObj.Department;
            Location = measuringDeviceObj.Location;
            Plant = measuringDeviceObj.Plant;
            DeviceType = measuringDeviceObj.DeviceType;
            Description = measuringDeviceObj.Description;
            CalibrationMethod = measuringDeviceObj.CalibrationMethod;
            FrequencyOfCalibration = measuringDeviceObj._FrequencyOfCalibration;
            NextCalibrationDate = measuringDeviceObj.NextCalibrationDate;
            Status = measuringDeviceObj.Status;
            Barcode = measuringDeviceObj.Barcode;
            Remarks = measuringDeviceObj.Remarks;
            EndOfLife = measuringDeviceObj.EndOfLife;
            Maker = measuringDeviceObj.Maker;
            AcceptanceCriteria = measuringDeviceObj.AcceptanceCriteria;
            Resolution = measuringDeviceObj.Resolution;
            DeviceRange = measuringDeviceObj.DeviceRange;
            Accuracy = measuringDeviceObj.Accuracy;
            Unit = measuringDeviceObj.Unit;
        }

        #endregion

        #region Full Property

        private MeasuringDevice _measuringDevice = new MeasuringDevice();
        public MeasuringDevice MeasuringDevice
        {
            get { return _measuringDevice; }
            set { SetProperty(ref _measuringDevice, value); }
        }

        #endregion

        #region Observable Collections

        private ObservableCollection<MeasuringDeviceLedger> _measuringDeviceLedgers;
        private readonly IRegionManager regionManager;

        public ObservableCollection<MeasuringDeviceLedger> MeasuringDeviceLedgers
        {
            get { return _measuringDeviceLedgers; }
            set { SetProperty(ref _measuringDeviceLedgers, value); }

        }


        #endregion

    }
}

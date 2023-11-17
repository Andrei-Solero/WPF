using IMTE_dotnetframework.Commands;
using IMTE_dotnetframework.DataAccess;
using IMTE_dotnetframework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IMTE_dotnetframework.ViewModels
{
    public class UpdateMeasuringDeviceViewModel : BaseViewModel
    {
        public ICommand UpdateMeasuringDeviceCommand { get; private set; }
        private readonly MeasuringDeviceDA measuringDeviceDA;

        private MeasuringDevices _measuringDevice;

        public MeasuringDevices MeasuringDevice
        {
            get { return _measuringDevice; }
            set 
            { 
                _measuringDevice = value;
                OnPropertyChanged();
            }
        }

        public UpdateMeasuringDeviceViewModel()
        {
            UpdateMeasuringDeviceCommand = new BaseCommand(UpdateMeasuringDevice, CanUpdateMeasuringDevice);
            measuringDeviceDA = new MeasuringDeviceDA();
        }

        private bool CanUpdateMeasuringDevice(object obj)
        {
            return true;
        }

        private void UpdateMeasuringDevice(object obj)
        {
            measuringDeviceDA.UpdateSelectedMeasuringDevice(new MeasuringDevices
            {
                Id = _measuringDevice.Id,
                Version = _measuringDevice.Version,
                Type = _measuringDevice.Type,
                FrequencyOfCalibration = _measuringDevice.FrequencyOfCalibration,
                LastCalibrationDate = _measuringDevice.LastCalibrationDate,
                ResultOfCalibration = _measuringDevice.ResultOfCalibration,
                NextCalibrationDate = _measuringDevice.NextCalibrationDate,
                Status = _measuringDevice.Status,
                CalibrationRemarks = _measuringDevice.CalibrationRemarks,
                TrgTpgAndSettingsRemarks = _measuringDevice.TrgTpgAndSettingsRemarks,
                Remarks = _measuringDevice.Remarks,
                Date = _measuringDevice.Date,
                Maker = _measuringDevice.Maker,
                Resolution = _measuringDevice.Resolution,
                DeviceRange = _measuringDevice.DeviceRange,
                Accuracy = _measuringDevice.Accuracy,
                UnitOfMeasurement = _measuringDevice.UnitOfMeasurement,
                Barcode = _measuringDevice.Barcode,
                CalibrationMethod = _measuringDevice.CalibrationMethod
            });

            MessageBox.Show("Successfully Deleted");
        }
    }
}

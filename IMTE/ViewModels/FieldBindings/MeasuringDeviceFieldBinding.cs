using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.IMTEEntity;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels.FieldBindings
{
    public class MeasuringDeviceFieldBinding : BindableBase
    {
        #region ------------ FIELD BINDING ---------------

        private string _serialNo;
        public string SerialNo
        {
            get { return _serialNo; }
            set
            {
                SetProperty(ref _serialNo, value);
            }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set
            {
                SetProperty(ref _department, value);
            }
        }

        private Location _location = new Location();
        public Location Location
        {
            get { return _location; }
            set
            {
                SetProperty(ref _location, value);
            }
        }

        private Plant _plant = new Plant();
        public Plant Plant
        {
            get { return _plant; }
            set
            {
                SetProperty(ref _plant, value);
            }
        }

        private string _calibrationMethod;
        public string CalibrationMethod
        {
            get { return _calibrationMethod; }
            set
            {
                SetProperty(ref _calibrationMethod, value);
            }
        }

        private DeviceType _deviceType = new DeviceType();
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

        private AcceptanceCriteria _acceptanceCriteria = new AcceptanceCriteria();
        public AcceptanceCriteria AcceptanceCriteria
        {
            get { return _acceptanceCriteria; }
            set
            {
                SetProperty(ref _acceptanceCriteria, value);
            }
        }

        private FrequencyOfCalibration _frequencyOfCalibration = new FrequencyOfCalibration();
        public FrequencyOfCalibration FrequencyOfCalibration
        {
            get { return _frequencyOfCalibration; }
            set
            {
                SetProperty(ref _frequencyOfCalibration, value);
            }
        }

        private DateTime? _nextCalibrationDate = DateTime.UtcNow.Date;
        public DateTime? NextCalibrationDate
        {
            get { return _nextCalibrationDate; }
            set
            {
                SetProperty(ref _nextCalibrationDate, value);
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        private string _barcode;
        public string Barcode
        {
            get { return _barcode; }
            set
            {
                SetProperty(ref _barcode, value);
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                SetProperty(ref _remarks, value);
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
            }
        }

        private string _resolution;
        public string Resolution
        {
            get { return _resolution; }
            set
            {
                SetProperty(ref _resolution, value);
            }
        }

        private string _maker;
        public string Maker
        {
            get { return _maker; }
            set
            {
                SetProperty(ref _maker, value);
            }
        }


        private string _deviceRange;
        public string DeviceRange
        {
            get { return _deviceRange; }
            set
            {
                SetProperty(ref _deviceRange, value);
            }
        }

        private string _accuracy;
        public string Accuracy
        {
            get { return _accuracy; }
            set
            {
                SetProperty(ref _accuracy, value);
            }
        }

        private UnitEntity _unit = new UnitEntity();
        public UnitEntity Unit
        {
            get { return _unit; }
            set
            {
                SetProperty(ref _unit, value);
            }
        }

        private DateTime? _endOfLife = DateTime.UtcNow;
        public DateTime? EndOfLife
        {
            get { return _endOfLife; }
            set
            {
                SetProperty(ref _endOfLife, value);
            }
        }

        private EquipmentSerial _equipmentSerial = new EquipmentSerial();
        public EquipmentSerial EquipmentSerial
        {
            get { return _equipmentSerial; }
            set
            {
                SetProperty(ref _equipmentSerial, value);
            }
        }

        private InstrumentSerial _instrumentSerial = new InstrumentSerial();

        public InstrumentSerial InstrumentSerial
        {
            get { return _instrumentSerial; }
            set
            {
                SetProperty(ref _instrumentSerial, value);
            }
        }

        private MachineToolSerial _machineToolSerial = new MachineToolSerial();
        public MachineToolSerial MachineToolSerial
        {
            get { return _machineToolSerial; }
            set
            {
                SetProperty(ref _machineToolSerial, value);
            }
        }


        #endregion
    }
}

using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.IMTEEntity;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
using IMTE.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Unity;

namespace IMTE.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class MeasuringDeviceFormViewModel : BindableBase, INavigationAware, IDataErrorInfo
    {
        private readonly MeasuringDeviceDA measuringDeviceDA;
        private readonly EmployeeDA employeeDA;
        private readonly DepartmentDA departmentDA;
        private readonly LocationDA locationDA;
        private readonly UnitDA unitDA;
        private readonly PlantDA plantDA;
        private readonly EquipmentTypeDA equipmentTypeDA;
        private readonly InstrumentSerialDA instrumentSerialDA;
        private readonly AcceptanceCriteriaDA acceptanceCriteriaDA;
        private readonly FrequencyOfCalibrationDA frequencyOfCalibrationDA;
        private readonly ResolutionDA resolutionDA;
        private readonly MakerDA makerDA;
        private readonly IRegionManager regionManager;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator ea;

        #region IDataErrorInfo

        private Dictionary<string, string> _errorCollection = new Dictionary<string, string>();
        public Dictionary<string, string> ErrorCollection
        {
            get { return _errorCollection; }
            set { SetProperty(ref _errorCollection, value); }
        }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string result = null;
                string errorTextForCmb = "Select required option...";
                string errorTextForText = "Required...";

                switch (columnName)
                {
                    case "SerialNo":
                        if (string.IsNullOrWhiteSpace(SerialNo))
                            result = errorTextForText;
                        break;
                    case "Department":
                        if (Department == null || Department.Id == null || Department.Id == 0)
                            result = errorTextForCmb;
                        break;
                    case "Location":
                        if (Location == null || Location.Id == null || Location.Id == 0)
                            result = errorTextForCmb;
                        break;
                    case "Plant":
                        if (Plant == null || Plant.Id == null || Plant.Id == 0)
                            result = errorTextForCmb;
                        break;
                    case "Description":
                        if (string.IsNullOrWhiteSpace(Description))
                            result = errorTextForText;
                        break;
                    case "CalibrationMethod":
                        if (string.IsNullOrWhiteSpace(CalibrationMethod))
                            result = errorTextForCmb;
                        break;
                    case "AcceptanceCriteria":
                        if (AcceptanceCriteria == null || AcceptanceCriteria.Id == null || AcceptanceCriteria.Id == 0)
                            result = errorTextForCmb;
                        break;
                    case "FrequencyOfCalibration":
                        if (FrequencyOfCalibration == null || FrequencyOfCalibration.Id == null || FrequencyOfCalibration.Id == 0)
                            result = errorTextForCmb;
                        break;
                    case "Remarks":
                        if (string.IsNullOrWhiteSpace(Remarks))
                            result = errorTextForText;
                        break;
                    case "Barcode":
                        if (string.IsNullOrWhiteSpace(Barcode))
                            result = errorTextForText;
                        break;
                    case "Status":
                        if (string.IsNullOrWhiteSpace(Status))
                            result = errorTextForCmb;
                        break;
                    case "Resolution":
                        if (string.IsNullOrWhiteSpace(Resolution))
                            result = errorTextForText;
                        break;
                    case "Maker":
                        if (string.IsNullOrWhiteSpace(Maker))
                            result = errorTextForText;
                        break;
                    case "Accuracy":
                        if (string.IsNullOrWhiteSpace(Accuracy))
                            result = errorTextForText;
                        break;
                    case "DeviceRange":
                        if (string.IsNullOrWhiteSpace(DeviceRange))
                            result = errorTextForText;
                        break;
                    case "Unit":
                        if (Unit.Id == null || Unit.Id == 0)
                            result = errorTextForCmb;
                        break;

                }

                if (ErrorCollection.ContainsKey(columnName))
                    ErrorCollection[columnName] = result;
                else if (result != null)
                    ErrorCollection.Add(columnName, result);

                SetProperty(ref _errorCollection, ErrorCollection);


                return result;
            }
        }

        #endregion

        #region -------------- DELEGATE COMMANDS --------------

        private DelegateCommand _saveChangesCommand;
        public DelegateCommand SaveChangesCommand
        {
            get { return _saveChangesCommand; }
            set { SetProperty(ref _saveChangesCommand, value); }
        }

        public DelegateCommand<MeasuringDevice> SaveMeasuringDeviceCommand { get; private set; }
        public DelegateCommand ToUpdateMeasringDeviceCommand { get; private set; }
        public DelegateCommand UpdateMeasuringDeviceCommand { get; private set; }
        public DelegateCommand DeleteMeasuringDeviceCommand { get; private set; }
        public DelegateCommand CreateNewMeasuringDevice { get; private set; }
        public DelegateCommand<string> NavigateBackToList { get; private set; }
        public DelegateCommand IssuedToEmployeeLookupCommand { get; private set; }
        public DelegateCommand CalibratedByEmployeeLookupCommand { get; private set; }
        public DelegateCommand DepartmentConfigLookupCommand { get; private set; }
        public DelegateCommand PlantConfigLookupCommand { get; private set; }
        public DelegateCommand EquipmentSelectionCommand { get; private set; }
        public DelegateCommand MachineToolSelectionCommand { get; private set; }
        public DelegateCommand InstrumentSelectionCommand { get; private set; }
        public DelegateCommand OpenUnitConfigLookupCommand { get; private set; }
        public DelegateCommand InstrumentToolSelectionCommand { get; private set; }

        #endregion

        private MeasuringDevice _currentMeasuringDevice = new MeasuringDevice();
        public MeasuringDevice CurrentMeasuringDevice
        {
            get { return _currentMeasuringDevice; }
            set
            {
                SetProperty(ref _currentMeasuringDevice, value);

                Department = value.Department;
                Location = value.Location;
                Plant = value.Plant;
                Unit = value.Unit;
            }
        }

        private SolidColorBrush _saveButtonColor;
        public SolidColorBrush SaveButtonColor
        {
            get { return _saveButtonColor; }
            set { SetProperty(ref _saveButtonColor, value); }
        }

        /// <summary>
        /// Will send the tool data from the measuring device list to the tool form
        /// </summary>
        private void EventAggregatorPublish(MeasuringDevice tool)
        {
            if (tool.InstrumentSerial != null)
            {
                // pass the data from the measuring device to the instrument serial form
                ea.GetEvent<MeasuringDeviceToInstrumentSerial>().Publish(tool.InstrumentSerial);
            }
            else if (tool.EquipmentSerial != null)
            {
                // pass the data from the measuring device to the equipment serial form
                ea.GetEvent<EquipmentSerialToMeasuringDevice>().Publish(tool.EquipmentSerial);
            }
            else if (tool.MachineToolSerial != null)
            {
                // pass the data from the measuring device to the machine tool serial form
                ea.GetEvent<MachineToolSerialToMeasuringDevice>().Publish(tool.MachineToolSerial);
            }
        }


        private void EventAggregatorSubscribe()
        {
            // Will receive the data from employee lookup
            ea.GetEvent<EmployeeLookupToMDForm>().Subscribe(SetEmployeeDataFromLookup);

            // Will receive the data from the unit lookup
            ea.GetEvent<UnitToMDForm>().Subscribe(SetUnitFromLookup);

            // Will receive the data from the department lookup
            ea.GetEvent<DepartmentLookupToMDForm>().Subscribe(SetDepartmentFromLookup);

            ea.GetEvent<PlantLookupToMDForm>().Subscribe(SetPlantFromLookup);

            // Will receive the data from equipment form in this form
            ea.GetEvent<EquipmentToMeasuringDevice>().Subscribe(SetEquipmentDetails);

            // Will receive the data from machine tool in this form
            ea.GetEvent<MachineToolToMeasuringDevice>().Subscribe(SetMachineToolDetails);

            // Will receive data from equipment serial form
            ea.GetEvent<EquipmentSerialToMeasuringDevice>().Subscribe(SetEquipmentSerial);

            // Will receive data from instrument serial form
            ea.GetEvent<InstrumentSerialToMeasuringDevice>().Subscribe(SetInstrumentSerial);

			// Will receive data from machine tool serial form
			ea.GetEvent<MachineToolSerialToMeasuringDevice>().Subscribe(SetMachineToolSerial);

            ea.GetEvent<ToolFormValidationToMeasuringDevice>().Subscribe(SetIsFormValid);
        }

		private void SetIsFormValid(bool obj)
        {
            IsToolFormValid = obj;
			RaiseCanExecuteForSaveChangesCommand();

			RequiredValidation();
		}

		/// <summary>
		/// Set the current measuring device machine tool serial from the machine tool serial form
		/// </summary>
		/// <param name="obj"></param>
		private void SetMachineToolSerial(MachineToolSerial obj)
        {
            CurrentMeasuringDevice.MachineToolSerial = obj;
        }

        /// <summary>
        /// Set the current measuring device instrument serial from the instrument form
        /// </summary>
        /// <param name="obj"></param>
        private void SetInstrumentSerial(InstrumentSerial obj)
        {
            CurrentMeasuringDevice.InstrumentSerial = obj;
        }

        /// <summary>
        /// Set the current measuring device Equipment serial from the equipment form
        /// </summary>
        /// <param name="obj"></param>
        private void SetEquipmentSerial(EquipmentSerial obj)
        {
            CurrentMeasuringDevice.EquipmentSerial = obj;
        }


        #region Helper

        private void SetFieldBindingValue(MeasuringDevice measuringDeviceObj)
        {
            CurrentMeasuringDevice = measuringDeviceObj;

            SerialNo = CurrentMeasuringDevice.SerialNo;
            Department = CurrentMeasuringDevice.Department;
            Location = CurrentMeasuringDevice.Location;
            Plant = CurrentMeasuringDevice.Plant;
            Description = CurrentMeasuringDevice.Description;
            CalibrationMethod = CurrentMeasuringDevice.CalibrationMethod;
            FrequencyOfCalibration = CurrentMeasuringDevice._FrequencyOfCalibration;
            NextCalibrationDate = CurrentMeasuringDevice.NextCalibrationDate;
            Status = CurrentMeasuringDevice.Status;
            Barcode = CurrentMeasuringDevice.Barcode;
            Remarks = CurrentMeasuringDevice.Remarks;
            EndOfLife = CurrentMeasuringDevice.EndOfLife;
            Maker = CurrentMeasuringDevice.Maker;
            AcceptanceCriteria = CurrentMeasuringDevice.AcceptanceCriteria;
            Resolution = CurrentMeasuringDevice.Resolution;
            DeviceRange = CurrentMeasuringDevice.DeviceRange;
            Accuracy = CurrentMeasuringDevice.Accuracy;
            Unit = CurrentMeasuringDevice.Unit;
        }

        #endregion

        #region ------------ FIELD BINDING ---------------

        private string _serialNo;
        public string SerialNo
        {
            get { return _serialNo; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                CurrentMeasuringDevice.SerialNo = value;
                SetProperty(ref _serialNo, value);
            }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _department, value);
                CurrentMeasuringDevice.Department = value;
            }
        }

        private Location _location = new Location();
        public Location Location
        {
            get { return _location; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _location, value);
                CurrentMeasuringDevice.Location = value;
            }
        }

        private Plant _plant = new Plant();
        public Plant Plant
        {
            get { return _plant; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _plant, value);
                CurrentMeasuringDevice.Plant = value;
            }
        }

        private string _calibrationMethod;
        public string CalibrationMethod
        {
            get { return _calibrationMethod; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _calibrationMethod, value);
                CurrentMeasuringDevice.CalibrationMethod = value;
            }
        }

        private AcceptanceCriteria _acceptanceCriteria = new AcceptanceCriteria();
        public AcceptanceCriteria AcceptanceCriteria
        {
            get { return _acceptanceCriteria; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _acceptanceCriteria, value);
                CurrentMeasuringDevice.AcceptanceCriteria = value;
            }
        }



        private FrequencyOfCalibration _frequencyOfCalibration = new FrequencyOfCalibration();
        public FrequencyOfCalibration FrequencyOfCalibration
        {
            get { return _frequencyOfCalibration; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _frequencyOfCalibration, value);
                CurrentMeasuringDevice._FrequencyOfCalibration = value;
            }
        }

        private DateTime? _nextCalibrationDate = DateTime.UtcNow;
        public DateTime? NextCalibrationDate
        {
            get { return _nextCalibrationDate; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _nextCalibrationDate, value);
                CurrentMeasuringDevice.NextCalibrationDate = value;
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _status, value);
                CurrentMeasuringDevice.Status = value;
            }
        }

        private string _barcode;
        public string Barcode
        {
            get { return _barcode; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _barcode, value);
                CurrentMeasuringDevice.Barcode = value;
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _remarks, value);
                CurrentMeasuringDevice.Remarks = value;
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _description, value);
                CurrentMeasuringDevice.Description = value;
            }
        }

        private string _resolution;
        public string Resolution
        {
            get { return _resolution; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _resolution, value);
                CurrentMeasuringDevice.Resolution = value;
            }
        }

        private string _maker;
        public string Maker
        {
            get { return _maker; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _maker, value);
                CurrentMeasuringDevice.Maker = value;
            }
        }


        private string _deviceRange;
        public string DeviceRange
        {
            get { return _deviceRange; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _deviceRange, value);
                CurrentMeasuringDevice.DeviceRange = value;
            }
        }

        private string _accuracy;
        public string Accuracy
        {
            get { return _accuracy; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _accuracy, value);
                CurrentMeasuringDevice.Accuracy = value;
            }
        }

        private UnitEntity _unit = new UnitEntity();
        public UnitEntity Unit
        {
            get { return _unit; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _unit, value);
                CurrentMeasuringDevice.Unit = value;
            }
        }

        private DateTime? _endOfLife = DateTime.UtcNow;
        public DateTime? EndOfLife
        {
            get { return _endOfLife; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _endOfLife, value);
                CurrentMeasuringDevice.EndOfLife = value;
            }
        }

        private EquipmentSerial _equipmentSerial = new EquipmentSerial();
        public EquipmentSerial EquipmentSerial
        {
            get { return _equipmentSerial; }
            set
            {
                SetProperty(ref _equipmentSerial, value);
                CurrentMeasuringDevice.EquipmentSerial = value;
            }
        }

        private InstrumentSerial _instrumentSerial = new InstrumentSerial();

        public InstrumentSerial InstrumentSerial
        {
            get { return _instrumentSerial; }
            set
            {
                SetProperty(ref _instrumentSerial, value);
                CurrentMeasuringDevice.InstrumentSerial = value;
            }
        }

        private MachineToolSerial _machineToolSerial = new MachineToolSerial();
        public MachineToolSerial MachineToolSerial
        {
            get { return _machineToolSerial; }
            set
            {
                SetProperty(ref _machineToolSerial, value);
                CurrentMeasuringDevice.MachineToolSerial = value;
            }
        }

		/// <summary>
		/// this will always check if the ExecuteMethod can be executed (this will run the validation function which is CanSave())
		/// </summary>
		private void RaiseCanExecuteForSaveChangesCommand()
        {
            if (SaveChangesCommand != null)
                SaveChangesCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Measuring Device Tool Logic

        private bool _isEquipment;
        public bool IsEquipment
        {
            get { return _isEquipment; }
            set
            {
                SetProperty(ref _isEquipment, value);
            }
        }

        private bool _isMachineTool;
        public bool IsMachineTool
        {
            get { return _isMachineTool; }
            set
            {
                SetProperty(ref _isMachineTool, value);
            }
        }

        private bool _isInstrument;
        public bool IsInstrument
        {
            get { return _isInstrument; }
            set
            {
                SetProperty(ref _isInstrument, value);
            }
        }

        private void SelectMachineTool()
        {
            IsInstrument = false;
            IsMachineTool = true;
            IsEquipment = false;

            CurrentMeasuringDevice.EquipmentSerial = null;
            CurrentMeasuringDevice.InstrumentSerial = null;
            CurrentMeasuringDevice.MachineToolSerial = new MachineToolSerial();
            CurrentMeasuringDevice.DeviceType = new DeviceType { Id = 2 };

            EventAggregatorSubscribe();

            regionManager.RequestNavigate("ToolRegion", "MachineToolForMeasuringDevice");
        }

        private void SelectEquipment()
        {
            IsEquipment = true;
            IsMachineTool = false;
            IsInstrument = false;

            CurrentMeasuringDevice.MachineToolSerial = null;
            CurrentMeasuringDevice.InstrumentSerial = null;
            CurrentMeasuringDevice.EquipmentSerial = new EquipmentSerial();
            CurrentMeasuringDevice.DeviceType = new DeviceType { Id = 1 };

            EventAggregatorSubscribe();

            regionManager.RequestNavigate("ToolRegion", "EquipmentFormForMeasuringDevice");
        }

        private void SelectInstrument()
        {
            IsEquipment = false;
            IsMachineTool = false;
            IsInstrument = true;

            CurrentMeasuringDevice.EquipmentSerial = null;
            CurrentMeasuringDevice.MachineToolSerial = null;
            CurrentMeasuringDevice.InstrumentSerial = new InstrumentSerial();
            CurrentMeasuringDevice.DeviceType = new DeviceType { Id = 3 };

            EventAggregatorSubscribe();

            regionManager.RequestNavigate("ToolRegion", "InstrumentForMeasuringDevice");
        }

        #endregion

        #region UI logic

        private bool _isToolFormValid;
        public bool IsToolFormValid
        {
            get { return _isToolFormValid; }
            set { SetProperty(ref _isToolFormValid, value); }
        }

        private bool _isDataSaving = true;
        public bool IsDataSaving
        {
            get { return _isDataSaving; }
            set { SetProperty(ref _isDataSaving, value); }
        }

        private bool _isForIssuedEmployee;
        public bool IsForIssuedEmployee
        {
            get { return _isForIssuedEmployee; }
            set { SetProperty(ref _isForIssuedEmployee, value); }
        }

        private bool _isForCalibratedEmployee;
        public bool IsForCalibratedByEmployee
        {
            get { return _isForCalibratedEmployee; }
            set { SetProperty(ref _isForCalibratedEmployee, value); }
        }

        private bool _isSerialNoNull;
        public bool IsSerialNoNull
        {
            get { return _isSerialNoNull; }
            set
            {
                RaiseCanExecuteForSaveChangesCommand();
                SetProperty(ref _isSerialNoNull, value);
            }
        }

        private bool _isCalibratedByNull;
        public bool IsCalibratedByNull
        {
            get { return _isCalibratedByNull; }
            set
            { SetProperty(ref _isCalibratedByNull, value); }
        }

        private bool _isResultOfCalibrationNull;
        public bool IsResultOfCalibrationNull
        {
            get { return _isResultOfCalibrationNull; }
            set { SetProperty(ref _isResultOfCalibrationNull, value); }
        }

        private bool _isCalibrationMethodNull;
        public bool IsCalibrationMethodNull
        {
            get { return _isCalibrationMethodNull; }
            set { SetProperty(ref _isCalibrationMethodNull, value); }
        }

        private bool _isAcceptanceCriteriaNull;
        public bool IsAcceptanceCriteriaNull
        {
            get { return _isAcceptanceCriteriaNull; }
            set { SetProperty(ref _isAcceptanceCriteriaNull, value); }
        }

        private bool _isFrequencyOfCalibrationNull;
        public bool IsFrequencyOfCalibrationNull
        {
            get { return _isFrequencyOfCalibrationNull; }
            set { SetProperty(ref _isFrequencyOfCalibrationNull, value); }
        }

        private bool _isLastCalibrationDateNull;
        public bool IsLastCalibrationDateNull
        {
            get { return _isLastCalibrationDateNull; }
            set { SetProperty(ref _isLastCalibrationDateNull, value); }
        }

        private bool _isNextCalibrationDateNull;
        public bool IsNextCalibrationDateNull
        {
            get { return _isNextCalibrationDateNull; }
            set { SetProperty(ref _isNextCalibrationDateNull, value); }
        }

        #endregion

        #region Observable collection from database

        private ObservableCollection<Maker> _makers;
        public ObservableCollection<Maker> Makers
        {
            get { return _makers; }
            set { SetProperty(ref _makers, value); }
        }

        private ObservableCollection<Resolution> _resolutions;
        public ObservableCollection<Resolution> Resolutions
        {
            get { return _resolutions; }
            set { SetProperty(ref _resolutions, value); }
        }

        private ObservableCollection<FrequencyOfCalibration> _frequencyOfCalibrations;
        public ObservableCollection<FrequencyOfCalibration> FrequencyOfCalibrations
        {
            get { return _frequencyOfCalibrations; }
            set { SetProperty(ref _frequencyOfCalibrations, value); }
        }

        private ObservableCollection<AcceptanceCriteria> _acceptanceCriterias;
        public ObservableCollection<AcceptanceCriteria> AcceptanceCriterias
        {
            get { return _acceptanceCriterias; }
            set { SetProperty(ref _acceptanceCriterias, value); }
        }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private ObservableCollection<Department> departments;

        public ObservableCollection<Department> Departments
        {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }

        private ObservableCollection<Location> _locations;

        public ObservableCollection<Location> Locations
        {
            get { return _locations; }
            set { SetProperty(ref _locations, value); }
        }

        private ObservableCollection<UnitEntity> _units;

        public ObservableCollection<UnitEntity> Units
        {
            get { return _units; }
            set { SetProperty(ref _units, value); }
        }

        private ObservableCollection<Plant> _plants;

        public ObservableCollection<Plant> Plants
        {
            get { return _plants; }
            set { SetProperty(ref _plants, value); }
        }

        private ObservableCollection<EquipmentType> _equipmentTypes;
        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get { return _equipmentTypes; }
            set { SetProperty(ref _equipmentTypes, value); }
        }

        private ObservableCollection<InstrumentSerial> _instrumentSerials;
        public ObservableCollection<InstrumentSerial> InstrumentSerials
        {
            get { return _instrumentSerials; }
            set { SetProperty(ref _instrumentSerials, value); }
        }

        #endregion

        #region Custom Observable Collection

        private ObservableCollection<string> _calibrationMethods;

        public ObservableCollection<string> CalibrationMethods
        {
            get { return _calibrationMethods; }
            set { SetProperty(ref _calibrationMethods, value); }
        }

        private ObservableCollection<string> _calibrationResults;

        public ObservableCollection<string> CalibrationResults
        {
            get { return _calibrationResults; }
            set { SetProperty(ref _calibrationResults, value); }
        }



        private ObservableCollection<string> _descriptions;

        public ObservableCollection<string> Descriptions
        {
            get { return _descriptions; }
            set { SetProperty(ref _descriptions, value); }
        }

        private ObservableCollection<string> _type;

        public ObservableCollection<string> Types
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private ObservableCollection<string> _statusData;

        public ObservableCollection<string> StatusData
        {
            get { return _statusData; }
            set { SetProperty(ref _statusData, value); }
        }

        private ObservableCollection<string> _accuracies;
        public ObservableCollection<string> Accuracies
        {
            get { return _accuracies; }
            set { SetProperty(ref _accuracies, value); }
        }


        private ObservableCollection<string> _range;

        public ObservableCollection<string> Range
        {
            get { return _range; }
            set { SetProperty(ref _range, value); }
        }

        #endregion

        public MeasuringDeviceFormViewModel(IRegionManager regionManager, IDialogService dialogService, IEventAggregator ea)
        {
            CalibrationMethods = new ObservableCollection<string>(new List<string> { "Calibration Method 1", "Calibration Method 2", "Calibration Method 3", "Calibration Method 4", "Calibration Method 5" });
            CalibrationResults = new ObservableCollection<string>(new List<string> { "Calibration Result 1", "Calibration Result 2", "Calibration Result 3", "Calibration Result 4", "Calibration Result 5" });
            Descriptions = new ObservableCollection<string>(new List<string> { "Description 1", "Description 2", "Description 3", "Description 4", "Description 5" });
            Types = new ObservableCollection<string>(new List<string> { "Type 1", "Type 2", "Type 3", "Type 4", "Type 5" });
            StatusData = new ObservableCollection<string>(new List<string> { "Status 1", "Status 2", "Status 3", "Status 4", "Status 5" });

            Accuracies = new ObservableCollection<string>();
            for (int i = 1; i <= 100; i += 4)
            {
                Accuracies.Add(i.ToString());
            }

            Range = new ObservableCollection<string>();
            for (int i = 1; i <= 100; i += 9)
            {
                Range.Add(i.ToString());
            }

            this.regionManager = regionManager;
            _dialogService = dialogService;
            this.ea = ea;
            UpdateMeasuringDeviceCommand = new DelegateCommand(ExecuteUpdate);
            DeleteMeasuringDeviceCommand = new DelegateCommand(ExecuteDelete);
            CreateNewMeasuringDevice = new DelegateCommand(ExecuteCreateNew);
            ToUpdateMeasringDeviceCommand = new DelegateCommand(ExecuteToUpdate);
            IssuedToEmployeeLookupCommand = new DelegateCommand(ExecuteOpenLookupForIssuedToEmployee);
            CalibratedByEmployeeLookupCommand = new DelegateCommand(ExecuteOpenLookupForCalibratedByEmployee);
            DepartmentConfigLookupCommand = new DelegateCommand(ExecuteOpenDepartmentConfigLookup);
            PlantConfigLookupCommand = new DelegateCommand(ExecuteOpenPlantConfig);
            EquipmentSelectionCommand = new DelegateCommand(SelectEquipment);
            InstrumentToolSelectionCommand = new DelegateCommand(SelectInstrument);
            MachineToolSelectionCommand = new DelegateCommand(SelectMachineTool);
            InstrumentSelectionCommand = new DelegateCommand(SelectInstrument);
            NavigateBackToList = new DelegateCommand<string>(Navigate);
            OpenUnitConfigLookupCommand = new DelegateCommand(OpenUnitConfigLookup);

            measuringDeviceDA = new MeasuringDeviceDA();
            employeeDA = new EmployeeDA();
            departmentDA = new DepartmentDA();
            locationDA = new LocationDA();
            unitDA = new UnitDA();
            plantDA = new PlantDA();
            equipmentTypeDA = new EquipmentTypeDA();
            instrumentSerialDA = new InstrumentSerialDA();
            acceptanceCriteriaDA = new AcceptanceCriteriaDA();
            frequencyOfCalibrationDA = new FrequencyOfCalibrationDA();
            resolutionDA = new ResolutionDA();
            makerDA = new MakerDA();

            Employees = new ObservableCollection<Employee>(employeeDA.GetAllEmployees());
            Departments = new ObservableCollection<Department>(departmentDA.GetAllDepartments());
            Locations = new ObservableCollection<Location>(locationDA.GetAllLocations());
            Units = new ObservableCollection<UnitEntity>(unitDA.GetAllUnit());
            Plants = new ObservableCollection<Plant>(plantDA.GetAllPlant());
            EquipmentTypes = new ObservableCollection<EquipmentType>(equipmentTypeDA.GetAllEquipmentType());
            InstrumentSerials = new ObservableCollection<InstrumentSerial>(instrumentSerialDA.GetAllInstrumentSerial());
            AcceptanceCriterias = new ObservableCollection<AcceptanceCriteria>(acceptanceCriteriaDA.GetAllAcceptanceCriteria());
            FrequencyOfCalibrations = new ObservableCollection<FrequencyOfCalibration>(frequencyOfCalibrationDA.GetAllFrequencyOfCalibration());
            Resolutions = new ObservableCollection<Resolution>(resolutionDA.GetAllResolutions());
            Makers = new ObservableCollection<Maker>(makerDA.GetAllMaker());

            EventAggregatorSubscribe();

            //Set the default device type as equipment
            IsEquipment = true;
            CurrentMeasuringDevice.DeviceType = new DeviceType { Id = 2 };
        }


        private void SetItemDetailsFromItemLookup(Item obj)
        {

        }

        private void SetPlantFromLookup(Plant obj)
        {
            Plant = obj;
        }

        private void SetUnitFromLookup(UnitEntity obj)
        {
            Unit = obj;
        }

        private void SetInstrumentDetails(Instrument instrument)
        {
            //CurrentMeasuringDevice.Instrument = instrument;
        }


        private void SetMachineToolDetails(MachineTool tool)
        {
            //CurrentMeasuringDevice.MachineTool = tool;
        }

        private void SetEquipmentDetails(Equipment equipment)
        {
            //CurrentMeasuringDevice.Equipment = equipment;
        }

        private void OpenUnitConfigLookup()
        {
            _dialogService.ShowDialog("UnitConfig");
        }

        private void SetDepartmentFromLookup(Department obj)
        {
            Department = obj;
        }

        private void SetEmployeeDataFromLookup(Employee empObj)
        {

        }

        public MeasuringDeviceFormViewModel()
        {

        }

        #region Command's Methods
        public void ExecuteCreateNew()
        {
            IsDataSaving = true;

            CurrentMeasuringDevice = new MeasuringDevice()
            {
                //IssuedToEmployee = new Employee(),
                //CalibratedByEmployee = new Employee(),
                Location = new Location(),
                Plant = new Plant(),
                //Equipment = new Equipment
                //{
                //    EquipmentType = new EquipmentType(),
                //    Item = new Item
                //    {
                //        Description = new Description()
                //    }
                //},
                //MachineTool = new MachineTool
                //{
                //    MachineToolType = new MachineToolType(),
                //    Item = new Item
                //    {
                //        Description = new Description()
                //    }
                //}
            };

            //IssuedToEmployee = CurrentMeasuringDevice.IssuedToEmployee;
            Department = CurrentMeasuringDevice.Department;
            //CalibratedByEmployee = CurrentMeasuringDevice.CalibratedByEmployee;

            //ea.GetEvent<EquipmentToMeasuringDevice>().Publish(CurrentMeasuringDevice.Equipment);
            //ea.GetEvent<MachineToolToMeasuringDevice>().Publish(CurrentMeasuringDevice.MachineTool);

            SetProperty(ref _currentMeasuringDevice, CurrentMeasuringDevice);
        }

        public void ExecuteDelete()
        {
            if (CurrentMeasuringDevice.Id != 0 && CurrentMeasuringDevice != null)
            {
                var delete = MessageBox.Show("Are you sure you want to delete this measuring device?", "Delete", MessageBoxButton.YesNo);
                if (delete == MessageBoxResult.Yes)
                {
                    measuringDeviceDA.DeleteMeasuringDevice(CurrentMeasuringDevice);
                    regionManager.RequestNavigate("MainRegion", "List");
                }
            }
            else
            {
                MessageBox.Show("No measuring device selected", "Invalid", MessageBoxButton.OK);
            }
        }

        public void ExecuteUpdate()
        {
            measuringDeviceDA.UpdateMeasuringDevice(CurrentMeasuringDevice);

        }

        public void ExecuteSave()
        {
            measuringDeviceDA.CreateMeasuringDevice(CurrentMeasuringDevice);
        }

        private bool RequiredValidation()
        {
            var output = true;
            if (string.IsNullOrEmpty(SerialNo) || Department.Id == null ||
                Location == null || Plant == null ||
                string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(CalibrationMethod) || AcceptanceCriteria == null ||
                FrequencyOfCalibration == null || NextCalibrationDate.Value == null || string.IsNullOrEmpty(Status) ||
                string.IsNullOrEmpty(Barcode) || string.IsNullOrEmpty(Remarks) || EndOfLife.Value == null || string.IsNullOrEmpty(Resolution) ||
                string.IsNullOrEmpty(Maker) || string.IsNullOrEmpty(Accuracy) || string.IsNullOrEmpty(DeviceRange) || Unit.Id == null || IsToolFormValid == false)
			{
				output = false;
			}

            return output;
		}

        private bool CanSave()
        {
            return RequiredValidation();
        }

		private bool CanUpdate()
		{
            return RequiredValidation();
		}

		private void ExecuteOpenDepartmentConfigLookup()
        {
            _dialogService.ShowDialog("DeptConfig");
        }



        private void ExecuteOpenLookupForIssuedToEmployee()
        {
            IsForIssuedEmployee = true;
            IsForCalibratedByEmployee = false;

            _dialogService.ShowDialog("EmpConfig");
        }

        private void ExecuteOpenLookupForCalibratedByEmployee()
        {
            IsForIssuedEmployee = false;
            IsForCalibratedByEmployee = true;

            _dialogService.ShowDialog("EmpConfig");
        }

        private void ExecuteToUpdate()
        {
            IsDataSaving = true;
        }

        private void ExecuteOpenPlantConfig()
        {
            _dialogService.ShowDialog("PlantConfig");
        }

        #endregion


        private void Navigate(string uri)
        {
            regionManager.RequestNavigate("MainRegion", uri);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Set default command as Save data first
            SaveChangesCommand = new DelegateCommand(ExecuteSave, CanSave);

            // check first what view will be injected to the region
            if (navigationContext.Parameters.Count != 0)
            {
                // set the default command now as Update data
                IsDataSaving = false;
                var measuringDeviceFromList = navigationContext.Parameters["measuringDeviceObj"] as MeasuringDevice;
                SetFieldBindingValue(measuringDeviceFromList);

				var navParameter = new NavigationParameters();

                // will load the instrument serial form that has data
                if (measuringDeviceFromList.InstrumentSerial != null)
                {
                    navParameter.Add("instrumentObj", measuringDeviceFromList.InstrumentSerial);
                    IsInstrument = true;

                    regionManager.RequestNavigate("ToolRegion", "InstrumentForMeasuringDevice", navParameter);
                }
                else if (measuringDeviceFromList.EquipmentSerial != null)
                {
                    navParameter.Add("equipmentObj", measuringDeviceFromList.EquipmentSerial);
                    IsEquipment = true;

                    regionManager.RequestNavigate("ToolRegion", "EquipmentFormForMeasuringDevice", navParameter);
                }
                else if (measuringDeviceFromList.MachineToolSerial != null)
                {
                    navParameter.Add("machineToolObj", measuringDeviceFromList.MachineToolSerial);
                    IsMachineTool = true;

                    regionManager.RequestNavigate("ToolRegion", "MachineToolForMeasuringDevice", navParameter);
                }

				SaveChangesCommand = new DelegateCommand(ExecuteUpdate, CanUpdate);
			}
			else
            {
                // Set default region as Equipment
                regionManager.RequestNavigate("ToolRegion", "EquipmentFormForMeasuringDevice");
            }
        }

		

		public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var selectedMeasuringDeviceObj = navigationContext.Parameters["MeasuringDeviceObj"] as MeasuringDevice;
            return selectedMeasuringDeviceObj == null ? true : false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}

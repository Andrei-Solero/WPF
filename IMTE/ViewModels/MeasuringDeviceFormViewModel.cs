using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Unity;

namespace IMTE.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class MeasuringDeviceFormViewModel : BindableBase, INavigationAware
    {
        private readonly MeasuringDeviceDA measuringDeviceDA;
        private readonly EmployeeDA employeeDA;
        private readonly DepartmentDA departmentDA;
        private readonly LocationDA locationDA;
        private readonly UnitDA unitDA;
        private readonly PlantDA plantDA;
        private readonly EquipmentTypeDA equipmentTypeDA;
        private readonly IRegionManager regionManager;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator ea;

        #region -------------- DELEGATE COMMANDS --------------

        private DelegateCommand _saveChangesCommand;
        public DelegateCommand SaveChangesCommand
        {
            get { return _saveChangesCommand; }
            set { SetProperty(ref _saveChangesCommand, value); }
        }

        public DelegateCommand<MeasuringDevice> SaveMeasuringDeviceCommand { get; private set; }
        public DelegateCommand ToUpdateMeasringDeviceCommand { get; }
        public DelegateCommand UpdateMeasuringDeviceCommand { get; }
        public DelegateCommand DeleteMeasuringDeviceCommand { get; }
        public DelegateCommand CreateNewMeasuringDevice { get; }
        public DelegateCommand<string> NavigateBackToList { get; }
        public DelegateCommand IssuedToEmployeeLookupCommand { get; }
        public DelegateCommand CalibratedByEmployeeLookupCommand { get; set; }
        public DelegateCommand DepartmentConfigLookupCommand { get; set; }
        public DelegateCommand PlantConfigLookupCommand { get; set; }
        public DelegateCommand EquipmentSelectionCommand { get; }
        public DelegateCommand MachineToolSelectionCommand { get; }
        public DelegateCommand OpenUnitConfigLookupCommand { get; }

        #endregion

        private MeasuringDevice _currentMeasuringDevice = new MeasuringDevice();
        public MeasuringDevice CurrentMeasuringDevice
        {
            get { return _currentMeasuringDevice; }
            set
            {
                SetProperty(ref _currentMeasuringDevice, value);

                IssuedToEmployee = value.IssuedToEmployee;
                Department = value.Department;
                Location = value.Location;
                Plant = value.Plant;
                CalibratedByEmployee = value.CalibratedByEmployee;
                Unit = value.Unit;
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

            // Will receive the data from equipment form in this form
            ea.GetEvent<EquipmentToMeasuringDevice>().Subscribe(SetEquipmentDetailsToCurrentMD);

            // Will receive the data from machine tool in this form
            ea.GetEvent<MachineToolToMeasuringDevice>().Subscribe(GetMachineToolDetails);

            //ea.GetEvent<EquipmentMachineToolWithMeasuringDeviceDataToMDForm>().Subscribe(SetMeasuringDeviceWithEquipmentToForm);
        }


        #region Helper

        private void SetFieldBindingValue(MeasuringDevice measuringDeviceObj)
        {
            CurrentMeasuringDevice = measuringDeviceObj;

            SerialNo = measuringDeviceObj.SerialNo;
            IssuedToEmployee = measuringDeviceObj.IssuedToEmployee;
            CalibratedByEmployee = measuringDeviceObj.CalibratedByEmployee;
            Department = measuringDeviceObj.Department;
            Location = measuringDeviceObj.Location;
            Plant = measuringDeviceObj.Plant;
            ResultOfCalibration = measuringDeviceObj.ResultOfCalibration;
            CalibrationMethod = measuringDeviceObj.CalibrationMethod;
            AcceptanceCriteria = measuringDeviceObj.AcceptanceCriteria;
            FrequencyOfCalibration = measuringDeviceObj.FrequencyOfCalibration;
            LastCalibrationDate = measuringDeviceObj.LastCalibrationDate;
            NextCalibrationDate = measuringDeviceObj.NextCalibrationDate;
            CalibrationRemarks = measuringDeviceObj.CalibrationRemarks;
            ThreadGaugeRingGaugeUsageNo = measuringDeviceObj.ThreadGaugeRingGaugeUsageNo;
            Status = measuringDeviceObj.Status;
            Barcode = measuringDeviceObj.Barcode;
            Remarks = measuringDeviceObj.Remarks;
            Date = measuringDeviceObj.Date;
            Maker = measuringDeviceObj.Maker;
            Resolution = measuringDeviceObj.Resolution;
            DeviceRange = measuringDeviceObj.DeviceRange;
            Accuracy = measuringDeviceObj.Accuracy;
            Unit = measuringDeviceObj.Unit;

            Equipment = measuringDeviceObj.Equipment != null ? measuringDeviceObj.Equipment : null;
            MachineTool = measuringDeviceObj.MachineTool != null ? measuringDeviceObj.MachineTool : null;
        }

        #endregion

        #region ------------ FIELD BINDING ---------------

        private string _serialNo;
        public string SerialNo
        {
            get { return _serialNo; }
            set
            {
                SetProperty(ref _serialNo, value);
                CurrentMeasuringDevice.SerialNo = value;
            }
        }

        private Employee _issuedToEmployee = new Employee();
        public Employee IssuedToEmployee
        {
            get { return _issuedToEmployee; }
            set
            {
                SetProperty(ref _issuedToEmployee, value);
                CurrentMeasuringDevice.IssuedToEmployee = value;
            }
        }

        private Employee _calibratedByEmployee = new Employee();
        public Employee CalibratedByEmployee
        {
            get { return _calibratedByEmployee; }
            set
            {
                SetProperty(ref _calibratedByEmployee, value);
                CurrentMeasuringDevice.CalibratedByEmployee = value;
            }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set
            {
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
                SetProperty(ref _plant, value);
                CurrentMeasuringDevice.Plant = value;
            }
        }

        private string _resultOfCalibration;
        public string ResultOfCalibration
        {
            get { return _resultOfCalibration; }
            set 
            {
                SetProperty(ref _resultOfCalibration, value);
                CurrentMeasuringDevice.ResultOfCalibration = value;
            }
        }

        private string _calibrationMethod;
        public string CalibrationMethod
        {
            get { return _calibrationMethod; }
            set
            {
                SetProperty(ref _calibrationMethod, value);
                CurrentMeasuringDevice.CalibrationMethod = value;
            }
        }

        private string _acceptanceCriteria;
        public string AcceptanceCriteria
        {
            get { return _acceptanceCriteria; }
            set
            {
                SetProperty(ref _acceptanceCriteria, value);
                CurrentMeasuringDevice.AcceptanceCriteria = value;
            }
        }
        
        private string _frequencyOfCalibration;
        public string FrequencyOfCalibration
        {
            get { return _frequencyOfCalibration; }
            set
            {
                SetProperty(ref _frequencyOfCalibration, value);
                CurrentMeasuringDevice.FrequencyOfCalibration = value;
            }
        }

        private DateTime? _lastCalibrationDate = DateTime.UtcNow;
        public DateTime? LastCalibrationDate
        {
            get { return _lastCalibrationDate; }
            set
            {
                SetProperty(ref _lastCalibrationDate, value);
                CurrentMeasuringDevice.LastCalibrationDate = value;
            }
        }

        private DateTime? _nextCalibrationDate = DateTime.UtcNow;
        public DateTime? NextCalibrationDate
        {
            get { return _nextCalibrationDate; }
            set
            {
                SetProperty(ref _nextCalibrationDate, value);
                CurrentMeasuringDevice.NextCalibrationDate = value;
            }
        }

        private string _calibrationRemarks;
        public string CalibrationRemarks
        {
            get { return _calibrationRemarks; }
            set
            {
                SetProperty(ref _calibrationRemarks, value);
                CurrentMeasuringDevice.CalibrationRemarks = value;
            }
        }

        private decimal _threadGaugeRingGaugeUsageNo;
        public decimal ThreadGaugeRingGaugeUsageNo
        {
            get { return _threadGaugeRingGaugeUsageNo; }
            set
            {
                SetProperty(ref _threadGaugeRingGaugeUsageNo, value);
                CurrentMeasuringDevice.ThreadGaugeRingGaugeUsageNo = value;
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
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
                SetProperty(ref _remarks, value);
                CurrentMeasuringDevice.Remarks = value;
            }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                SetProperty(ref _date, value);
                CurrentMeasuringDevice.Date = value;
            }
        }

        private string _maker;
        public string Maker
        {
            get { return _maker; }
            set
            {
                SetProperty(ref _maker, value);
                CurrentMeasuringDevice.Maker = value;
            }
        }

        private string _resolution;
        public string Resolution
        {
            get { return _resolution; }
            set
            {
                SetProperty(ref _resolution, value);
                CurrentMeasuringDevice.Resolution = value;
            }
        }

        private string _deviceRange;
        public string DeviceRange
        {
            get { return _deviceRange; }
            set
            {
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
                SetProperty(ref _accuracy, value);
                CurrentMeasuringDevice.Accuracy = value;
            }
        }

        private UnitEntity _unit;
        public UnitEntity Unit
        {
            get { return _unit; }
            set
            {
                SetProperty(ref _unit, value);
                CurrentMeasuringDevice.Unit = value;
            }
        }


        private Equipment _equipment = new Equipment();
        public Equipment Equipment
        {
            get { return _equipment; }
            set
            {
                SetProperty(ref _equipment, value);
                CurrentMeasuringDevice.Equipment = value;
            }
        }

        private MachineTool _machineTool = new MachineTool();
        public MachineTool MachineTool
        {
            get { return _machineTool; }
            set
            {
                SetProperty(ref _machineTool, value);
                CurrentMeasuringDevice.MachineTool = value;
            }
        }

        #endregion

        #region UI logic

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

        private bool _isEquipment = true;
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


        #endregion

        #region Observable collection from database

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

        private ObservableCollection<string> _acceptanceCriterias;

        public ObservableCollection<string> AcceptanceCriterias
        {
            get { return _acceptanceCriterias; }
            set { SetProperty(ref _acceptanceCriterias, value); }
        }

        private ObservableCollection<string> _frequencyOfCalibrations;

        public ObservableCollection<string> FrequencyOfCalibrations
        {
            get { return _frequencyOfCalibrations; }
            set { SetProperty(ref _frequencyOfCalibrations, value); }
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

        private ObservableCollection<string> _makers;
        public ObservableCollection<string> Makers
        {
            get { return _makers; }
            set { SetProperty(ref _makers, value); }
        }

        private ObservableCollection<string> _accuracies;
        public ObservableCollection<string> Accuracies
        {
            get { return _accuracies; }
            set { SetProperty(ref _accuracies, value); }
        }

        private ObservableCollection<string> _resolutions;
        public ObservableCollection<string> Resolutions
        {
            get { return _resolutions; }
            set { SetProperty(ref _resolutions, value); }
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
            AcceptanceCriterias = new ObservableCollection<string>(new List<string> { "Acceptance 1", "Acceptance 2", "Acceptance 3", "Acceptance 4", "Acceptance 5" });
            FrequencyOfCalibrations = new ObservableCollection<string>(new List<string> { "Frequency of Calibration 1", "Frequency of Calibration 2", "Frequency of Calibration 3", "Frequency of Calibration 4", "Frequency of Calibration 5" });
            Descriptions = new ObservableCollection<string>(new List<string> { "Description 1", "Description 2", "Description 3", "Description 4", "Description 5" });
            Types = new ObservableCollection<string>(new List<string> { "Type 1", "Type 2", "Type 3", "Type 4", "Type 5" });
            StatusData = new ObservableCollection<string>(new List<string> { "Status 1", "Status 2", "Status 3", "Status 4", "Status 5" });
            Makers = new ObservableCollection<string>(new List<string> { "Maker 1", "Maker 2", "Maker 3", "Maker 4", "Maker 5" });
            Resolutions = new ObservableCollection<string>(new List<string> { "Resolution 1", "Resolution 2", "Resolution 3", "Resolution 4", "Resolution 5" });

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
            MachineToolSelectionCommand = new DelegateCommand(SelectMachineTool);
            NavigateBackToList = new DelegateCommand<string>(Navigate);
            OpenUnitConfigLookupCommand = new DelegateCommand(OpenUnitConfigLookup);

            measuringDeviceDA = new MeasuringDeviceDA();
            employeeDA = new EmployeeDA();
            departmentDA = new DepartmentDA();
            locationDA = new LocationDA();
            unitDA = new UnitDA();
            plantDA = new PlantDA();
            equipmentTypeDA = new EquipmentTypeDA();

            Employees = new ObservableCollection<Employee>(employeeDA.GetAllEmployees());
            Departments = new ObservableCollection<Department>(departmentDA.GetAllDepartments());
            Locations = new ObservableCollection<Location>(locationDA.GetAllLocations());
            Units = new ObservableCollection<UnitEntity>(unitDA.GetAllUnit());
            Plants = new ObservableCollection<Plant>(plantDA.GetAllPlant());
            EquipmentTypes = new ObservableCollection<EquipmentType>(equipmentTypeDA.GetAllEquipmentType());

            EventAggregatorSubscribe();
        }

        private void SetItemDetailsFromItemLookup(Item obj)
        {
            
        }

        private void SetUnitFromLookup(UnitEntity obj)
        {
            Unit = obj;
        }

        private void GetMachineToolDetails(MachineTool tool)
        {
            CurrentMeasuringDevice.MachineTool = tool;
        }

        private void SetEquipmentDetailsToCurrentMD(Equipment equipment)
        {
            CurrentMeasuringDevice.Equipment = equipment;
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
            if (IsForIssuedEmployee)
            {
                IssuedToEmployee = empObj;
                SetProperty(ref _issuedToEmployee, empObj);
            }
            else if (IsForCalibratedByEmployee)
            {
                CalibratedByEmployee = empObj;
                SetProperty(ref _calibratedByEmployee, empObj);
            }
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
                IssuedToEmployee = new Employee(),
                CalibratedByEmployee = new Employee(),
                Location = new Location(),
                Plant = new Plant(),
                Equipment = new Equipment
                {
                    EquipmentType = new EquipmentType(),
                    Item = new Item
                    {
                        Description = new Description()
                    }
                },
                MachineTool = new MachineTool
                {
                    MachineToolType = new MachineToolType(),
                    Item = new Item
                    {
                        Description = new Description()
                    }
                }
            };

            IssuedToEmployee = CurrentMeasuringDevice.IssuedToEmployee;
            Department = CurrentMeasuringDevice.Department;
            CalibratedByEmployee = CurrentMeasuringDevice.CalibratedByEmployee;

            ea.GetEvent<EquipmentToMeasuringDevice>().Publish(CurrentMeasuringDevice.Equipment);
            ea.GetEvent<MachineToolToMeasuringDevice>().Publish(CurrentMeasuringDevice.MachineTool);

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
            EventAggregatorSubscribe();
            measuringDeviceDA.UpdateMeasuringDevice(CurrentMeasuringDevice);
            
        }

        public void ExecuteSave()
        {
            EventAggregatorSubscribe();
            measuringDeviceDA.CreateMeasuringDevice(CurrentMeasuringDevice);
        }

        private void ExecuteOpenDepartmentConfigLookup()
        {
            _dialogService.ShowDialog("DeptConfig");
        }

        private void SelectMachineTool()
        {
            IsMachineTool = true;
            SetProperty(ref _isMachineTool, true);

            IsEquipment = false;
            SetProperty(ref _isEquipment, false);
            regionManager.RequestNavigate("EquipmentMachineToolRegion", "MachineToolForMeasuringDevice");

            CurrentMeasuringDevice.Equipment = null;
            SetProperty(ref _equipment, null);
        }

        private void SelectEquipment()
        {
            IsEquipment = true;
            SetProperty(ref _isEquipment, true);

            IsMachineTool = false;
            SetProperty(ref _isMachineTool, false);
            regionManager.RequestNavigate("EquipmentMachineToolRegion", "EquipmentFormForMeasuringDevice");

            CurrentMeasuringDevice.MachineTool = null;
            SetProperty(ref _machineTool, null);
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
            SetProperty(ref _isForIssuedEmployee, false);
            SetProperty(ref _isForCalibratedEmployee, true);

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
            SaveChangesCommand = new DelegateCommand(ExecuteSave);

            // check first what view will be injected to the region
            if (navigationContext.Parameters.Count != 0)
            {
                EventAggregatorSubscribe();

                // set the default command now as Update data
                IsDataSaving = false;
                SaveChangesCommand = new DelegateCommand(ExecuteUpdate);
                var measuringDeviceFromList = navigationContext.Parameters["measuringDeviceObj"] as MeasuringDevice;
                SetFieldBindingValue(measuringDeviceFromList);

                if (CurrentMeasuringDevice.Equipment != null)
                {
                    var navParameter = new NavigationParameters();
                    navParameter.Add("equipmentObj", CurrentMeasuringDevice.Equipment);

                    IsMachineTool = false;
                    IsEquipment = true;
                    MachineTool = null;

                    regionManager.RequestNavigate("EquipmentMachineToolRegion", "EquipmentFormForMeasuringDevice", navParameter);
                }
                else if (CurrentMeasuringDevice.MachineTool != null)
                {
                    var navParameter = new NavigationParameters();
                    navParameter.Add("equipmentObj", CurrentMeasuringDevice.MachineTool);

                    IsMachineTool = true;
                    IsEquipment = false;
                    Equipment = null;

                    regionManager.RequestNavigate("EquipmentMachineToolRegion", "MachineToolForMeasuringDevice", navParameter);
                }
                else
                {
                    // no view will be injected to the region
                    IsMachineTool = false;
                    IsEquipment = false;
                }
            }
            else
            {
                // Set default region as Equipment
                regionManager.RequestNavigate("EquipmentMachineToolRegion", "EquipmentFormForMeasuringDevice");
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

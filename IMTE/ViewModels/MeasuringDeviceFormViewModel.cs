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
		private DelegateCommand<MeasuringDevice> _saveChangesCommand;

        public DelegateCommand<MeasuringDevice> SaveChangesCommand
        {
            get { return _saveChangesCommand; }
            set { SetProperty(ref _saveChangesCommand, value); }
        }

        public DelegateCommand<MeasuringDevice> SaveMeasuringDeviceCommand { get; private set; }
        public DelegateCommand ToUpdateMeasringDeviceCommand { get; }
        public DelegateCommand<MeasuringDevice> UpdateMeasuringDeviceCommand { get; }
        public DelegateCommand DeleteMeasuringDeviceCommand { get; }
        public DelegateCommand CreateNewMeasuringDevice { get; }
        public DelegateCommand<string> NavigateBackToList { get; }
        public DelegateCommand IssuedToEmployeeLookupCommand { get; }
        public DelegateCommand CalibratedByEmployeeLookupCommand { get; set; }
        public DelegateCommand DepartmentConfigLookupCommand { get; set; }
        public DelegateCommand PlantConfigLookupCommand { get; set; }
		public DelegateCommand EquipmentSelectionCommand { get; }
        public DelegateCommand MachineToolSelectionCommand { get; }
        

        private MeasuringDevice _currentMeasuringDevice = new MeasuringDevice();
        public MeasuringDevice CurrentMeasuringDevice
        {
            get { return _currentMeasuringDevice; }
            set { SetProperty(ref _currentMeasuringDevice, value); }
        }

        #region UI logic

        private bool _isDataEdit;
        public bool IsDataEdit
        {
            get { return _isDataEdit; }
            set { SetProperty(ref _isDataEdit, value); }
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

        #region Seperate complex objects for binding

        private MachineTool _machineTool = new MachineTool();
        public MachineTool MachineTool
        {
            get { return _machineTool; }
            set { SetProperty(ref _machineTool, value); }
        }


        private Equipment _equipment = new Equipment();
        public Equipment Equipment
        {
            get { return _equipment; }
            set { SetProperty(ref _equipment, value); }
        }

        private EquipmentType _equipmentType;
        public EquipmentType EquipmentType
        {
            get { return _equipmentType; }
            set { SetProperty(ref _equipmentType, value); }
        }

        private Item _item = new Item();

        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private UnitEntity _unitOfMeasurement = new UnitEntity();

        public UnitEntity UnitOfMeasurement
        {
            get { return _unitOfMeasurement; }
            set { SetProperty(ref _unitOfMeasurement, value); }
        }

        private Description _description = new Description();
        public Description Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private Employee _issuedToEmployee = new Employee();
        public Employee IssuedToEmployee
        {
            get { return _issuedToEmployee; }
            set { SetProperty(ref _issuedToEmployee, value); }
        }

        private Employee _calibratedByEmployee = new Employee();
        public Employee CalibratedByEmployee
        {
            get { return _calibratedByEmployee; }
            set { SetProperty(ref _calibratedByEmployee, value); }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
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

        private ObservableCollection<UnitEntity> _unit;

        public ObservableCollection<UnitEntity> Units
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
        }

        private ObservableCollection<Plant> _plant;

        public ObservableCollection<Plant> Plants
        {
            get { return _plant; }
            set { SetProperty(ref _plant, value); }
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

        private ObservableCollection<string> _acceptanceCriteria;

        public ObservableCollection<string> AcceptanceCriterias
        {
            get { return _acceptanceCriteria; }
            set { SetProperty(ref _acceptanceCriteria, value); }
        }

        private ObservableCollection<string> _frequencyOfCalibration;

        public ObservableCollection<string> FrequencyOfCalibrations
        {
            get { return _frequencyOfCalibration; }
            set { SetProperty(ref _frequencyOfCalibration, value); }
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

        private ObservableCollection<string> _status;

        public ObservableCollection<string> Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private ObservableCollection<string> _maker;
        public ObservableCollection<string> Maker
        {
            get { return _maker; }
            set { SetProperty(ref _maker, value); }
        }

        private ObservableCollection<string> _accuracy;
        public ObservableCollection<string> Accuracy
        {
            get { return _accuracy; }
            set { SetProperty(ref _accuracy, value); }
        }

        private ObservableCollection<string> _resolution;
        public ObservableCollection<string> Resolution
        {
            get { return _resolution; }
            set { SetProperty(ref _resolution, value); }
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
            CalibrationMethods = new ObservableCollection<string>(new List<string> { "Calibration Method 1", "Calibration Method 2", "Calibration Method 3", "Calibration Method 4", "Calibration Method 5"});
            CalibrationResults = new ObservableCollection<string>(new List<string> { "Calibration Result 1", "Calibration Result 2", "Calibration Result 3", "Calibration Result 4", "Calibration Result 5" });
            AcceptanceCriterias = new ObservableCollection<string>(new List<string> { "Acceptance 1", "Acceptance 2", "Acceptance 3", "Acceptance 4", "Acceptance 5" });
            FrequencyOfCalibrations = new ObservableCollection<string>(new List<string> { "Frequency of Calibration 1", "Frequency of Calibration 2", "Frequency of Calibration 3", "Frequency of Calibration 4", "Frequency of Calibration 5" });
            Descriptions = new ObservableCollection<string>(new List<string> { "Description 1", "Description 2", "Description 3", "Description 4", "Description 5" });
            Types = new ObservableCollection<string>(new List<string> { "Type 1", "Type 2", "Type 3", "Type 4", "Type 5" });
            Status = new ObservableCollection<string>(new List<string> { "Status 1", "Status 2", "Status 3", "Status 4", "Status 5" });
            Maker = new ObservableCollection<string>(new List<string> { "Maker 1", "Maker 2", "Maker 3", "Maker 4", "Maker 5" });
            Resolution = new ObservableCollection<string>(new List<string> { "Resolution 1", "Resolution 2", "Resolution 3", "Resolution 4", "Resolution 5" });

            Accuracy = new ObservableCollection<string>();
            for (int i = 1; i <= 100; i += 4)
            {
                Accuracy.Add(i.ToString());
            }

            Range = new ObservableCollection<string>();
            for (int i = 1; i <= 100; i += 9)
            {
                Range.Add(i.ToString());
            }

            this.regionManager = regionManager;
            _dialogService = dialogService;

            UpdateMeasuringDeviceCommand = new DelegateCommand<MeasuringDevice>(ExecuteUpdate);
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

            CurrentMeasuringDevice.IssuedToEmployee = IssuedToEmployee;
            CurrentMeasuringDevice.CalibratedByEmployee = CalibratedByEmployee;
            CurrentMeasuringDevice.Equipment = Equipment;
            CurrentMeasuringDevice.MachineTool = MachineTool;
            CurrentMeasuringDevice.Department = Department;

			ea.GetEvent<EquipmentToMeasuringDevice>().Subscribe(GetEquipmentDetails);
			ea.GetEvent<MachineToolToMeasuringDevice>().Subscribe(GetMachineToolDetails);

            ea.GetEvent<EmployeeLookupToMDForm>().Subscribe(SetIssuedToEmployeeFromLookup);
            ea.GetEvent<DepartmentLookupToMDForm>().Subscribe(SetDepartmentFromLookup);
        }

        private void SetDepartmentFromLookup(Department obj)
        {
            Department = obj;
        }

        private void SetIssuedToEmployeeFromLookup(Employee empObj)
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

        private void GetMachineToolDetails(MachineTool tool)
		{
            MachineTool = tool;
		}


		private void GetEquipmentDetails(Equipment equipment)
		{
			Equipment = equipment;
		}

		public MeasuringDeviceFormViewModel()
        {

        }        

        #region Command's Methods
        public void ExecuteCreateNew()
        {
            CurrentMeasuringDevice = new MeasuringDevice()
            {
                Department = new Department(),
                Location = new Location(),
                Plant = new Plant(),
                Equipment = new Equipment
                {
                    EquipmentTypeObj = new EquipmentType(),
                    Item = new Item
                    {
                        Description = new Description()
                    }
                }
            };

            Description = CurrentMeasuringDevice.Equipment.Item.Description;
            Item = CurrentMeasuringDevice.Equipment.Item;
            Equipment = CurrentMeasuringDevice.Equipment;
            EquipmentType = CurrentMeasuringDevice.Equipment.EquipmentTypeObj;

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

        public void ExecuteUpdate(MeasuringDevice measuringDeviceObj)
        {
            measuringDeviceObj.IssuedToEmployee = IssuedToEmployee;
            measuringDeviceObj.CalibratedByEmployee = CalibratedByEmployee;

            if (IsMachineTool)
                measuringDeviceObj.MachineTool = MachineTool;
            else if (IsEquipment)
                measuringDeviceObj.Equipment = Equipment;

            //measuringDeviceDA.UpdateMeasuringDevice(measuringDeviceObj);
        }

        public void ExecuteSave(MeasuringDevice measuringDeviceObj)
        {
            measuringDeviceObj.IssuedToEmployee = IssuedToEmployee;
            measuringDeviceObj.CalibratedByEmployee = CalibratedByEmployee;

            if (IsMachineTool)
                measuringDeviceObj.MachineTool = MachineTool;
            else if (IsEquipment)
                measuringDeviceObj.Equipment = Equipment;

            //measuringDeviceDA.CreateMeasuringDevice(measuringDeviceObj);
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
		}

		private void SelectEquipment()
		{
            IsEquipment = true;
            SetProperty(ref _isEquipment, true);

            IsMachineTool = false;
            SetProperty(ref _isMachineTool, false);
            regionManager.RequestNavigate("EquipmentMachineToolRegion", "EquipmentFormForMeasuringDevice");

        }

        private void ExecuteOpenLookupForIssuedToEmployee()
		{
            IsForIssuedEmployee = true;
            IsForCalibratedByEmployee = false;
            SetProperty(ref _isForIssuedEmployee, true);
            SetProperty(ref _isForCalibratedEmployee, false);

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
			IsDataEdit = true;
			SetProperty(ref _isDataEdit, IsDataEdit);
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
            SaveChangesCommand = new DelegateCommand<MeasuringDevice>(ExecuteSave);

            // check first what view will be injected to the region
            if (navigationContext.Parameters.Count != 0)
            {
                // set the default command now as Update data
                SaveChangesCommand = new DelegateCommand<MeasuringDevice>(ExecuteUpdate);
                CurrentMeasuringDevice = navigationContext.Parameters["measuringDeviceObj"] as MeasuringDevice;

                IssuedToEmployee = CurrentMeasuringDevice.IssuedToEmployee;
                CalibratedByEmployee = CurrentMeasuringDevice.CalibratedByEmployee;
                Department = CurrentMeasuringDevice.Department;

                if (CurrentMeasuringDevice.Equipment != null)
                {
                    var navParameter = new NavigationParameters();
                    navParameter.Add("equipmentObj", CurrentMeasuringDevice.Equipment);

                    IsMachineTool = false;
                    IsEquipment = true;

                    regionManager.RequestNavigate("EquipmentMachineToolRegion", "EquipmentFormForMeasuringDevice", navParameter);
                }
                else if (CurrentMeasuringDevice.MachineTool != null)
                {
                    var navParameter = new NavigationParameters();
                    navParameter.Add("equipmentObj", CurrentMeasuringDevice.MachineTool);

                    IsMachineTool = true;
                    IsEquipment = false;

                    regionManager.RequestNavigate("EquipmentMachineToolRegion", "MachineToolForMeasuringDevice", navParameter);
                }
                else
                {
                    // no view will be injected to the region
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

using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.Inventory;
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
using System.Windows;
using System.Windows.Media;
using Unity;

namespace IMTE.ViewModels
{
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
        private readonly IUnityContainer unityContainer;
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
        public DelegateCommand EmployeeConfigLookupCommand { get; }

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


        private bool _isControlsEnabled;
        public bool IsControlsEnabled
        {
            get { return _isControlsEnabled; }
            set { SetProperty(ref _isControlsEnabled, value); }
        }

        private Brush _addButtonColor;

        public Brush AddButtonColor
        {
            get { return _addButtonColor; }
            set { SetProperty(ref _addButtonColor, value); }
        }

        private bool _isAddEnabled;
        public bool IsAddEnabled
        {
            get { return _isAddEnabled; }
            set 
            { 
                if (IsDataEdit)
                {
                    value = true;
                    AddButtonColor = Brushes.Green;
                }
                else
                {
                    AddButtonColor = Brushes.DarkTurquoise;
                }

                SetProperty(ref _isAddEnabled, value);
            }
        }


        private bool _isEditEnabled;
        public bool IsEditEnabled
        {
            get { return _isEditEnabled; }
            set 
            { 
                if (IsDataEdit)
                {
                    value = true;
                }

                SetProperty(ref _isEditEnabled, value); 
            }
        }

        private bool _isSaveEnabled;
        public bool IsSaveEnabled
        {
            get { return _isSaveEnabled; }
            set 
            { 
                SetProperty(ref _isSaveEnabled, value); 
            }
        }

        private bool _isDeleteEnabled;
        public bool IsDeleteEnabled
        {
            get { return _isDeleteEnabled; }
            set { SetProperty(ref _isDeleteEnabled, value); }
        }



        #endregion

        #region Seperate complex objects for binding

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

        public MeasuringDeviceFormViewModel(IRegionManager regionManager, IDialogService dialogService, IUnityContainer unityContainer)
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

            UpdateMeasuringDeviceCommand = new DelegateCommand<MeasuringDevice>(ExecuteUpdate);
            DeleteMeasuringDeviceCommand = new DelegateCommand(ExecuteDelete);
            CreateNewMeasuringDevice = new DelegateCommand(ExecuteCreateNew);
            ToUpdateMeasringDeviceCommand = new DelegateCommand(ExecuteToUpdate);
            EmployeeConfigLookupCommand = new DelegateCommand(ExecuteOpenEmployeeConfigLookup);

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

            CurrentMeasuringDevice.IssuedToEmployee = new Employee();
            CurrentMeasuringDevice.CalibratedByEmployee = new Employee();
            CurrentMeasuringDevice.Equipment = Equipment;
            CurrentMeasuringDevice.Equipment.Item = Item;
            CurrentMeasuringDevice.Unit = UnitOfMeasurement;
            CurrentMeasuringDevice.Equipment.Item.Description = Description;
            CurrentMeasuringDevice.Equipment.EquipmentTypeObj = EquipmentType;

            
            this.regionManager = regionManager;

            this.regionManager.CreateRegionManager();
            _dialogService = dialogService;
            this.unityContainer = unityContainer;
            NavigateBackToList = new DelegateCommand<string>(Navigate);
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
            var delete = MessageBox.Show("Are you sure you want to delete this measuring device?", "Delete", MessageBoxButton.YesNo);
            if (delete == MessageBoxResult.Yes)
            {
                measuringDeviceDA.DeleteMeasuringDevice(CurrentMeasuringDevice);
                regionManager.RequestNavigate("MainRegion", "List");
            }
        }

        public void ExecuteUpdate(MeasuringDevice measuringDeviceObj)
        {
            measuringDeviceDA.UpdateMeasuringDevice(measuringDeviceObj);
        }

        public void ExecuteSave(MeasuringDevice measuringDeviceObj)
        {
            measuringDeviceDA.CreateMeasuringDevice(measuringDeviceObj);
        }

        #endregion

        private void ExecuteOpenEmployeeConfigLookup()
        {
            _dialogService.ShowDialog("EmpConfig");
        }

        private void ExecuteToUpdate()
        {
            IsDataEdit = true;
            SetProperty(ref _isDataEdit, IsDataEdit);
        }

        private void Navigate(string uri)
        {
            regionManager.RequestNavigate("MainRegion", uri);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SaveChangesCommand = new DelegateCommand<MeasuringDevice>(ExecuteSave);

            if (navigationContext.Parameters.Count >= 1)
            {
                this.CurrentMeasuringDevice = navigationContext.Parameters["measuringDeviceObj"] as MeasuringDevice;

                if (CurrentMeasuringDevice.Id != 0)
                {
                    Description = CurrentMeasuringDevice.Equipment.Item.Description;
                    Item = CurrentMeasuringDevice.Equipment.Item;
                    Equipment = CurrentMeasuringDevice.Equipment;
                    EquipmentType = CurrentMeasuringDevice.Equipment.EquipmentTypeObj;

                    IsDataEdit = true;

                    // override the SaveMeasuringDeviceCommand when there's a data to edit
                    SaveChangesCommand = new DelegateCommand<MeasuringDevice>(ExecuteUpdate);
                }
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

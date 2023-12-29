using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.IMTEEntity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.ViewModels
{
    public class MD_BorrowFormViewModel : BindableBase
    {
        private readonly EmployeeDA employeeDA;
        private readonly PlantDA plantDA;
        private readonly DepartmentDA departmentDA;
        private readonly DeviceStatusDA deviceStatusDA;
        private readonly MeasuringDeviceDA measuringDeviceDA;
        private readonly MeasuringDeviceLedgerDA measuringDeviceLedgerDA;
        private readonly IRegionManager regionManager;

        public DelegateCommand SaveLedgerCommand { get; private set; }
        public DelegateCommand BackToListCommand { get; private set; }


        public MD_BorrowFormViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            employeeDA = new EmployeeDA();
            plantDA = new PlantDA();
            departmentDA = new DepartmentDA();
            deviceStatusDA = new DeviceStatusDA();
            measuringDeviceDA = new MeasuringDeviceDA();
            measuringDeviceLedgerDA = new MeasuringDeviceLedgerDA();

            SaveLedgerCommand = new DelegateCommand(ExecuteSave);
            BackToListCommand = new DelegateCommand(ExecuteBackToList);

            Task.Run(async () => await LoadData());
        }

        private async Task LoadData()
        {
            var employees = await employeeDA.GetEmployees();
            Employees = new ObservableCollection<Employee>(employees);

            var plants = await plantDA.GetPlant();
            Plants = new ObservableCollection<Plant>(plants);

            var departments = await departmentDA.GetDepartments();
            Departments = new ObservableCollection<Department>(departments);

            var deviceStatuses = await deviceStatusDA.GetAllDeviceStatus();
            DeviceStatuses = new ObservableCollection<DeviceStatus>(deviceStatuses);

            var measuringDevices = await measuringDeviceLedgerDA.GetMeasuringDevicesWithLedgerDetails();
            MeasuringDevices = new ObservableCollection<MeasuringDeviceLedger>(measuringDevices);
        }


        private void ExecuteBackToList()
        {
            regionManager.RequestNavigate("BorrowMainRegion", "BorrowList");
        }

        private void ExecuteSave()
        {
            // Will trigger this condition if user selected multiple measuring device
            if (SelectedMeasuringDevices.Count > 1)
            {
                foreach (var measuringDevice in SelectedMeasuringDevices)
                {
                    var mdLedger = (MeasuringDeviceLedger)measuringDevice;
                    MeasuringDeviceLedger.MeasuringDevice = mdLedger.MeasuringDevice;

                    measuringDeviceLedgerDA.CreateMeasuringDeviceLedger(MeasuringDeviceLedger);
                }
            }

            // Will trigger this condition if user selected only 1 measuring device
            else if (SelectedMeasuringDevices.Count == 1)
            {
                var mdLedger = (MeasuringDeviceLedger)SelectedMeasuringDevices[0];
                MeasuringDeviceLedger.MeasuringDevice = mdLedger.MeasuringDevice;

                measuringDeviceLedgerDA.CreateMeasuringDeviceLedger(MeasuringDeviceLedger);
            }

            // Will trigger this condition if user selected no measuring device
            else
            {
                MessageBox.Show("Select a measuring device to borrow");
            }
        }


        #region Full Property

        private MeasuringDeviceLedger _measuringDeviceLedger = new MeasuringDeviceLedger();
        public MeasuringDeviceLedger MeasuringDeviceLedger
        {
            get { return _measuringDeviceLedger; }
            set { SetProperty(ref _measuringDeviceLedger, value); }
        }

        #endregion

        #region Observable Collections

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private ObservableCollection<Plant> _plants;
        public ObservableCollection<Plant> Plants
        {
            get { return _plants; }
            set { SetProperty(ref _plants, value); }
        }

        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }

        private ObservableCollection<DeviceStatus> _deviceStatuses;
        public ObservableCollection<DeviceStatus> DeviceStatuses
        {
            get { return _deviceStatuses; }
            set { SetProperty(ref _deviceStatuses, value); }
        }

        private ObservableCollection<MeasuringDeviceLedger> _measuringDevices;
        public ObservableCollection<MeasuringDeviceLedger> MeasuringDevices
        {
            get { return _measuringDevices; }
            set { SetProperty(ref _measuringDevices, value); }
        }

        #endregion

        #region Field Binding

        private ObservableCollection<object> _selectedMeasuringDevices = new ObservableCollection<object>();
        public ObservableCollection<object> SelectedMeasuringDevices
        {
            get { return _selectedMeasuringDevices; }
            set 
            {
                SetProperty(ref _selectedMeasuringDevices, value); 
            }
        }

        private Employee _issuedToEmployee = new Employee();
        public Employee IssuedToEmployee
        {
            get { return _issuedToEmployee; }
            set 
            { 
                if (value != null)
                {
                    SetProperty(ref _issuedToEmployee, value);

                    MeasuringDeviceLedger.IssuedToEmployee = value;
                }
            }
        }

        private Employee _issuedByEmployee = new Employee();
        public Employee IssuedByEmployee
        {
            get { return _issuedByEmployee; }
            set 
            { 
                if (value != null)
                {
                    SetProperty(ref _issuedByEmployee, value);

                    MeasuringDeviceLedger.IssuedByEmployee = value;
                }
            }
        }

        private Plant _plant = new Plant();
        public Plant Plant
        {
            get { return _plant; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _plant, value);

                    MeasuringDeviceLedger.Plant = value;
                }
            }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _department, value);

                    MeasuringDeviceLedger.TransferToDepartment = value;
                }
            }
        }

        private DeviceStatus _deviceStatus = new DeviceStatus();
        public DeviceStatus DeviceStatus
        {
            get { return _deviceStatus; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _deviceStatus, value);

                    MeasuringDeviceLedger.DeviceStatus = value;
                }
            }
        }

        private DateTime _transactionDate = DateTime.UtcNow;
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _transactionDate, value);

                    MeasuringDeviceLedger.TransactionDate = value;
                }
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _remarks, value);

                    MeasuringDeviceLedger.Remarks = value;
                }
            }
        }

        private int? _deviceUsage = 0;
        public int? DeviceUsage
        {
            get { return _deviceUsage; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _deviceUsage, value);

                    MeasuringDeviceLedger.DeviceUsage = value;
                }
            }
        }

        private int? _deviceRemainUsage = 0;
        public int? DeviceRemainUsage
        {
            get { return _deviceRemainUsage; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _deviceRemainUsage, value);

                    MeasuringDeviceLedger.DeviceRemainUsage = value;
                }
            }
        }

        private string _referenceNo;
        public string ReferenceNo
        {
            get { return _referenceNo; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _referenceNo, value);

                    MeasuringDeviceLedger.RefNo = value;
                }
            }
        }


        #endregion

    }
}

using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.General.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.ViewModels
{
    public class MDLookup_EmployeeConfigCreateViewModel : BindableBase
    {
        private readonly IEventAggregator ea;
        private readonly EmployeeDA employeeDA;
        private readonly EmployeeTypeDA employeeTypeDA;
        private readonly PositionDA positionDA;
        private readonly DepartmentDA departmentDA;

        public DelegateCommand SaveChangesCommand { get; private set; }

        public MDLookup_EmployeeConfigCreateViewModel(IEventAggregator ea)
        {
            this.ea = ea;
            employeeDA = new EmployeeDA();
            employeeTypeDA = new EmployeeTypeDA();
            positionDA = new PositionDA();
            departmentDA = new DepartmentDA();

            EmployeeTypes = new ObservableCollection<EmployeeType>(employeeTypeDA.GetAllEmployeeTypes());
            JobPositions = new ObservableCollection<Position>(positionDA.GetAllJobPosition());
            Departments = new ObservableCollection<Department>(departmentDA.GetAllDepartments());

            SaveChangesCommand = new DelegateCommand(SaveEmployee);

            SetEmployeeDataFromSeparateObject();
        }

        #region Helper

        private void SetEmployeeDataFromSeparateObject()
        {
            Employee.Person = Person;
            Employee.EmployeeType = EmployeeType;
            Employee.Position = Position;
            Employee.PrimaryDepartment = Department;
        }

        #endregion

        #region DelegateCommand Implementation

        private void SaveEmployee()
        {
            employeeDA.CreateEmployee(Employee);
            MessageBox.Show("Employee Saved");

            ea.GetEvent<EmployeeLookupToMDForm>().Publish(Employee);
        }

        #endregion

        #region Full Property

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }

        private Position _position = new Position();
        public Position Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }


        private EmployeeType _employeeType = new EmployeeType();
        public EmployeeType EmployeeType
        {
            get { return _employeeType; }
            set { SetProperty(ref _employeeType, value); }
        }

        private Person _person = new Person();
        public Person Person
        {
            get { return _person; }
            set { SetProperty(ref _person, value); }
        }

        private Employee _employee = new Employee();
        public Employee Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        #endregion

        #region Observable Collections

        private ObservableCollection<EmployeeType> _employeeTypes;
        public ObservableCollection<EmployeeType> EmployeeTypes
        {
            get { return _employeeTypes; }
            set { SetProperty(ref _employeeTypes, value); }
        }

        private ObservableCollection<Position> _jobPositions;
        public ObservableCollection<Position> JobPositions
        {
            get { return _jobPositions; }
            set { SetProperty(ref _jobPositions, value); }
        }

        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }


        #endregion

    }
}

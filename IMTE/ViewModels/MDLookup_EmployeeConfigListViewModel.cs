using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
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

namespace IMTE.ViewModels
{
    public class MDLookup_EmployeeConfigListViewModel : BindableBase
    {
        private readonly EmployeeDA employeeDA;
        private readonly IEventAggregator ea;

        public DelegateCommand PassSelectedObjToMDFormCommand { get; set; }

        public MDLookup_EmployeeConfigListViewModel(IEventAggregator ea)
        {
            this.ea = ea;

            employeeDA = new EmployeeDA();
			PassSelectedObjToMDFormCommand = new DelegateCommand(PassEmployeeLookupToMDForm);
            //EmployeeList = new ObservableCollection<Employee>(employeeDA.GetAllEmployees());
        }

        private void PassEmployeeLookupToMDForm()
        {
            ea.GetEvent<EmployeeLookupToMDForm>().Publish(SelectedEmployee);
        }

        #region Full Property

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { SetProperty(ref _selectedEmployee, value); }
        }

        private bool _isIssuedToEmployee;
        public bool IsIssuedToEmployee
        {
            get { return _isIssuedToEmployee; }
            set { SetProperty(ref _isIssuedToEmployee, value); }
        }


        #endregion

        #region Observable Collections

        private ObservableCollection<Employee> _employeeList;
        public ObservableCollection<Employee> EmployeeList
        {
            get { return _employeeList; }
            set { SetProperty(ref _employeeList, value); }
        }

        #endregion

    }
}

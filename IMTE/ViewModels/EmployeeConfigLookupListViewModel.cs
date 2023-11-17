using IMTE.DataAccess;
using IMTE.Models.HumanResources;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class EmployeeConfigLookupListViewModel : BindableBase
    {
        private readonly EmployeeDA employeeDA;


        private ObservableCollection<Employee> _employeeList;
        public ObservableCollection<Employee> EmployeeList
        {
            get { return _employeeList; }
            set { SetProperty(ref _employeeList, value); }
        }

        public EmployeeConfigLookupListViewModel()
        {
            employeeDA = new EmployeeDA();

            EmployeeList = new ObservableCollection<Employee>(employeeDA.GetAllEmployees());
        }

    }
}

using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
	public class MDLookup_DepartmentConfigListViewModel : BindableBase
	{
        private readonly DepartmentDA departmentDA;
        private readonly IEventAggregator ea;

        public DelegateCommand PassSelectedObjToMDFormCommand { get; set; }

        public MDLookup_DepartmentConfigListViewModel(IEventAggregator ea)
        {
            departmentDA = new DepartmentDA();
			PassSelectedObjToMDFormCommand = new DelegateCommand(PassSelectedObjToMDForm);
            this.ea = ea;

            Task.Run(async () => await LoadDepartmentsToList());
        }

        private async Task LoadDepartmentsToList()
        {
            Departments = new ObservableCollection<Department>(await departmentDA.GetDepartments());
        }

        private void PassSelectedObjToMDForm()
        {
            ea.GetEvent<DepartmentLookupToMDForm>().Publish(SelectedDepartment);
        }


        #region Full properties

        private Department _selectedDepartment = new Department();
        public Department SelectedDepartment
        {
            get { return _selectedDepartment; }
            set { SetProperty(ref _selectedDepartment, value); }
        }

        #endregion

        #region Observable Collections

        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }


        #endregion

    }
}

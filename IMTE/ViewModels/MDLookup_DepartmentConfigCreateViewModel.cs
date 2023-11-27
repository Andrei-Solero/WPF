using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class MDLookup_DepartmentConfigCreateViewModel : BindableBase
    {
        private readonly IEventAggregator ea;
        private readonly DepartmentDA departmentDA;

        public DelegateCommand SaveDepartmentCommand { get; private set; }

        public MDLookup_DepartmentConfigCreateViewModel(IEventAggregator ea)
        {
            this.ea = ea;
            departmentDA = new DepartmentDA();

            SaveDepartmentCommand = new DelegateCommand(SaveDepartment);
        }

        private void SaveDepartment()
        {
            departmentDA.CreateDepartment(Department);

            MessageBox.Show("Department Saved!", "Department", MessageBoxButton.OK);

            ea.GetEvent<DepartmentLookupToMDForm>().Publish(Department);
        }

        #region Full properties

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }

        #endregion

    }
}

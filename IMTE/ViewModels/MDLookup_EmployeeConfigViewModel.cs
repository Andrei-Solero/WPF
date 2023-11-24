using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.HumanResources;
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

namespace IMTE.ViewModels
{
    public class MDLookup_EmployeeConfigViewModel : BindableBase, IDialogAware
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand OpenEmployeeListCommand { get; private set; }
        public DelegateCommand OpenCreateCommand { get; private set; }

        public MDLookup_EmployeeConfigViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            regionManager.RequestNavigate("EmpConfigMainRegion", "EmpConfigList");

            OpenEmployeeListCommand = new DelegateCommand(OpenEmployeeList);
            OpenCreateCommand = new DelegateCommand(OpenCreateForm);
        }

        private void OpenCreateForm()
        {
            regionManager.RequestNavigate("EmpConfigMainRegion", "EmpConfigCreate");
        }

        private void OpenEmployeeList()
        {
            regionManager.RequestNavigate("EmpConfigMainRegion", "EmpConfigList");
        }

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}

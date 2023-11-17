﻿using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class EmployeeConfigLookupViewModel : IDialogAware
    {
        private readonly IRegionManager regionManager;
        public DelegateCommand EmployeeListCommand { get; set; }

        public EmployeeConfigLookupViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            EmployeeListCommand = new DelegateCommand(OpenEmployeeList);
        }

        private void OpenEmployeeList()
        {
            regionManager.RequestNavigate("EmpConfigRegion", "EmpConfigList");
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

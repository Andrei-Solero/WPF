using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	public class MDLookup_DepartmentConfigViewModel : DialogAwareBase
	{
		private readonly IRegionManager regionManager;

        public DelegateCommand OpenDepartmentListCommand { get; }
        public DelegateCommand OpenDepartmentCreateCommand { get; }

        public MDLookup_DepartmentConfigViewModel(IRegionManager regionManager)
        {
			this.regionManager = regionManager;

			regionManager.RegisterViewWithRegion("DeptConfigRegion", "DeptConfigList");

			OpenDepartmentListCommand = new DelegateCommand(OpenDepartmentList);
			OpenDepartmentCreateCommand = new DelegateCommand(OpenDepartmentCreate);
		}

		private void OpenDepartmentCreate()
		{
			regionManager.RequestNavigate("DeptConfigRegion", "DeptConfigCreate");
		}

		private void OpenDepartmentList()
		{
			regionManager.RequestNavigate("DeptConfigRegion", "DeptConfigList");
		}
	}
}

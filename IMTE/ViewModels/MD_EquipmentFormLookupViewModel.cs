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
	public class MD_EquipmentFormLookupViewModel : DialogAwareBase
	{
		private readonly IRegionManager regionManager;

		public MD_EquipmentFormLookupViewModel(IRegionManager regionManager)
		{
			this.regionManager = regionManager;

			//regionManager.RegisterViewWithRegion("EquipmentRegion", "")
		}
	}
}

using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	public class MDLookup_ItemConfigViewModel : IDialogAware
	{
		private readonly IRegionManager regionManager;

		public MDLookup_ItemConfigViewModel(IRegionManager regionManager)
        {
			this.regionManager = regionManager;

			regionManager.RegisterViewWithRegion("ItemConfigRegion", "ItemConfigList");
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

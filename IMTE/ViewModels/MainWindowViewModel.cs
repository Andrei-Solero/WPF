using IMTE.IMTEEntity.Models;
using IMTE.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
		private readonly IRegionManager regionManager;

		public MainWindowViewModel(IRegionManager regionManager)
        {
			this.regionManager = regionManager;

			regionManager.RegisterViewWithRegion("MainWindowRegion", typeof(MainWindowMenu));
		}

	}
}

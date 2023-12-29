using IMTE.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MainWindowMenuViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand OpenListCommand { get; private set; }
        public DelegateCommand OpenBorrowFormCommand { get; private set; }
        public DelegateCommand ReturnDashboardCommand { get; private set; }

        public MainWindowMenuViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            OpenListCommand = new DelegateCommand(ExecuteOpenList);
            OpenBorrowFormCommand = new DelegateCommand(ExecuteOpenForm);
            ReturnDashboardCommand = new DelegateCommand(ExecuteReturnToDashboard);
        }

        private void ExecuteOpenForm()
        {
            OpenNewTabSettings();

            regionManager.RequestNavigate("MainIMTERegion", "Borrow");
        }

        private void ExecuteReturnToDashboard()
        {
            ResetHeaderSetting();

            IRegion region = regionManager.Regions["MainIMTERegion"];
            var activeViews = region.ActiveViews;

            regionManager.Regions["MainIMTERegion"].RemoveAll();
        }

        private void ExecuteOpenList()
        {
            OpenNewTabSettings();
            regionManager.RequestNavigate("MainIMTERegion", "List");
        }

        private void OpenNewTabSettings()
        {
            HeaderSize = "auto";
            HeaderFontSize = 30;
            HeaderTextSize = "0";
            DashboardButtonVisibility = "Visible";
        }

        private void ResetHeaderSetting()
        {
            HeaderSize = "*";
            HeaderFontSize = 70;
            HeaderTextSize = "auto";
            DashboardButtonVisibility = "Hidden";
        }


        private string _headerSize = "*";
        public string HeaderSize
        {
            get { return _headerSize; }
            set { SetProperty(ref _headerSize, value); }
        }

        private int headerFontSize = 70;
        public int HeaderFontSize
        {
            get { return headerFontSize; }
            set { SetProperty(ref headerFontSize, value); }
        }

        private string _headerTextSize = "auto";
        public string HeaderTextSize
        {
            get { return _headerTextSize; }
            set { SetProperty(ref _headerTextSize, value); }
        }   

        private string _dashboardButtonVisibility = "Hidden";
        public string DashboardButtonVisibility
        {
            get { return _dashboardButtonVisibility; }
            set { SetProperty(ref _dashboardButtonVisibility, value); }
        }

    }
}

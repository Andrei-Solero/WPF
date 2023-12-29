using IMTE.ViewModels;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMTE.Views
{
    /// <summary>
    /// Interaction logic for MD_Borrow_LedgerTransaction.xaml
    /// </summary>
    public partial class MD_Borrow_LedgerTransaction : UserControl
    {
        public MD_Borrow_LedgerTransaction()
        {
            InitializeComponent();

            var regionManager = new RegionManager();

            RegionManager.SetRegionManager(this, regionManager);
            DataContext = new MD_Borrow_LedgerTransactionViewModel(regionManager);
        }
    }
}

using IMTE_dotnetframework.Models;
using IMTE_dotnetframework.ViewModels;
using Syncfusion.UI.Xaml.Grid;
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

namespace IMTE_dotnetframework.Views
{
    /// <summary>
    /// Interaction logic for MeasuringDevicesList.xaml
    /// </summary>
    public partial class MeasuringDevicesList : UserControl
    {
        public MeasuringDevicesList(Frame frameObj)
        {
            MeasuringDeviceListViewModel listVM = new MeasuringDeviceListViewModel
            {
                MainFrame = frameObj,
                
            };
            this.DataContext = listVM;
            InitializeComponent();
        }
    }
}

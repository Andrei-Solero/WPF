using IMTE_dotnetframework.ViewModels;
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

namespace IMTE_dotnetframework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWJCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH9feHRVQmVdVkZwWEQ=");
            InitializeComponent();

            MainWindowViewModel mainVM = new MainWindowViewModel();
            this.DataContext = mainVM;
        }

    }
}

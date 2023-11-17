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

namespace TestPrismUnity.Views
{
    /// <summary>
    /// Interaction logic for ViewADialogContent.xaml
    /// </summary>
    public partial class ViewADialogContent : UserControl
    {
        private readonly IRegionManager regionManager;

        public ViewADialogContent(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(cc, "viewAregionContent");
            RegionManager.SetRegionManager(cc, regionManager);
        }
    }
}

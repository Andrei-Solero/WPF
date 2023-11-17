using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFNavigationSS.ViewModels;

namespace WPFNavigationSS.Stores
{
    public class NavigationStore
    {
        public ViewModelBase CurrentViewModel { get; set; }
    }
}

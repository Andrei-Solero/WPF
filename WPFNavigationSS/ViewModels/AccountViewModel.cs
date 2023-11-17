using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFNavigationSS.Commands;
using WPFNavigationSS.Stores;

namespace WPFNavigationSS.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; set; }

        public AccountViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
        }
    }
}

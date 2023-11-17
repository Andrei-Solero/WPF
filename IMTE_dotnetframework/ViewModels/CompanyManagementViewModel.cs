using IMTE_dotnetframework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IMTE_dotnetframework.ViewModels
{
    public class CompanyManagementViewModel : BaseViewModel
    {
        public ICommand NavigateCreateNewCompany { get; set; }

        public CompanyManagementViewModel()
        {
            NavigateCreateNewCompany = new BaseCommand(OpenCreateNewCompany, CanOpenCreateNewCompany);
        }

        private bool CanOpenCreateNewCompany(object obj) => true;

        private void OpenCreateNewCompany(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

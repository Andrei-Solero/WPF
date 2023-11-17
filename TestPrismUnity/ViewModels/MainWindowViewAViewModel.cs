using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPrismUnity.ViewModels
{
    public class MainWindowViewAViewModel
    {
        private readonly IDialogService dialogService;

        public DelegateCommand OpenDialogACommand { get; private set; }

        public MainWindowViewAViewModel(IDialogService dialogService)
        {
            OpenDialogACommand = new DelegateCommand(OpenA);
            this.dialogService = dialogService;
        }

        private void OpenA()
        {
           
            dialogService.ShowDialog("ViewADialog");
        }
    }
}

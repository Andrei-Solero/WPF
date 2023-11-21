using EventAggregator.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using Prism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace EventAggregator.ViewModels
{
    public class TextDialogViewModel : BindableBase, IDialogAware 
    {
        private readonly IEventAggregator ea;
        public DelegateCommand SendSelectedTextCommand { get; set; }

        private string _selectedText;
        public string SelectedText
        {
            get { return _selectedText; }
            set { SetProperty(ref _selectedText, value); }
        }

        public TextDialogViewModel(IEventAggregator ea)
        {
            this.ea = ea;
            SendSelectedTextCommand = new DelegateCommand(SendSelectedText);
        }

        private void SendSelectedText()
        {
            ea.GetEvent<SelectedTextSentEvent>().Publish(SelectedText);
        }

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}

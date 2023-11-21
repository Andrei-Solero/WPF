using EventAggregator.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace EventAggregator.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IDialogService dialogService;
        private readonly IEventAggregator ea;

        public DelegateCommand OpenWindowCommand { get; set; }

        private string _selectedText;
        public string SelectedText
        {
            get { return _selectedText; }
            set { SetProperty(ref _selectedText, value); }
        }

        private string _title = "Prism Application";
        

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IDialogService dialogService, IEventAggregator ea)
        {
            this.dialogService = dialogService;
            this.ea = ea;
            OpenWindowCommand = new DelegateCommand(OpenWindow);

            ea.GetEvent<SelectedTextSentEvent>().Subscribe(SetSelectedText);
        }

        private void SetSelectedText(string obj)
        {
            SelectedText = obj;
        }

        private void OpenWindow()
        {
            dialogService.ShowDialog("TextDialog");
        }
    }
}

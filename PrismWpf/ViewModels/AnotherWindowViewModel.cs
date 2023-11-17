using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrismWpf.ViewModels
{
    public class AnotherWindowViewModel : BindableBase
    {
        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set 
            { 
                SetProperty(ref _isEnabled, value);
                //ButtonCommand.RaiseCanExecuteChanged();
            }
        }

        private string _updateText;

        public string UpdateText
        {
            get { return _updateText; }
            set 
            {
                SetProperty(ref _updateText, value);
            }
        }

        public DelegateCommand ButtonCommand { get; set; }

        public AnotherWindowViewModel()
        {
            ButtonCommand = new DelegateCommand(Execute).ObservesCanExecute(() => IsEnabled);
        }

        private void Execute()
        {
            UpdateText = $"Updated: {DateTime.Now}";
        }
    }
}

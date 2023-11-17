using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismWpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private Services.ICustomerStore _customerStore = null;
        public ObservableCollection<string> Customers { get; private set; } = new ObservableCollection<string>();

        public MainWindowViewModel(Services.ICustomerStore customerStore)
        {
            this._customerStore = customerStore;
        }

        private string _selectedCustomer;

        public string SelectedCustomer
        {
            get => _selectedCustomer; 
            set 
            { 
                if (SetProperty<string>(ref _selectedCustomer, value))
                {
                    _selectedCustomer = value;
                }
                
            }
        }

        private DelegateCommand _commandLoad;

        public DelegateCommand CommandLoad => _commandLoad ?? (_commandLoad = new DelegateCommand(CommandLoadExecute));

        private void CommandLoadExecute()
        {
            Customers.Clear();
            List<string> list = _customerStore.GetAll();
            foreach (string item in list)
                Customers.Add(item);
        }
    }
}

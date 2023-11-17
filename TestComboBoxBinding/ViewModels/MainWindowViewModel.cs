using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestComboBoxBinding.Models;

namespace TestComboBoxBinding.ViewModels
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _Excute { get; set; }
        private Predicate<object> _CanExcute { get; set; }


        public RelayCommand(Action<object> ExcuteMethod, Predicate<object> CanExcuteMethod)
        {

            _Excute = ExcuteMethod;
            _CanExcute = CanExcuteMethod;

        }

        public bool CanExecute(object parameter)
        {
            return _CanExcute(parameter);
        }

        public void Execute(object parameter)
        {
            _Excute(parameter);
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Fruits> _fruits;
        public ICommand SelectData { get; set; }

        public ObservableCollection<Fruits> FruitCollection
        {
            get { return _fruits; }
            set 
            {
                _fruits = value;
                OnPropertyChanged();
            }
        }

        private Fruits _selectedFruit;

        public Fruits SelectedFruit
        {
            get { return _selectedFruit; }
            set 
            { 
                _selectedFruit = value;
                OnPropertyChanged();
            }
        }


        public MainWindowViewModel()
        {
            FruitCollection = new ObservableCollection<Fruits>()
            {
                new Fruits { Id = 3, Name = "Apple", Taste = "Sweet"},
                new Fruits { Id = 2, Name = "Mango", Taste = "Sweet"},
                new Fruits { Id = 21, Name = "Kamias", Taste = "Sour"},
            };

            SelectData = new RelayCommand(Populate, CanPopulate);
        }

        private bool CanPopulate(object obj)
        {
            return true;
        }

        private void Populate(object obj)
        {
            SelectedFruit = FruitCollection[1];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFNavigation.ViewModels;

namespace WPFNavigation.Commands
{
    public class NavigationViewModel : INotifyPropertyChanged
    {
        public ICommand EmpCommand { get; set; }
        public ICommand DeptCommand { get; set; }

        private object _selectedViewModel;

        public object SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            { 
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        public NavigationViewModel()
        {
            EmpCommand = new BaseCommand(OpenEmp, CanOpenEmp);
            DeptCommand = new BaseCommand(OpenDept, CanOpenDept);
        }

        private bool CanOpenDept(object obj)
        {
            return true;
        }

        private void OpenDept(object obj)
        {
            SelectedViewModel = new DepartmentViewModel();
        }

        private bool CanOpenEmp(object obj)
        {
            return true;
        }

        private void OpenEmp(object obj)
        {
            SelectedViewModel = new EmployeeViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}


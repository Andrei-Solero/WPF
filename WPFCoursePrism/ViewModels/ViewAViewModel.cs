using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFCoursePrism.Events;
using WPFCoursePrism.Models;

namespace WPFCoursePrism.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private Person _person = new Person();

        public Person Person
        {
            get { return _person; }
            set { SetProperty(ref _person, value); }
        }


        private DateTime? _lastUpdated;
        public DateTime? LastUpdated
        {
            get { return _lastUpdated; }
            set { SetProperty(ref _lastUpdated, value); }
        }

        public ICommand UpdateCommand { get; private set; }
        private readonly IEventAggregator eventAggregator;

        public DelegateCommand LoginCommand { get; private set; }

        public ViewAViewModel(IEventAggregator eventAggregator)
        {
            //UpdateCommand = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => Person.FirstName).ObservesProperty(() => Person.LastName);
            LoginCommand = new DelegateCommand(Login, CanLogin);

            this.eventAggregator = eventAggregator;
        }

        private bool CanLogin()
        {
            return true;
        }

        private void Login()
        {
            var people = PeopleData.People();
            var person = people.FirstOrDefault(i => i.FirstName == Person.FirstName && i.LastName == Person.LastName);

        }

        private void Execute()
        {
            LastUpdated = DateTime.Now;

            eventAggregator.GetEvent<UpdatedEvent>().Publish(LastUpdated.ToString());
        }

        private bool CanExecute()
        {
            return !string.IsNullOrEmpty(Person.FirstName) && !string.IsNullOrEmpty(Person.LastName);
        }
    }
}

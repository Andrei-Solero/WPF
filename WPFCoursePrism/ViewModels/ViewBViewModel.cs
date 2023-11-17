using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFCoursePrism.Events;

namespace WPFCoursePrism.ViewModels
{
    public class ViewBViewModel : BindableBase
    {
        private string _message = "View B";
        public string Message
        {
            get { return _message; }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        private readonly IEventAggregator eventAggregator;
        public ViewBViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<UpdatedEvent>().Subscribe(Updated);
        }

        private void Updated(string obj)
        {
            Message = obj;
        }

    }
}

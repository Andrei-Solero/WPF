using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignDemo
{
    public class MainWindowViewModel 
    {
        public DelegateCommand ClickCommand { get; set; }

        public MainWindowViewModel()
        {
            ClickCommand = new DelegateCommand(ExecuteClick);
        }

        private void ExecuteClick()
        {
        }
    }
}

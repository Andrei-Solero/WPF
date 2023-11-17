using IMTE_dotnetframework.Commands;
using IMTE_dotnetframework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace IMTE_dotnetframework.ViewModels
{
    public class MainWindowViewModel
    {
        public ICommand OpenCreateMDForm { get; private set; }
        public ICommand OpenListMDForm { get; private set; }

        public MainWindowViewModel()
        {
            OpenCreateMDForm = new BaseCommand(OpenForm, CanOpenForm);
            OpenListMDForm = new BaseCommand(OpenListForm, CanOpenListForm);
        }

        private void OpenListForm(object obj)
        {
            var mainFrame = obj as Frame;

            MeasuringDevicesList createMD = new MeasuringDevicesList(mainFrame);
            mainFrame.Content = createMD;
        }

        private bool CanOpenListForm(object obj)
        {
            return true;
        }

        private bool CanOpenForm(object obj)
        {
            return true;
        }

        private void OpenForm(object obj)
        {
            var mainFrame = obj as Frame;

            AddMeasuringDevice createMD = new AddMeasuringDevice();
            mainFrame.Content = createMD;
        }
    }
}

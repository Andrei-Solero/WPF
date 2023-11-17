using IMTE_dotnetframework.Commands;
using IMTE_dotnetframework.DataAccess;
using IMTE_dotnetframework.Models;
using IMTE_dotnetframework.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace IMTE_dotnetframework.ViewModels
{
    public class MeasuringDeviceListViewModel : BaseViewModel
    {
        public ICommand PassExistingData { get; private set; }
        public ICommand DeleteSelectedData { get; private set; }
        public ObservableCollection<MeasuringDevices> MeasuringDevicesLists { get; private set; }
        public Frame MainFrame { get; set; }


        private readonly MeasuringDeviceDA measuringDeviceDA;

        private MeasuringDevices _selectedMeasuringDevices;

        public MeasuringDevices SelectedMeasuringDevice
        {
            get { return _selectedMeasuringDevices; }
            set 
            { 
                _selectedMeasuringDevices = value;
                OnPropertyChanged();
            }
        }

        public MeasuringDeviceListViewModel()
        {
            PassExistingData = new BaseCommand(PassDataToEdit, CanPassDataToEdit);
            DeleteSelectedData = new BaseCommand(DeleteData, CanDeleteData);

            measuringDeviceDA = new MeasuringDeviceDA();

            MeasuringDevicesLists = measuringDeviceDA.GetAllMeasuringDevices();
        }

        private bool CanDeleteData(object obj)
        {
            return true;
        }

        private void DeleteData(object obj)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to delete this measuring device?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                measuringDeviceDA.DeleteMeasuringDevice(SelectedMeasuringDevice);
                MessageBox.Show("Successfully Deleted");

                MainFrame.Content = new MeasuringDevicesList(MainFrame);
            }
        }

        private bool CanPassDataToEdit(object obj)
        {
            return true;
        }

        private void PassDataToEdit(object obj)
        {
            var selectedMeasuringDevice = obj as MeasuringDevices;
            ModifyMeasuringDevice createMeasuringDevice = new ModifyMeasuringDevice();

            createMeasuringDevice.DataContext = new UpdateMeasuringDeviceViewModel { MeasuringDevice = selectedMeasuringDevice };
            MainFrame.Content = createMeasuringDevice;
        }
    }
}


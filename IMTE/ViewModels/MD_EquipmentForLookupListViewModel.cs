using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Inventory;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.ViewModels
{
	public class MD_EquipmentForLookupListViewModel : BindableBase
	{
        private readonly EquipmentDA equipmentDA;
        private readonly MeasuringDeviceDA measuringDeviceDA;
        private readonly IEventAggregator ea;

        public DelegateCommand PassSelectedObjToMDFormCommand { get; private set; }

        public MD_EquipmentForLookupListViewModel(IEventAggregator ea)
        {
            equipmentDA = new EquipmentDA();
            measuringDeviceDA = new MeasuringDeviceDA();

            Equipments = new ObservableCollection<Equipment>(equipmentDA.GetAllEquipment());

            PassSelectedObjToMDFormCommand = new DelegateCommand(PassSelectedObjToForm);
            this.ea = ea;

        }

        private void PassSelectedObjToForm()
        {
            var dialogResult = MessageBox.Show("Do you want to load this equipment's measuring device?", "Load Data", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                MeasuringDevice = measuringDeviceDA.GetMeasuringDeviceBySelectedEquipment(SelectedEquipment);

                ea.GetEvent<EquipmentToMeasuringDevice>().Publish(SelectedEquipment);
                ea.GetEvent<EquipmentMachineToolWithMeasuringDeviceDataToMDForm>().Publish(MeasuringDevice);
            }
        }


        #region Observable collections

        private ObservableCollection<Equipment> _equipments;
        public ObservableCollection<Equipment> Equipments
        {
            get { return _equipments; }
            set { SetProperty(ref _equipments, value); }
        }

        #endregion

        #region Full Property

        private Equipment _selectedEquipment = new Equipment();
        public Equipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set 
            {
                SetProperty(ref _selectedEquipment, value); 
            }
        }

        private MeasuringDevice _measuringDevice = new MeasuringDevice();
        public MeasuringDevice MeasuringDevice
        {
            get { return _measuringDevice; }
            set { SetProperty(ref _measuringDevice, value); }
        }



        #endregion
    }
}

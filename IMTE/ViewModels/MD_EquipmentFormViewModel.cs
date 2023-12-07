using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.Inventory;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels

{
    [RegionMemberLifetime(KeepAlive = false)]
    public class MD_EquipmentFormViewModel : BindableBase, INavigationAware, IDataErrorInfo
    {
        private readonly IEventAggregator ea;
        private readonly IDialogService dialogService;
        private readonly EquipmentTypeDA equipmentTypeDA;

        public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
        public DelegateCommand OpenItemLookupCommand { get; private set; }
        public DelegateCommand ShowEquipmentConfigCommand { get; private set; }


        public MD_EquipmentFormViewModel(IEventAggregator ea, IDialogService dialogService)
        {
            this.ea = ea;
            this.dialogService = dialogService;
            equipmentTypeDA = new EquipmentTypeDA();

            EquipmentTypes = new ObservableCollection<EquipmentType>(equipmentTypeDA.GetAllEquipmentType());

            OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
            OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
            ShowEquipmentConfigCommand = new DelegateCommand(OpenEquipmentConfig);

            // send the equipment serial data to measuring device
            ea.GetEvent<EquipmentSerialToMeasuringDevice>().Publish(EquipmentSerial);

            // get the data from this form's description lookup
            ea.GetEvent<DescriptionLookupToMDForms>().Subscribe(SetDescriptionFromLookup);

            // get the data from this form's item lookup
            ea.GetEvent<ItemLookupToMDForms>().Subscribe(SetItemFromLookup);
        }



        private void OpenEquipmentConfig()
        {
            dialogService.ShowDialog("EquipmentConfig");
        }

        private void SetItemFromLookup(Item itemObj)
        {
            ItemEntity = itemObj;
        }

        private void SetDescriptionFromLookup(Description obj)
        {
            Description = obj;
            Equipment.Item.Description = obj;
        }

        private void OpenItemLookup()
        {
            dialogService.ShowDialog("ItemConfig");
        }

        private void OpenDescriptionLookup()
        {
            dialogService.ShowDialog("ItemDescriptionConfig");
        }

        #region Helper

        private void SetFieldBindingData(Equipment equipmentObj)
        {
            Manufacturer = equipmentObj.Manufacturer;
            Model = equipmentObj.Model;
            HasAccessory = equipmentObj.HasAccessory;
            ApprovalCode = equipmentObj.ApprovalCode;
            IsPrinted = equipmentObj.IsPrinted;
            IsForeignCurrency = equipmentObj.IsForeignCurrency;
            IsSent = equipmentObj.IsSent;
            EquipmentType = equipmentObj.EquipmentType;
            ItemEntity = equipmentObj.Item;
            Description = equipmentObj.Item.Description;
            ItemCode = equipmentObj.Item.ItemCode;
            ItemShortDescription = equipmentObj.Item.ShortDescription;
            ItemDescriptionText = equipmentObj.Item.Description.Text;
        }

        #endregion

        #region ----------------FIELD BINDING----------------

        private string _serialNO;
        public string SerialNO
        {
            get { return _serialNO; }
            set 
            {
                SetProperty(ref _serialNO, value);
                EquipmentSerial.SerialNo = value;

            }
        }


        private string _manufacturer;
        public string Manufacturer
        {
            get { return _manufacturer; }
            set 
            { 
                SetProperty(ref _manufacturer, value);

                // TODO: THIS IS NOT GOOD
                Equipment.Manufacturer = value;
                EquipmentSerial.Equipment = Equipment;
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
                Equipment.Model = value;
                EquipmentSerial.Equipment = Equipment;
            }
        }

        private bool? _hasAccessory;
        public bool? HasAccessory
        {
            get { return _hasAccessory; }
            set
            {
                SetProperty(ref _hasAccessory, value);
                Equipment.HasAccessory = value;
                EquipmentSerial.Equipment = Equipment;

            }
        }

        private string _approvalCode;
        public string ApprovalCode
        {
            get { return _approvalCode; }
            set
            {
                SetProperty(ref _approvalCode, value);
                Equipment.ApprovalCode = value;
                EquipmentSerial.Equipment = Equipment;

            }
        }

        private bool? _isPrinted;
        public bool? IsPrinted
        {
            get { return _isPrinted; }
            set
            {
                SetProperty(ref _isPrinted, value);
                Equipment.IsPrinted = value;
                EquipmentSerial.Equipment = Equipment;

            }
        }

        private bool? _isForeignCurrency;
        public bool? IsForeignCurrency
        {
            get { return _isForeignCurrency; }
            set
            {
                SetProperty(ref _isForeignCurrency, value);
                Equipment.IsForeignCurrency = value;
                EquipmentSerial.Equipment = Equipment;

            }
        }

        private bool? _isSent;
        public bool? IsSent
        {
            get { return _isSent; }
            set
            {
                SetProperty(ref _isSent, value);
                Equipment.IsSent = value;
                EquipmentSerial.Equipment = Equipment;

            }
        }

        private EquipmentType _equipmentType = new EquipmentType();
        public EquipmentType EquipmentType
        {
            get { return _equipmentType; }
            set 
            { 
                SetProperty(ref _equipmentType, value);
                Equipment.EquipmentType = value;
                EquipmentSerial.Equipment = Equipment;


            }
        }

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                SetProperty(ref _itemCode, value);
                ItemEntity.ItemCode = value;
                EquipmentSerial.Equipment.Item = ItemEntity;
            }
        }

        private string _itemShortDescription;
        public string ItemShortDescription
        {
            get { return _itemShortDescription; }
            set
            {
                SetProperty(ref _itemShortDescription, value);
                ItemEntity.ShortDescription = value;
                EquipmentSerial.Equipment.Item = ItemEntity;
            }
        }

        private string _itemDescriptiontext;
        public string ItemDescriptionText
        {
            get { return _itemDescriptiontext; }
            set
            {
                SetProperty(ref _itemDescriptiontext, value);
                Description.Text = value;
                EquipmentSerial.Equipment.Item = ItemEntity;
            }
        }

        #endregion

        #region Full Properties

        private EquipmentSerial _equipmentSerial = new EquipmentSerial();
        public EquipmentSerial EquipmentSerial
        {
            get { return _equipmentSerial; }
            set 
            {
                SetProperty(ref _equipmentSerial, value);
            }
        }


        private Description _description = new Description();
        public Description Description
        {
            get { return _description; }
            set 
            {
                SetProperty(ref _description, value);
                ItemEntity.Description = value;
            }
        }

        private Item _item = new Item();
        public Item ItemEntity
        {
            get { return _item; }
            set 
            { 
                SetProperty(ref _item, value);
                Equipment.Item = value;

                ItemCode = value.ItemCode;
                ItemShortDescription = value.ShortDescription;
                ItemDescriptionText = value.Description.Text;
            }
        }


        private Equipment _equipment = new Equipment();
        public Equipment Equipment
        {
            get { return _equipment; }
            set
            {
                SetProperty(ref _equipment, value);
            }
        }

        #endregion

        #region Observable Collections

        private ObservableCollection<EquipmentType> _equipmentTypes;
        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get { return _equipmentTypes; }
            set { SetProperty(ref _equipmentTypes, value); }
        }

        #endregion

        #region INavigationAware implementation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.Count != 0)
            {
                var equipmentObj = navigationContext.Parameters["equipmentObj"] as Equipment;
                Equipment = equipmentObj;

                // send the data of the Equipment to the main measuring device form
                ea.GetEvent<EquipmentToMeasuringDevice>().Publish(Equipment);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }


        #endregion

        #region IDataErrorInfo

        private Dictionary<string, string> _errorCollection = new Dictionary<string, string>();
        public Dictionary<string, string> ErrorCollection
        {
            get { return _errorCollection; }
            set { SetProperty(ref _errorCollection, value); }
        }


        public string Error => null;
        private string result = null;

        public string this[string columnName]
        {
            get
            {
                return result;
            }
            set
            {
				switch (columnName)
				{
					case "Manufacturer":
						if (string.IsNullOrEmpty(Manufacturer))
                        {
                            result = "Description Text cannot be empty";
                        }
                        break;

				}

				if (ErrorCollection.ContainsKey(columnName))
					ErrorCollection[columnName] = result;
				else if (result != null)
					ErrorCollection.Add(columnName, result);

				SetProperty(ref _errorCollection, ErrorCollection);
			}
        }

        #endregion

    }
}

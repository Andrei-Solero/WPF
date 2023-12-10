﻿using IMTE.DataAccess;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MD_InstrumentFormViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator ea;
        private readonly IDialogService dialogService;
        private readonly InstrumentTypeDA instrumentTypeDA;
        private readonly DepartmentDA departmentDA;

        public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
        public DelegateCommand OpenItemLookupCommand { get; private set; }
        public DelegateCommand ShowEquipmentConfigCommand { get; private set; }

        public MD_InstrumentFormViewModel(IEventAggregator ea, IDialogService dialogService)
        {
            this.ea = ea;
            this.dialogService = dialogService;
            instrumentTypeDA = new InstrumentTypeDA();
            departmentDA = new DepartmentDA();

            Instrument = new Instrument();
            Item = new Item();
            Description = new Description();

            InstrumentTypes = new ObservableCollection<InstrumentType>(instrumentTypeDA.GetAllInstrumentType());
            Departments = new ObservableCollection<Department>(departmentDA.GetAllDepartments());

            OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
            OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
            ShowEquipmentConfigCommand = new DelegateCommand(OpenEquipmentConfig);

            //// this will trigger when the user selected an existing measuring device and that measuring device has InstrumentSerial
            //ea.GetEvent<MeasuringDeviceToInstrumentSerial>().Subscribe(SetInstrumentSerialFromMeasuringDevice);

			// send the instrument data to the main measuring device form
			ea.GetEvent<InstrumentSerialToMeasuringDevice>().Publish(InstrumentSerial);

			// get the data from this form's description lookup
			ea.GetEvent<DescriptionLookupToMDForms>().Subscribe(SetDescriptionFromLookup);

            // get the data from this form's item lookup
            ea.GetEvent<ItemLookupToMDForms>().Subscribe(SetItemFromLookup);
        }

		private void SetInstrumentSerialFromMeasuringDevice(InstrumentSerial serial)
		{
            InstrumentSerial = serial;
		}

		private void OpenEquipmentConfig()
        {
            dialogService.ShowDialog("EquipmentConfig");
        }

        private void OpenItemLookup()
        {
            dialogService.ShowDialog("ItemConfig");
        }

        private void OpenDescriptionLookup()
        {
            dialogService.ShowDialog("ItemDescriptionConfig");
        }

        private void SetItemFromLookup(Item itemObj)
        {
            Item = itemObj;
        }

        private void SetDescriptionFromLookup(Description descriptionObj)
        {
            Description = descriptionObj;
        }

        #region Helper

        private void SetFieldBindingData(InstrumentSerial instrumentSerialObj)
        {
            SerialNO = instrumentSerialObj.SerialNo;
            Manufacturer = instrumentSerialObj.Instrument.Manufacturer;
            Model = instrumentSerialObj.Instrument.Model;
            HasAccessory = instrumentSerialObj.Instrument.HasAccessory;
            ApprovalCode = instrumentSerialObj.Instrument.ApprovalCode;
            IsPrinted = instrumentSerialObj.Instrument.IsPrinted;
            IsForeignCurrency = instrumentSerialObj.Instrument.IsForeignCurrency;
            IsSent = instrumentSerialObj.Instrument.IsSent;
            InstrumentType = instrumentSerialObj.Instrument.InstrumentType;
            Item = instrumentSerialObj.Instrument.Item;
            Description = instrumentSerialObj.Instrument.Item.Description;
            Department = instrumentSerialObj.Instrument.Department;
            ItemCode = instrumentSerialObj.Instrument.Item.ItemCode;
            ItemShortDescription = instrumentSerialObj.Instrument.Item.ShortDescription;
            ItemDescriptionText = instrumentSerialObj.Instrument.Item.Description.Text;
        }

        #endregion

        #region ----------------FIELD BINDING----------------

        private InstrumentSerial _instrumentSerial = new InstrumentSerial();
        public InstrumentSerial InstrumentSerial
        {
            get { return _instrumentSerial; }
            set 
            { 
                SetProperty(ref _instrumentSerial, value);

                Instrument = value.Instrument;
            }
        }


        private Instrument _instrument;
        public Instrument Instrument
        {
            get { return _instrument; }
            set 
            { 
                SetProperty(ref _instrument, value);

                InstrumentSerial.Instrument = value;
                Item = value.Item;
            }
        }

        private InstrumentType _instrumentType = new InstrumentType();
        public InstrumentType InstrumentType
        {
            get { return _instrumentType; }
            set 
            { 
                SetProperty(ref _instrumentType, value);
                Instrument.InstrumentType = value;
            }
        }

        private Item _item;
        public Item Item
        {
            get { return _item; }
            set 
            { 
                SetProperty(ref _item, value);
                Instrument.Item = value;

                if (value != null)
                {
					ItemCode = value.ItemCode;
					ItemShortDescription = value.ShortDescription;
                    Description = value.Description;
				}
            }
        }

        private Description _description;
        public Description Description
        {
            get { return _description; }
            set 
            { 
                SetProperty(ref _description, value);
                Item.Description = value;

                // For field binding
                if (value != null)
                {
                    ItemDescriptionText = value.Text;
                }
            }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set 
            {
                SetProperty(ref _department, value);
                Instrument.Department = value;
            }
        }

        private string _serialNO;
        public string SerialNO
        {
            get { return _serialNO; }
            set
            {
                SetProperty(ref _serialNO, value);
                InstrumentSerial.SerialNo = value;
            }
        }


        private string _manufacturer;
        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                SetProperty(ref _manufacturer, value);
                Instrument.Manufacturer = value;
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
                Instrument.Model = value;
            }
        }

        private bool? _hasAccessory;
        public bool? HasAccessory
        {
            get { return _hasAccessory; }
            set
            {
                SetProperty(ref _hasAccessory, value);
                Instrument.HasAccessory = value;
            }
        }

        private string _approvalCode;
        public string ApprovalCode
        {
            get { return _approvalCode; }
            set
            {
                SetProperty(ref _approvalCode, value);
                Instrument.ApprovalCode = value;
            }
        }

        private bool? _isPrinted;
        public bool? IsPrinted
        {
            get { return _isPrinted; }
            set
            {
                SetProperty(ref _isPrinted, value);
                Instrument.IsPrinted = value;
            }
        }

        private bool? _isForeignCurrency;
        public bool? IsForeignCurrency
        {
            get { return _isForeignCurrency; }
            set
            {
                SetProperty(ref _isForeignCurrency, value);
                Instrument.IsForeignCurrency = value;
            }
        }

        private bool? _isSent;
        public bool? IsSent
        {
            get { return _isSent; }
            set
            {
                SetProperty(ref _isSent, value);
                Instrument.IsSent = value;
            }
        }

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                SetProperty(ref _itemCode, value);
                Item.ItemCode = value;
            }
        }

        private string _itemShortDescription;
        public string ItemShortDescription
        {
            get { return _itemShortDescription; }
            set
            {
                SetProperty(ref _itemShortDescription, value);
                Item.ShortDescription = value;
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
            }
        }

        #endregion

        #region INavigationAware

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.Count != 0)
            {
                // Getting the data from the measuring device list - measuring device form
                InstrumentSerial = navigationContext.Parameters["instrumentObj"] as InstrumentSerial;

                // set the binding fields values from the navigationcontext parameter
                SetFieldBindingData(InstrumentSerial);

                // send the data again back to measuring device form
                ea.GetEvent<InstrumentSerialToMeasuringDevice>().Publish(InstrumentSerial);
			}
		}

        #endregion

        #region ObservableCollections

        private ObservableCollection<InstrumentType> _instrumentTypes;
        public ObservableCollection<InstrumentType> InstrumentTypes
        {
            get { return _instrumentTypes; }
            set { SetProperty(ref _instrumentTypes, value); }
        }

        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }

        #endregion


    }
}

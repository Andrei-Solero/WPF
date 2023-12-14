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
using System.Xml.Linq;

namespace IMTE.ViewModels

{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MD_EquipmentFormViewModel : BindableBase, INavigationAware, IDataErrorInfo
	{
		private readonly IEventAggregator ea;
		private readonly IDialogService dialogService;
		private readonly EquipmentTypeDA equipmentTypeDA;
		private readonly EquipmentSerialDA equipmentSerialDA;

		public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
		public DelegateCommand OpenItemLookupCommand { get; private set; }
		public DelegateCommand ShowEquipmentConfigCommand { get; private set; }
		public DelegateCommand TestCommand { get; private set; }

		public List<EquipmentSerial> ExistingEquipmentSerials { get; private set; }

		public MD_EquipmentFormViewModel(IEventAggregator ea, IDialogService dialogService)
		{
			this.ea = ea;
			this.dialogService = dialogService;
			equipmentTypeDA = new EquipmentTypeDA();
			equipmentSerialDA = new EquipmentSerialDA();

			EquipmentTypes = new ObservableCollection<EquipmentType>(equipmentTypeDA.GetAllEquipmentType());

			OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
			OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
			ShowEquipmentConfigCommand = new DelegateCommand(OpenEquipmentConfig);
			TestCommand = new DelegateCommand(Test);

			Equipment = new Equipment();
			ItemEntity = new Item();
			Description = new Description();

			// send the equipment serial data to measuring device
			ea.GetEvent<EquipmentSerialToMeasuringDevice>().Publish(EquipmentSerial);

			// get the data from this form's description lookup
			ea.GetEvent<DescriptionLookupToMDForms>().Subscribe(SetDescriptionFromLookup);

			// get the data from this form's item lookup
			ea.GetEvent<ItemLookupToMDForms>().Subscribe(SetItemFromLookup);

			// get the data from the equipment lists lookup
			ea.GetEvent<DataFromEquipmentLookup>().Subscribe(SetEquipmentDetailsFromLookup);


			ExistingEquipmentSerials = equipmentSerialDA.GetAllEquipmentSerial().ToList();
		}

		private void SetEquipmentDetailsFromLookup(Equipment equipmentObj)
		{
			Equipment = equipmentObj;
			SetFieldBindingData(equipmentObj);
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

		private void SetFieldBindingData(EquipmentSerial equipmentSerialObj)
		{
			SerialNo = equipmentSerialObj.SerialNo;
			Manufacturer = equipmentSerialObj.Equipment.Manufacturer;
			Model = equipmentSerialObj.Equipment.Model;
			HasAccessory = equipmentSerialObj.Equipment.HasAccessory;
			ApprovalCode = equipmentSerialObj.Equipment.ApprovalCode;
			IsPrinted = equipmentSerialObj.Equipment.IsPrinted;
			IsForeignCurrency = equipmentSerialObj.Equipment.IsForeignCurrency;
			IsSent = equipmentSerialObj.Equipment.IsSent;
			EquipmentType = equipmentSerialObj.Equipment.EquipmentType;
			ItemEntity = equipmentSerialObj.Equipment.Item;
			Description = equipmentSerialObj.Equipment.Item.Description;
			ItemCode = equipmentSerialObj.Equipment.Item.ItemCode;
			ItemShortDescription = equipmentSerialObj.Equipment.Item.ShortDescription;
			ItemDescriptionText = equipmentSerialObj.Equipment.Item.Description.Text;
		}

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

		private bool FieldValidation()
		{
			if (string.IsNullOrEmpty(SerialNo) /*|| string.IsNullOrEmpty(Manufacturer) || string.IsNullOrEmpty(Model) ||*/
				//string.IsNullOrEmpty(ApprovalCode) || EquipmentType.Id == null || string.IsNullOrEmpty(ItemCode) || string.IsNullOrEmpty(ItemShortDescription) ||
				/*string.IsNullOrEmpty(ItemDescriptionText)*/)
			{
				ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(false);
				return false;
			}
			else if (!string.IsNullOrEmpty(SerialNo) && ExistingEquipmentSerials.Any(x => x.SerialNo == SerialNo) && EquipmentSerial == null && EquipmentSerial.Id == 0)
			{
				ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(false);
				return false;
			}
			else
			{
				ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(true);
				return true;
			}

		}

		#endregion

		private void Test()
		{
		}


		#region ----------------FIELD BINDING----------------

		private string _serialNo;
		public string SerialNo
		{
			get { return _serialNo; }
			set
			{
				SetProperty(ref _serialNo, value);
				EquipmentSerial.SerialNo = value;

				FieldValidation();
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

				FieldValidation();
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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				FieldValidation();

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

				Equipment = value.Equipment;
			}
		}


		private Description _description;
		public Description Description
		{
			get { return _description; }
			set
			{
				SetProperty(ref _description, value);

				ItemEntity.Description = value;

				// For field binding
				if (value != null)
				{
					ItemDescriptionText = value.Text;
				}
			}
		}

		private Item _item;
		public Item ItemEntity
		{
			get { return _item; }
			set
			{
				SetProperty(ref _item, value);

				Equipment.Item = value;

				// For field binding
				if (value != null)
				{
					ItemCode = value.ItemCode;
					ItemShortDescription = value.ShortDescription;
					Description = value.Description;
				}
			}
		}


		private Equipment _equipment;
		public Equipment Equipment
		{
			get { return _equipment; }
			set
			{
				SetProperty(ref _equipment, value);

				EquipmentSerial.Equipment = value;
				ItemEntity = value.Item;
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
				// Getting the data from the measuring device list - measuring device form
				EquipmentSerial = navigationContext.Parameters["equipmentObj"] as EquipmentSerial;

				// set the binding fields values from the navigationcontext parameter
				SetFieldBindingData(EquipmentSerial);

				// send the data of the Equipment to the main measuring device form
				ea.GetEvent<EquipmentSerialToMeasuringDevice>().Publish(EquipmentSerial);
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

		public string this[string columnName]
		{
			get
			{
				string result = null;
				string errorTextForCmb = "Select required option...";
				string errorTextForText = "Required...";

				switch (columnName)
				{
					case "SerialNo":
						if (string.IsNullOrEmpty(SerialNo))
							result = errorTextForText;
						else if ( (EquipmentSerial == null || EquipmentSerial.Id == 0) && ExistingEquipmentSerials.Any(x => x.SerialNo == SerialNo))
							result = "Serial No already exists";
						break;
					case "Manufacturer":
						if (string.IsNullOrEmpty(Manufacturer))
							result = errorTextForText;
						break;
					case "Model":
						if (string.IsNullOrEmpty(Model))
							result = errorTextForText;
						break;
					case "ApprovalCode":
						if (string.IsNullOrEmpty(ApprovalCode))
							result = errorTextForText;
						break;
					case "EquipmentType":
						if (EquipmentType == null)
							result = errorTextForCmb;
						break;
					case "ItemCode":
						if (string.IsNullOrEmpty(ItemCode))
							result = errorTextForText;
						break;
					case "ItemShortDescription":
						if (string.IsNullOrEmpty(ItemShortDescription))
							result = errorTextForText;
						break;
					case "ItemDescriptionText":
						if (string.IsNullOrEmpty(ItemDescriptionText))
							result = errorTextForText;
						break;

				}

				if (ErrorCollection.ContainsKey(columnName))
					ErrorCollection[columnName] = result;
				else if (result != null)
					ErrorCollection.Add(columnName, result);

				SetProperty(ref _errorCollection, ErrorCollection);

				return result;
			}
		}

		#endregion

	}
}

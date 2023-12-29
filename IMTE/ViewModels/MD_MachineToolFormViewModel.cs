using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
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
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MD_MachineToolFormViewModel : BindableBase, INavigationAware, IDataErrorInfo
	{
		private readonly IEventAggregator ea;
		private readonly IDialogService dialogService;
		private readonly MachineToolTypeDA machineToolTypeDA;
		private readonly MachineToolSerialDA machineToolSerialDA;
		private readonly MachineToolStatusDA machineToolStatusDA;

		public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
		public DelegateCommand OpenItemLookupCommand { get; private set; }
		public DelegateCommand OpenMachineToolConfigCommand { get; private set; }
		public DelegateCommand TestCommand { get; private set; }

		public List<MachineToolSerial> ExistingMachineToolSerial { get; private set; }


		public MD_MachineToolFormViewModel(IEventAggregator ea, IDialogService dialogService)
		{
			this.ea = ea;
			this.dialogService = dialogService;
			machineToolTypeDA = new MachineToolTypeDA();
			machineToolSerialDA = new MachineToolSerialDA();
			machineToolStatusDA = new MachineToolStatusDA();

			MachineTool = new MachineTool();
			ItemEntity = new Item();
			Description = new Description();

            try
            {
				Task.Run(async () => await LoadDataToFormAsync());
			}
			catch (Exception)
            {

                throw;
            }

			// send the data of the Machine Tool to the main measuring device form
			//ea.GetEvent<MachineToolToMeasuringDevice>().Publish(MachineTool);

			// get the data from this form's description lookup
			ea.GetEvent<DescriptionLookupToMDForms>().Subscribe(SetDescriptionFromLookup);

			// get the data from this form's item lookup
			ea.GetEvent<ItemLookupToMDForms>().Subscribe(SetItemFromLookup);

			ea.GetEvent<MachineToolSerialToMeasuringDevice>().Publish(MachineToolSerial);

			OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
			OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
			OpenMachineToolConfigCommand = new DelegateCommand(OpenMachineToolConfig);
			TestCommand = new DelegateCommand(Test);

			ea.GetEvent<DataFromMachineToolLookup>().Subscribe(SetMachineToolDetailsFromLookup);
		}



		private async Task LoadDataToFormAsync()
        {
			var existingMachineToolSerial = await machineToolSerialDA.GetAllMachineToolSerial();
			ExistingMachineToolSerial =existingMachineToolSerial.ToList();

			MachineToolTypes = new ObservableCollection<MachineToolType>(await machineToolTypeDA.GetAllMachineToolType());
			MachineToolStatuses = new ObservableCollection<MachineToolStatus>(await machineToolStatusDA.GetAllMachineToolStatus());
		}

		private void SetMachineToolDetailsFromLookup(MachineTool tool)
		{
			MachineTool = tool;
			SetFieldBindingData(tool);
		}

		private void SetMachineToolDataFromMachineToolLookup(MeasuringDevice obj)
		{

		}

		private void OpenMachineToolConfig()
		{
			dialogService.ShowDialog("MachineToolConfig");
		}

		private void SetItemFromLookup(Item obj)
		{
			ItemEntity = obj;
			Description = obj.Description;

			MachineTool.Item = obj;
		}

		private void SetDescriptionFromLookup(Description obj)
		{
			Description = obj;

			MachineTool.Item.Description = obj;
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

		private void SetFieldBindingData(MachineToolSerial machineToolSerialObj)
		{
			Note = machineToolSerialObj.MachineTool.Note;
			MachineToolDescription = machineToolSerialObj.MachineTool.Description;
			ToolName = machineToolSerialObj.MachineTool.ToolName;
			UnitCost = machineToolSerialObj.MachineTool.UnitCost;
			ToolLifeUsagePcs = machineToolSerialObj.MachineTool.ToolLifeUsagePcs;
			MachineToolType = machineToolSerialObj.MachineTool.MachineToolType;
			ItemCode = machineToolSerialObj.MachineTool.Item.ItemCode;
			ItemShortDescription = machineToolSerialObj.MachineTool.Item.ShortDescription;
			ItemDescriptionText = machineToolSerialObj.MachineTool.Item.Description.Text;
			SerialNo = machineToolSerialObj.SerialNo;
			Quantity = machineToolSerialObj.Quantity;

		}

		private void SetFieldBindingData(MachineTool machineToolObj)
		{
			Note = machineToolObj.Note;
			MachineToolDescription = machineToolObj.Description;
			ToolName = machineToolObj.ToolName;
			UnitCost = machineToolObj.UnitCost;
			ToolLifeUsagePcs = machineToolObj.ToolLifeUsagePcs;
			MachineToolType = machineToolObj.MachineToolType;
			ItemCode = machineToolObj.Item.ItemCode;
			ItemShortDescription = machineToolObj.Item.ShortDescription;
			ItemDescriptionText = machineToolObj.Item.Description.Text;
		}

		#endregion

		#region ------------FIELD BINDING------------

		private string _serialNo;
		public string SerialNo
		{
			get { return _serialNo; }
			set
			{
				SetProperty(ref _serialNo, value);
				MachineToolSerial.SerialNo = value;
				FieldValidation();
			}
		}


		private string _note;
		public string Note
		{
			get { return _note; }
			set
			{
				SetProperty(ref _note, value);
				MachineTool.Note = value;
				FieldValidation();
			}
		}

		private string _machineToolDescription;
		public string MachineToolDescription
		{
			get { return _machineToolDescription; }
			set
			{
				SetProperty(ref _machineToolDescription, value);
				MachineTool.Description = value;
				FieldValidation();
			}
		}

		private string _toolName;
		public string ToolName
		{
			get { return _toolName; }
			set
			{
				SetProperty(ref _toolName, value);
				MachineTool.ToolName = value;
				FieldValidation();
			}
		}

		private decimal _unitCost;
		public decimal UnitCost
		{
			get { return _unitCost; }
			set
			{
				SetProperty(ref _unitCost, value);
				MachineTool.UnitCost = value;
				FieldValidation();

			}
		}

		private int? _toolLifeUsagePcs;
		public int? ToolLifeUsagePcs
		{
			get { return _toolLifeUsagePcs; }
			set
			{
				SetProperty(ref _toolLifeUsagePcs, value);
				MachineTool.ToolLifeUsagePcs = value;
				FieldValidation();

			}
		}

		private MachineToolType _machineToolType = new MachineToolType();
		public MachineToolType MachineToolType
		{
			get { return _machineToolType; }
			set
			{
				SetProperty(ref _machineToolType, value);
				MachineTool.MachineToolType = value;
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

		private int? _toolUsageLifePcs;
		public int? ToolUsageLifePcs
		{
			get { return _toolUsageLifePcs; }
			set
			{
				SetProperty(ref _toolUsageLifePcs, value);
				MachineToolSerial.ToolLifeUsagePcs = value;
				FieldValidation();

			}
		}

		private int? _quantity;
		public int? Quantity
		{
			get { return _quantity; }
			set
			{
				SetProperty(ref _quantity, value);
				MachineToolSerial.Quantity = value;
				FieldValidation();

			}
		}


		#endregion

		private void Test()
		{
		}


		#region Full Properties

		private Description _description;
		public Description Description
		{
			get { return _description; }
			set
			{
				SetProperty(ref _description, value);

				ItemEntity.Description = value;

				// For Field Binding
				if (value != null)
				{
					ItemDescriptionText = value.Text;
				}
			}
		}

		private Item _itemEntity;
		public Item ItemEntity
		{
			get { return _itemEntity; }
			set
			{
				SetProperty(ref _itemEntity, value);

				MachineTool.Item = value;

				// For Field Binding
				if (value != null)
				{
					ItemCode = value.ItemCode;
					ItemShortDescription = ItemEntity.ShortDescription;
					Description = value.Description;
				}
			}
		}


		private MachineTool _machineTool = new MachineTool();
		public MachineTool MachineTool
		{
			get { return _machineTool; }
			set
			{
				SetProperty(ref _machineTool, value);

				MachineToolSerial.MachineTool = MachineTool;
				ItemEntity = value.Item;
			}
		}


		private MachineToolSerial _machineToolSerial = new MachineToolSerial();
		public MachineToolSerial MachineToolSerial
		{
			get { return _machineToolSerial; }
			set
			{
				SetProperty(ref _machineToolSerial, value);

				MachineTool = value.MachineTool;
			}
		}

		private MachineToolStatus _machineToolStatus = new MachineToolStatus();
		public MachineToolStatus MachineToolStatus
		{
			get { return _machineToolStatus; }
			set
			{
				SetProperty(ref _machineToolStatus, value);
				MachineToolSerial.MachineToolStatus = value;
			}
		}

		private MachineToolSpecClass _machineToolSpecClass = new MachineToolSpecClass();
		public MachineToolSpecClass MachineToolSpecClass
		{
			get { return _machineToolSpecClass; }
			set
			{
				SetProperty(ref _machineToolSpecClass, value);
			}
		}

		#endregion

		#region Observable Collections

		private ObservableCollection<MachineToolType> _machineToolTypes;
		public ObservableCollection<MachineToolType> MachineToolTypes
		{
			get { return _machineToolTypes; }
			set { SetProperty(ref _machineToolTypes, value); }
		}

        private ObservableCollection<MachineToolStatus> _machineToolStatuses;
        public ObservableCollection<MachineToolStatus> MachineToolStatuses
        {
            get { return _machineToolStatuses; }
            set { SetProperty(ref _machineToolStatuses, value); }
        }


        #endregion

        #region IDataErrorInfo - Validation

        private bool FieldValidation()
		{
			var output = true;

			if (string.IsNullOrEmpty(Note) || 
				string.IsNullOrEmpty(MachineToolDescription) || 
				string.IsNullOrEmpty(ToolName) ||
				UnitCost < 1 || 
				ToolLifeUsagePcs < 1 || 
				MachineToolType.Id == null || 
				string.IsNullOrEmpty(ItemCode) || 
				string.IsNullOrEmpty(ItemShortDescription) ||
				string.IsNullOrEmpty(ItemDescriptionText) || 
				string.IsNullOrEmpty(SerialNo))
			{
				ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(false);
				output = false;
			}
			else if (MachineToolSerial == null || MachineToolSerial.Id == 0 || MachineToolSerial.Id == null)
			{
				if (ExistingMachineToolSerial.Any(x => x.SerialNo == SerialNo))
                {
					ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(false);
					output = false;
				}
			}

			ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(output);
			return output;

		}


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
					case "Note":
						if (string.IsNullOrEmpty(Note))
							result = errorTextForText;
						if (MachineToolSerial == null || MachineToolSerial.Id == 0 || MachineToolSerial.Id == null)
						{
							if (ExistingMachineToolSerial != null && ExistingMachineToolSerial.Any(x => x.SerialNo == SerialNo))
								result = "Serial No already exists";
						}
						break;
					case "MachineToolDescription":
						if (string.IsNullOrEmpty(MachineToolDescription))
							result = errorTextForText;
						break;
					case "ToolName":
						if (string.IsNullOrEmpty(ToolName))
							result = errorTextForText;
						break;
					case "UnitCost":
						if (UnitCost < 0)
							result = "Cannot be less than 1";
						break;
					case "ToolLifeUsagePcs":
						if (ToolLifeUsagePcs < 0)
							result = "Cannot be less than 1";
						break;
					case "MachineToolType":
						if (MachineToolType.Id == null)
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
					case "SerialNo":
						if (string.IsNullOrEmpty(SerialNo))
							result = errorTextForText;
						else if (ExistingMachineToolSerial.Any(x => x.SerialNo == SerialNo))
							result = "Serial No already exists";
						break;
					case "ToolUsageLifePcs":
						if (ToolUsageLifePcs < 0)
							result = "Cannot be less than 1";
						break;
					
						
					case "Quantity":
						if (Quantity < 0)
							result = "Cannot be less than 1";
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

		#region INavigationAware Implementation

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			if (navigationContext.Parameters.Count != 0)
			{
				// Getting the data from the measuring device list - measuring device form
				MachineToolSerial = navigationContext.Parameters["machineToolObj"] as MachineToolSerial;

				// set the binding fields values from the navigationcontext parameter
				SetFieldBindingData(MachineToolSerial);

                //// send the data again back to measuring device form
                ea.GetEvent<MachineToolSerialToMeasuringDevice>().Publish(MachineToolSerial);
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

	}
}

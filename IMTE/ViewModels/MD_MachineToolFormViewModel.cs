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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MD_MachineToolFormViewModel : BindableBase, INavigationAware
	{
		private readonly IEventAggregator ea;
		private readonly IDialogService dialogService;
		private readonly MachineToolTypeDA machineToolTypeDA;

		public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
		public DelegateCommand OpenItemLookupCommand { get; private set; }
		public DelegateCommand OpenMachineToolConfigCommand { get; private set; }
		public DelegateCommand TestCommand { get; private set; }

		public MD_MachineToolFormViewModel(IEventAggregator ea, IDialogService dialogService)
		{
			this.ea = ea;
			this.dialogService = dialogService;
			machineToolTypeDA = new MachineToolTypeDA();

			MachineToolTypes = new ObservableCollection<MachineToolType>(machineToolTypeDA.GetAllMachineToolType());

			MachineTool = new MachineTool();
			Item = new Item();
			Description = new Description();

			// send the data of the Machine Tool to the main measuring device form
			ea.GetEvent<MachineToolToMeasuringDevice>().Publish(MachineTool);

			// get the data from this form's description lookup
			ea.GetEvent<DescriptionLookupToMDForms>().Subscribe(SetDescriptionFromLookup);

			// get the data from this form's item lookup
			ea.GetEvent<ItemLookupToMDForms>().Subscribe(SetItemFromLookup);

			ea.GetEvent<MachineToolSerialToMeasuringDevice>().Publish(MachineToolSerial);

			OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
			OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
			OpenMachineToolConfigCommand = new DelegateCommand(OpenMachineToolConfig);
			TestCommand = new DelegateCommand(Test);

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
			Item = obj;
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
			ToolLifeUsagePcs = machineToolSerialObj.ToolLifeUsagePcs;
			Quantity = machineToolSerialObj.Quantity;
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

		private int _toolUsageLifePcs;
		public int ToolUsageLifePcs
		{
			get { return _toolUsageLifePcs; }
			set
			{
				SetProperty(ref _toolUsageLifePcs, value);
				MachineToolSerial.ToolLifeUsagePcs = value;
			}
		}

		private int _quantity;
		public int Quantity
		{
			get { return _quantity; }
			set
			{
				SetProperty(ref _quantity, value);
				MachineToolSerial.Quantity = value;
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

				Item.Description = value;

				// For Field Binding
				if (value != null)
				{
					ItemDescriptionText = value.Text;
				}
			}
		}

		private Item _item;
		public Item Item
		{
			get { return _item; }
			set
			{
				SetProperty(ref _item, value);

				MachineTool.Item = value;

				// For Field Binding
				if (value != null)
				{
					ItemCode = value.ItemCode;
					ItemShortDescription = Item.ShortDescription;
					Description = value.Description;
				}
			}
		}


		private MachineTool _machineTool;
		public MachineTool MachineTool
		{
			get { return _machineTool; }
			set
			{
				SetProperty(ref _machineTool, value);

				MachineToolSerial.MachineTool = value;
			}
		}


		private MachineToolSerial _machineToolSerial = new MachineToolSerial();
		public MachineToolSerial MachineToolSerial
		{
			get { return _machineToolSerial; }
			set
			{
				SetProperty(ref _machineToolSerial, value);
			}
		}

		private MachineToolStatus _machineToolStatus = new MachineToolStatus();
		public MachineToolStatus MachineToolStatus
		{
			get { return _machineToolStatus; }
			set
			{
				SetProperty(ref _machineToolStatus, value);
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

				// send the data again back to measuring device form
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

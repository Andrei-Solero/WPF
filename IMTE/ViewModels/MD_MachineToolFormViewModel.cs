using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
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

        public MD_MachineToolFormViewModel(IEventAggregator ea, IDialogService dialogService)
        {
			this.ea = ea;
			this.dialogService = dialogService;
			machineToolTypeDA = new MachineToolTypeDA();

			MachineToolTypes = new ObservableCollection<MachineToolType>(machineToolTypeDA.GetAllMachineToolType());

			// send the data of the Machine Tool to the main measuring device form
			ea.GetEvent<MachineToolToMeasuringDevice>().Publish(MachineTool);

			// get the data from this form's description lookup
			ea.GetEvent<DescriptionLookupToEQMTForm>().Subscribe(SetDescriptionFromLookup);

			// get the data from this form's item lookup
			ea.GetEvent<ItemLookupToMDForm>().Subscribe(SetItemFromLookup);

			ea.GetEvent<EquipmentMachineToolWithMeasuringDeviceDataToMDForm>().Subscribe(SetMachineToolDataFromMachineToolLookup);

			OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
			OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
			OpenMachineToolConfigCommand = new DelegateCommand(OpenMachineToolConfig);

		}

        private void SetMachineToolDataFromMachineToolLookup(MeasuringDevice obj)
        {
			if (obj.MachineTool != null)
			{
			}
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

		#endregion

		#region Full Properties

		private Description _description = new Description();
		public Description Description
		{
			get { return _description; }
			set 
			{ 
				SetProperty(ref _description, value);
				Item.Description = value;
			}
		}

		private Item _item = new Item();
		public Item Item
		{
			get { return _item; }
			set 
			{
				SetProperty(ref _item, value);
				MachineTool.Item = value;
			}
		}


		private MachineTool _machineTool = new MachineTool();
		public MachineTool MachineTool
		{
			get { return _machineTool; }
			set 
			{ 
				SetProperty(ref _machineTool, value);
				SetFieldBindingData(value);
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
				var equipmentObj = navigationContext.Parameters["equipmentObj"] as MachineTool;

				MachineTool = equipmentObj;
				Item = equipmentObj.Item;
				Description = equipmentObj.Item.Description;
				MachineToolType = equipmentObj.MachineToolType;
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

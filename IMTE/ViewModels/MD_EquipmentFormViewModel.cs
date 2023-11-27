
using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MD_EquipmentFormViewModel : BindableBase, INavigationAware
	{
		private readonly IEventAggregator ea;
        private readonly IDialogService dialogService;
        private readonly EquipmentTypeDA equipmentTypeDA;

        public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
		public DelegateCommand OpenItemLookupCommand { get; private set; }


        public MD_EquipmentFormViewModel(IEventAggregator ea, IDialogService dialogService)
        {
			this.ea = ea;
            this.dialogService = dialogService;
            equipmentTypeDA = new EquipmentTypeDA();

			ea.GetEvent<EquipmentToMeasuringDevice>().Publish(Equipment);

			Equipment.Item = Item;
			Equipment.Item.Description = Description;

			EquipmentTypes = new ObservableCollection<EquipmentType>(equipmentTypeDA.GetAllEquipmentType());

			OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
			OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);

			ea.GetEvent<DescriptionLookupToEQMTForm>().Subscribe(SetDescriptionFromLookup);
			ea.GetEvent<ItemLookupToMDForm>().Subscribe(SetItemFromLookup);
		}

		private void SetItemFromLookup(Item itemObj)
		{
			Item = itemObj;
			Description = itemObj.Description;
		}

		private void OpenItemLookup()
		{
			dialogService.ShowDialog("ItemConfig");
		}

		private void SetDescriptionFromLookup(Description obj)
        {
			Description = obj;
        }

        private void OpenDescriptionLookup()
        {
			dialogService.ShowDialog("ItemDescriptionConfig");
        }

        #region Full Properties

        private Description _description = new Description();
		public Description Description
		{
			get { return _description; }
			set { SetProperty(ref _description, value); }
		}

		private Item _item = new Item();
		public Item Item
		{
			get { return _item; }
			set { SetProperty(ref _item, value); }
		}

		private EquipmentType _equipmentType = new EquipmentType();
        public EquipmentType EquipmentType
        {
            get { return _equipmentType; }
			set { SetProperty(ref _equipmentType, value); }
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
				Item = equipmentObj.Item;
				Description = equipmentObj.Item.Description;
			}
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}

using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using IMTE.Models.Inventory;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	//[RegionMemberLifetime(KeepAlive = false)]
	public class MD_EquipmentFormViewModel : BindableBase
	{
		private readonly IEventAggregator ea;

		public MD_EquipmentFormViewModel(IEventAggregator ea)
        {
			this.ea = ea;
			ea.GetEvent<EquipmentToMeasuringDevice>().Publish(Equipment);

			Equipment.Item = Item;
			Equipment.Item.Description = Description;
		}

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

		private Equipment _equipment = new Equipment();
		public Equipment Equipment
        {
            get { return _equipment; }
            set 
			{
				SetProperty(ref _equipment, value);
			}
        }
    }
}

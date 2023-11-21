using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using IMTE.Models.Production;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MD_MachineToolFormViewModel : BindableBase
	{
		private readonly IEventAggregator ea;

		public MD_MachineToolFormViewModel(IEventAggregator ea)
        {
			this.ea = ea;

			ea.GetEvent<MachineToolToMeasuringDevice>().Publish(MachineTool);
			MachineTool.Item = Item;
			MachineTool.Item.Description = Description;
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

		private MachineTool _machineTool = new MachineTool();
		public MachineTool MachineTool
		{
			get { return _machineTool; }
			set { SetProperty(ref _machineTool, value); }
		}
	}
}

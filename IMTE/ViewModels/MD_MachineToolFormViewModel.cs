using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using IMTE.Models.Production;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	//[RegionMemberLifetime(KeepAlive = false)]
	public class MD_MachineToolFormViewModel : BindableBase, INavigationAware
	{
		private readonly IEventAggregator ea;
		private readonly MachineToolTypeDA machineToolTypeDA;

		public MD_MachineToolFormViewModel(IEventAggregator ea)
        {
			this.ea = ea;
			machineToolTypeDA = new MachineToolTypeDA();
			ea.GetEvent<MachineToolToMeasuringDevice>().Publish(MachineTool);

			MachineTool.Item = Item;
			MachineTool.Item.Description = Description;
			MachineTool.MachineToolType = MachineToolType;

			MachineToolTypes = new ObservableCollection<MachineToolType>(machineToolTypeDA.GetAllMachineToolType());

			ea.GetEvent<DescriptionLookupToEQMTForm>().Subscribe(SetDescriptionFromLookup);
		}

        private void SetDescriptionFromLookup(Description obj)
        {
			Description = obj;
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

		private MachineToolType _machineToolType = new MachineToolType();
        public MachineToolType MachineToolType
        {
            get { return _machineToolType; }
			set { SetProperty(ref _machineToolType, value); }
		}


		private MachineTool _machineTool = new MachineTool();
		public MachineTool MachineTool
		{
			get { return _machineTool; }
			set { SetProperty(ref _machineTool, value); }
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

using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Production;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.ViewModels
{
	public class MD_MachineToolFormLookupListViewModel : BindableBase
	{
		private readonly MachineToolDA machineToolDA;
		private readonly MeasuringDeviceDA measuringDeviceDA;
		private readonly IEventAggregator ea;

		public DelegateCommand PassSelectedObjToFormCommand { get; private set; }

		public MD_MachineToolFormLookupListViewModel(IEventAggregator ea)
		{
			machineToolDA = new MachineToolDA();
			measuringDeviceDA = new MeasuringDeviceDA();

			this.ea = ea;

			PassSelectedObjToFormCommand = new DelegateCommand(PassSelectedObjToForm);

			Task.Run(async () => await LoadDataToFormAsync());
		}

		private async Task LoadDataToFormAsync()
        {
			MachineTools = new ObservableCollection<MachineTool>(await machineToolDA.GetAllMachineTool());
		}

		private void PassSelectedObjToForm()
		{
			ea.GetEvent<DataFromMachineToolLookup>().Publish(SelectedMachineTool);
		}


		#region Full Properties

		private MachineTool _selectedMachineTool = new MachineTool();
		public MachineTool SelectedMachineTool
		{
			get { return _selectedMachineTool; }
			set { SetProperty(ref _selectedMachineTool, value); }
		}

		private MeasuringDevice _measuringDevice = new MeasuringDevice();
		public MeasuringDevice MeasuringDevice
		{
			get { return _measuringDevice; }
			set { SetProperty(ref _measuringDevice, value); }
		}


		#endregion

		#region Observable Collections

		private ObservableCollection<MachineTool> _machineTools;
		public ObservableCollection<MachineTool> MachineTools
		{
			get { return _machineTools; }
			set { SetProperty(ref _machineTools, value); }
		}


		#endregion

	}
}

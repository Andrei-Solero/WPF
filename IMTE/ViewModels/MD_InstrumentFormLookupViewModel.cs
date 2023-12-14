using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.Inventory;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	public class MD_InstrumentFormLookupViewModel : DialogAwareBase
	{
		private readonly IEventAggregator ea;
		private readonly InstrumentDA instrumentDA;

		public DelegateCommand PassSelectedObjToMDFormCommand { get; private set; }

		public MD_InstrumentFormLookupViewModel(IEventAggregator ea)
        {
			this.ea = ea;

			instrumentDA = new InstrumentDA();

			Instruments = new ObservableCollection<Models.Inventory.Instrument>(instrumentDA.GetAllInstruments());
			PassSelectedObjToMDFormCommand = new DelegateCommand(PassSelectedObjToForm);

		}

		private void PassSelectedObjToForm()
		{
			ea.GetEvent<DataFromIsntrumentLookup>().Publish(SelectedInstrument);
		}


		#region Observable Collections

		private ObservableCollection<Models.Inventory.Instrument> _instruments;
		public ObservableCollection<Models.Inventory.Instrument> Instruments
		{
			get { return _instruments; }
			set { SetProperty(ref _instruments, value); }
		}

		#endregion

		#region FullProperty

		private Models.Inventory.Instrument _selectedInstrument = new Models.Inventory.Instrument();
		public Models.Inventory.Instrument SelectedInstrument
		{
			get { return _selectedInstrument; }
			set { SetProperty(ref _selectedInstrument, value); }
		}

		#endregion

	}
}

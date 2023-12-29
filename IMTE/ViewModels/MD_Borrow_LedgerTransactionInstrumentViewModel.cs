using IMTE.Models.Inventory;
using IMTE.ViewModels.FieldBindings;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MD_Borrow_LedgerTransactionInstrumentViewModel : InstrumentFieldBindingNavigationAware
    {
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var instrumentSerialObj = navigationContext.Parameters["InstrumentSerialObj"] as InstrumentSerial;

            SetFieldBindingData(instrumentSerialObj);
        }
    }
}

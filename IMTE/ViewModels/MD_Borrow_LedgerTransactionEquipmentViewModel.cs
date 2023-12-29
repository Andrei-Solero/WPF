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
    public class MD_Borrow_LedgerTransactionEquipmentViewModel : EquipmentFieldBindingNavigationAware
    {
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var equipmentSerialObj = navigationContext.Parameters["EquipmentSerialObj"] as EquipmentSerial;

            SetFieldBinding(equipmentSerialObj);
        }
    }
}

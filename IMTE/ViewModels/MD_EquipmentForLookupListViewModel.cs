using IMTE.DataAccess;
using IMTE.Models.Inventory;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	public class MD_EquipmentForLookupListViewModel : BindableBase
	{
        private readonly EquipmentDA equipmentDA;

        public MD_EquipmentForLookupListViewModel()
        {
            equipmentDA = new EquipmentDA();

            Equipments = new ObservableCollection<Equipment>(equipmentDA.GetAllEquipment());
        }


        #region Observable collections

        private ObservableCollection<Equipment> _equipments;
        public ObservableCollection<Equipment> Equipments
        {
            get { return _equipments; }
            set { SetProperty(ref _equipments, value); }
        }

        #endregion
    }
}

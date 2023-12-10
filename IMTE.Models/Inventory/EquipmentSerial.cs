using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Inventory
{
    public class EquipmentSerial : StandardModel
    {
        public EquipmentSerial()
        {
            
        }

        public EquipmentSerial(int id, int version, bool isDeleted, DateTime createdOn, DateTime modifiedOn,
						   Equipment equipment, string serialNo, Employee modifiedByEmployee)
		{
			Id = id;
			Version = version;
			IsDeleted = isDeleted;
			CreatedOn = createdOn;
			ModifiedOn = modifiedOn;
			Equipment = equipment;
			SerialNo = serialNo;
			ModifiedByEmployee = modifiedByEmployee;
		}


		public Equipment Equipment { get; set; }
        public string SerialNo { get; set; }
        public Employee ModifiedByEmployee { get; set; }
    }
}

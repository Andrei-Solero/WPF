using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Inventory
{
    public class InstrumentSerial : StandardModel
    {
        public Employee ModifiedByEmployee { get; set; }
        public Instrument Instrument { get; set; }
        public string SerialNo { get; set; }
    }
}

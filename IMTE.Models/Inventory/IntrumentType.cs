using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Inventory
{
    public class InstrumentType : StandardModel
    {
        public Company Company { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

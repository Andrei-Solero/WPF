using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Production
{
    public class MachineToolStatus : StandardModel
    {
        public int ColorId { get; set; }
        public Company Company { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public string Status { get; set; }
        public bool? IsAvailable { get; set; }

        public override string ToString()
        {
            return Status;
        }

    }
}

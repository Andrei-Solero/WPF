using IMTE.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Production
{
    public class MachineToolType
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public string Description { get; set; }
        public string ToolTypeName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}

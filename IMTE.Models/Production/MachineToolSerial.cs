using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Production
{
    public class MachineToolSerial : StandardModel
    {
        public MachineTool MachineTool { get; set; }
        public string SerialNo { get; set; }
        public int? DocumentSetId { get; set; }
        public int? ToolLifeUsagePcs { get; set; }
        public int? OnMachineId { get; set; }
        public Employee ModifiedByEmployeeId { get; set; }
        public int? MachineToolSpecClassId { get; set; }
        public int? Quantity { get; set; }
        public int? ItemLotBinId { get; set; }
        public MachineToolStatus MachineToolStatus { get; set; }
    }
}

using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Production
{
    public class MachineTool
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public Company Company{ get; set; }
        public int CurrencyExchangeId { get; set; }
        public Currency Currency { get; set; }
        public Item Item { get; set; }
        public MachineToolType MachineToolType { get; set; } = new MachineToolType();
        public int ManufacturerCompanyId { get; set; }
        public Employee ModifiedByEmployeeId { get; set; }
        public int AssetId { get; set; }
        public Specification Specification { get; set; }
        public int DocumentSetId { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string ToolName { get; set; }
        public decimal UnitCost { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public byte ToolImage { get; set; }
        public int ToolLifeUsagePcs { get; set; }

    }
}

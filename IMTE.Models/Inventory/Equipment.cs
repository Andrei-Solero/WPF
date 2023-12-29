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
    public class Equipment : StandardModel
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int? AssetId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public int? CurrencyId { get; set; }
        public int? CurrencyExchangeId { get; set; }
        public Item Item { get; set; }
        public int? SpecificationId { get; set; }
        public Specification Specification { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public bool? HasAccessory { get; set; }
        public string ApprovalCode { get; set; }
        public Department Department { get; set; }
        public bool? IsPrinted { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsForeignCurrency { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

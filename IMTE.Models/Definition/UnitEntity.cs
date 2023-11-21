using IMTE.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Definition
{
    public class UnitEntity
    {
        public int? Id { get; set; }
        public int? Version { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public Employee ModifiedByEmployee{ get; set; }
        public string UnitCategory { get; set; }
        public string UnitVal { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public override string ToString()
        {
            return $"{UnitCategory} - {UnitVal}: {Description}";
        }
    }
}

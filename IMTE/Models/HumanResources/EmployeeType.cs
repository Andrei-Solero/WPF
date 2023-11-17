using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.HumanResources
{
    public class EmployeeType
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public string TypeName { get; set; }
        public bool IsEmployee { get; set; }
        public bool HasBenefits { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public override string ToString()
        {
            return $"{TypeName}";
        }
    }
}

using IMTE.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class SpecificationDetails
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int SpecificationId { get; set; }
        public Specification Specification { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public string SpecificationName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string SpecificationValue { get; set; }
        public int ItemSequence { get; set; }
        public int SpecificationCategoryDetailsId { get; set; }
        public SpecificationCategoryDetails SpecificationCategoryDetails { get; set; }

    }
}

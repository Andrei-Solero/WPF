using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class Specification
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public int SpecificationCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}

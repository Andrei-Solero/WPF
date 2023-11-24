using IMTE.General.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class Department
    {
        public int? Id { get; set; }
        public int? Version { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int? ModifiedByEmployeeId { get; set; }  
        public string DepartmentName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Description { get; set; }
        public int? LaborTypeId { get; set; }

        public override string ToString()
        {
            return this == null ? "" : $"{DepartmentName}";
        }

    }
}

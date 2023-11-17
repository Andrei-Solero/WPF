using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.General.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }
        public DateTime Birthdate { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public DateTime SinceDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int DefaultEmpCompanyId { get; set; }
        public string MiddleInitial { get; set; }
        public string SuffixName { get; set; }

        public override string ToString()
        {
            return $"{First}, {Last} {Middle    }";
        }

    }
}

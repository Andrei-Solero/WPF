using IMTE.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.IMTEEntity
{
	public class AcceptanceCriteria
	{
        public int? Id { get; set; }
        public int? Version { get; set; }
        public Employee ModifiedByEmployee { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}

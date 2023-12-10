using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class Plant
    {
        public int? Id { get; set; }
        public int? Version { get; set; }
        public int? PlantCompanyId { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public string PlantName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ParentCompanyGuid { get; set; }
        public string Description { get; set; }
        public bool IsSharedAutoNumber { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }

		public override string ToString()
		{
            return PlantName;
		}

	}
}

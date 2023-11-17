using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.HumanResources
{
    public class Position
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int CompanyId { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public string PositionName { get; set; }
        public string DutiesDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int SuggestMinLvlSectyClId { get; set; }

        public override string ToString()
        {
            return PositionName;
        }

    }
}

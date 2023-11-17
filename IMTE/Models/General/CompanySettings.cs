using IMTE.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class CompanySettings
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int CountryCodeId { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public int DefaultCurrencyId { get; set; }

    }
}

using IMTE.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Definition
{
    public class Currency
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public string LanguageCodeId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string DecimalSymbol { get; set; }
        public string SeparatorSymbol { get; set; }
        public string NegativeFormat { get; set; }
        public string PositiveFormat { get; set; }
        public string IsoCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsBoolean { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class Company
    {
        public int? Id { get; set; }
        public int? Version { get; set; }
        public string CompanyTypeId { get; set; }
        public int? MemoId { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public int? TaxRegistrationId { get; set; }
        public int? VatRegistrationId { get; set; }
        public string CompanyName { get; set; }
        public bool CanLogin { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string LegalName { get; set; }
        public bool IsTemplate { get; set; }
        public Guid? Guid { get; set; }

        public override string ToString()
        {
            return CompanyName;
        }
    }
}

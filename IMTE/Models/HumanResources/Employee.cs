using IMTE.General.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.HumanResources
{
    public class Employee
    {
        public int? Id { get; set; }
        public int? Version { get; set; }
        public int? CompanyId { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public int? EmergencyContactPersonId { get; set; }
        public int? EducationId { get; set; }
        public Position Position { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public int? PrimaryDepartmentId { get; set; }
        public Person Person { get; set; }
        public int? SpecificationId { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime? HiredDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public DateTime? TerminatedDate { get; set; }
        public DateTime? LastEvaluation { get; set; }
        public DateTime? NextEvaluation { get; set; }
        public string TaxIdentificationNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? LastRaise { get; set; }
        public bool IsTerminated { get; set; }
        public bool IsInactive { get; set; }
        public int? AssignedLocationId { get; set; }
        public int? SectyClId { get; set; }
        public DateTime? ResignedDate { get; set; }
        public Guid? Guid { get; set; }
        public int? LaborTypeId { get; set; }
        public int? PlantId { get; set; }
        public DateTime? ClearanceDate { get; set; }
        public int? OjtSchoolId { get; set; }
        public int? AgencyId { get; set; }
        public int? AccountGroupId { get; set; }

        public override string ToString()
        {
            return $"{Person.First} {Person.Last}";
        }

    }
}

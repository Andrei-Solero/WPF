using IMTE.General.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.IMTEEntity.Models
{
    public class MeasuringDevice
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int ModifiedByEmployeeId { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public Equipment Equipment { get; set; }
        public MachineTool MachineTool { get; set; }
        public Plant Plant { get; set; }
        public Employee IssuedToEmployee { get; set; }
        public Employee CalibratedByEmployee { get; set; }
        public Department Department { get; set; }
        public Location Location { get; set; }
        public Company Company { get; set; }
        public string Type { get; set; }
        public string FrequencyOfCalibration { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
        public string ResultOfCalibration { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public string Status { get; set; }
        public decimal ThreadGaugeRingGaugeUsageNo { get; set; }
        public string CalibrationRemarks { get; set; }
        public string TrgTpgAndSettingsRemarks { get; set; }
        public string Remarks { get; set; }
        public string Date { get; set; }
        public string Maker { get; set; }
        public string Resolution { get; set; }
        public string DeviceRange { get; set; }
        public string Accuracy { get; set; }
        public UnitEntity Unit { get; set; }
        public byte? CalibrationCertificate { get; set; }
        public string Barcode { get; set; }
        public string CalibrationMethod { get; set; }
        public string AcceptanceCriteria { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string SerialNo { get; set; }

    }
}

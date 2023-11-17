using System;

namespace IMTE_dotnetframework.Models
{
    public class MeasuringDevices
    {
        public int? Id { get; set; }
        public int? Version { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public int? EquipmentId { get; set; }
        public int? PlantId { get; set; }
        public int? IssuedToEmployeeId { get; set; }
        public int? CalibrateByEmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? LocationId { get; set; }
        public int? CompanyId { get; set; }
        public string Type { get; set; }
        public string FrequencyOfCalibration { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
        public string ResultOfCalibration { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public string Status { get; set; }
        public bool? ThreadGaugeRingGaugeUsageNo { get; set; }
        public string CalibrationRemarks { get; set; }
        public string TrgTpgAndSettingsRemarks { get; set; }
        public string Remarks { get; set; }
        public string Date { get; set; }
        public string Maker { get; set; }
        public string Resolution { get; set; }
        public string DeviceRange { get; set; }
        public string Accuracy { get; set; }
        public string UnitOfMeasurement { get; set; }
        public byte? CalibrationCertificate { get; set; }
        public string Barcode { get; set; }
        public string CalibrationMethod { get; set; }
        public string AcceptanceCriteria { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}

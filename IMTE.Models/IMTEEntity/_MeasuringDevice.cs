using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.Inventory;
using IMTE.Models.ModelBase;
using IMTE.Models.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.IMTEEntity
{
    public class _MeasuringDevice : StandardModel
    {
        public InstrumentSerial InstrumentSerial { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public Plant Plant { get; set; }
        public Department Department { get; set; }
        public Location Location { get; set; }
        public Company Company { get; set; }
        public UnitEntity Unit { get; set; }
        public int FrequencyOfCalibration { get; set; }
        public DateTime NextCalibrationDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Maker { get; set; }
        public string Resolution { get; set; }
        public string DeviceRange { get; set; }
        public string Accuracy { get; set; }
        public string Barcode { get; set; }
        public string CalibrationMethod { get; set; }
        public string AcceptanceCriteria { get; set; }
        public string Description { get; set; }
        public string SerialNo { get; set; }
        public DeviceType DeviceType { get; set; }
        public EquipmentSerial EquipmentSerial { get; set; }
        public MachineToolSerial MachineToolSerial { get; set; }
        public int MeasuringDeviceTemplateId { get; set; }
        public bool IsCalibrationCertificate { get; set; }
        public string JsonProperties { get; set; }
        public DateTime EndOfLife { get; set; }

    }
}

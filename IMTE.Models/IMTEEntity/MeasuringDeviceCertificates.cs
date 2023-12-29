using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.IMTEEntity
{
    public class MeasuringDeviceCertificates : StandardModel
    {
        public MeasuringDevice MeasuringDevice { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public Company Company { get; set; }
        public byte[] CalibrationCerticate { get; set; }
        public Employee CalibratedByEmployee { get; set; }
        public string CalibrationMethod { get; set; }
        public DateTime? CalibratedOn { get; set; }
        public int? UsageNoOnCalibration { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public string Remarks { get; set; }
        public string AcceptanceCriteria { get; set; }
        public string CalibrationResult { get; set; }
    }
}

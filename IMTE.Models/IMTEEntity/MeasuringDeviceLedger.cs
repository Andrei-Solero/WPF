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
    public class MeasuringDeviceLedger : StandardModel
    {
        public _MeasuringDevice MeasuringDevice { get; set; }
        public string RefNo { get; set; }
        public Employee IssuedToEmployeeId { get; set; }
        public Employee IssuedByEmployeeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Plant Plant { get; set; }
        public Department TransferToDepartmentId { get; set; }
        public int WorkOrderId { get; set; }
        public string Remarks { get; set; }
        public DeviceStatus DeviceStatus { get; set; }
        public int DeviceUsage { get; set; }
        public int DeviceRemainUsage { get; set; }

    }
}

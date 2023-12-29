using IMTE.Models.IMTEEntity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.EventAggregator.Core.EventAggregators
{
    public class MeasuringDeviceCertificatesData : PubSubEvent<MeasuringDeviceCertificates> { }
    public class SaveMeasuringDeviceCertificate : PubSubEvent<bool> { }
    public class SaveMeasuringDeviceCertificateDirectlyToForm : PubSubEvent<bool> { }


}

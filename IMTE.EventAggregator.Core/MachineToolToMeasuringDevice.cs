using IMTE.Models.Production;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.EventAggregator.Core
{
	public class MachineToolToMeasuringDevice : PubSubEvent<MachineTool>
	{
	}
}

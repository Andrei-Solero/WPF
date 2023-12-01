using IMTE.Models.Definition;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.EventAggregator.Core
{
    public class UnitToMDForm : PubSubEvent<UnitEntity>
    {
    }
}

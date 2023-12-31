﻿using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Production
{
    public class MachineToolSpecClass : StandardModel
    {
        public MachineTool MachineTool { get; set; }
        public int RawMaterialSpecId { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public string ClassName { get; set; }
        public int ToolUsagePcs { get; set; }
        public int ClassSequence { get; set; }
    }
}

using IMTE.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class MachineToolTest
    {
        [TestMethod]
        public void GetAllMachineTool()
        {
            MachineToolDA machineToolDA = new MachineToolDA();
            var a = machineToolDA.GetAllMachineTool();
        }

        

    }
}

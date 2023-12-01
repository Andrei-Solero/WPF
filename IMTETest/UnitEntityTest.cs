using IMTE.DataAccess;
using IMTE.Models.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class UnitEntityTest
    {
        [TestMethod]
        public void SaveUnit()
        {
            UnitEntity ue = new UnitEntity { UnitCategory = "Sample", UnitVal = "SA" };
            UnitDA unitDA = new UnitDA();

            unitDA.CreateUnit(ue);
        }
    }
}

using IMTE.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTETest
{
    [TestClass]
    public class PositionTest
    {
        [TestMethod]
        public void GetAllJobPosition()
        {
            PositionDA posDa = new PositionDA();
            var a = posDa.GetAllJobPosition();
        }
    }
}

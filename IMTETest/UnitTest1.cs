using IMTE.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetAllPlant()
        {
            PlantDA da = new PlantDA();
            var a = da.GetAllPlant();
        }
    }
}

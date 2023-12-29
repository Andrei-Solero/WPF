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
            var a = da.GetPlant();
        }


        [TestMethod]
        public void GetAllCertificates()
        {
            //MeasuringDeviceCertificateDA da = new MeasuringDeviceCertificateDA();
            //var a = da.GetMeasuringDeviceCertificatesAsync();
            //var b = a.Result;
        }
    }
}

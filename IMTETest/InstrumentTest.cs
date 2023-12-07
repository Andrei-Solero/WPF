using IMTE.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class InstrumentTest
    {
        [TestMethod]
        public void GetAllInstrumentType()
        {
            InstrumentTypeDA da = new InstrumentTypeDA();
            var a = da.GetAllInstrumentType();
        }

        [TestMethod]
        public void GetAllInstrument()
        {
            InstrumentDA da = new InstrumentDA();
            var a = da.GetAllInstruments();
        }


        [TestMethod]
        public void GetAllInstrumentSerial()
        {
            InstrumentSerialDA da = new InstrumentSerialDA();
            var a = da.GetAllInstrumentSerial();
        }
    }

}

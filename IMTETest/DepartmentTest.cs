using IMTE.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class DepartmentTest
    {
        [TestMethod]
        public void GetAllDepartment()
        {
            MeasuringDeviceFormViewModel mdVM = new MeasuringDeviceFormViewModel();
            var a = mdVM.Departments;
        }
    }
}

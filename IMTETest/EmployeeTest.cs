using IMTE.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class EmployeeTest
    {
        [TestMethod]
        public void GetAllEmployees()
        {
            MeasuringDeviceFormViewModel mdVM = new MeasuringDeviceFormViewModel();
            var employees = mdVM.Employees;
        }
    }
}

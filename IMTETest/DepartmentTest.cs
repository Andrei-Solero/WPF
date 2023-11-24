using IMTE.DataAccess;
using IMTE.Models.General;
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

        [TestMethod]
        public void SaveDepartment()
        {
            DepartmentDA deptDA = new DepartmentDA();

            Department dept = new Department
            {
                DepartmentName = "Test",
                Description = "Test Description"
            };

            deptDA.CreateDepartment(dept);
        }

    }
}

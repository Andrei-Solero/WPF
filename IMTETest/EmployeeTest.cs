using IMTE.DataAccess;
using IMTE.Models.HumanResources;
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


        [TestMethod]
        public void SaveEmployee()
        {
            Employee emp = new Employee
            {
                Person = new IMTE.General.Models.Person
                {
                    First = "Andrei",
                    Last = "Solero",
                    Middle = "Manlangit",
                    Birthdate = DateTime.UtcNow
                },
                EmployeeNo = "000022",
                EmployeeType = new EmployeeType
                {
                    Id = 1
                },
                Position = new Position
                {
                    Id = 7
                },
                PrimaryDepartment = new IMTE.Models.General.Department
                {
                    Id = 3
                }
            };

            EmployeeDA empDa = new EmployeeDA();
            empDa.CreateEmployee(emp);
        }

        [TestMethod]
        public void GetAllEmployeeType()
        {
            EmployeeTypeDA empTypeDa = new EmployeeTypeDA();
            var a = empTypeDa.GetAllEmployeeTypes();
        }


        [TestMethod]
        public void GetAllEmployee()
        {
            EmployeeDA da = new EmployeeDA();
            var a = da.GetEmployeesAsync().Result;
        }
    }
}

using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class MDTEst
    {
        [TestMethod]
        public void SaveMeasuringDeviceWithEquipment()
        {
            MeasuringDevice md = new MeasuringDevice
            {
                SerialNo = "1110",
                IssuedToEmployee = new Employee
                {
                    Id = 1
                },
                Department = new Department
                {
                    Id = 1
                },
                Location = new Location
                {
                    Id = 1
                },
                Plant = new Plant
                {
                    Id = 4
                },
                CalibratedByEmployee = new Employee
                {
                    Id = 4
                },
                ResultOfCalibration = "Calibration Result 1",
                CalibrationMethod = "Calibration Method 1",
                AcceptanceCriteria = "Acceptance 1",
                FrequencyOfCalibration = "Frequency of Calibration 1",
                LastCalibrationDate = DateTime.Now,
                NextCalibrationDate = DateTime.Now.AddMonths(1),
                CalibrationRemarks = "TestTestTestTestTestTestTest",
                ThreadGaugeRingGaugeUsageNo = 4,
                Status = "Status 1",
                Barcode = "0000022",
                Remarks = "SAMPLESAMPLESAMPLESAMPLESAMPLESAMPLE",
                Date = DateTime.Now.ToString(),
                Maker = "Maker 1",
                Resolution = "Resolution 1",
                DeviceRange = "1",
                Accuracy = "1",
                Unit = new IMTE.Models.Definition.UnitEntity
                {
                    Id = 3,
                },
                Equipment = new IMTE.Models.Inventory.Equipment
                {
                    Item = new Item
                    {
                        ItemCode = "samplesamplesamplesample",
                        ShortDescription = "samplesamplesample",
                        Description = new Description
                        {
                            Text = "samplesamplesamplesample"
                        }
                    },
                    Manufacturer = "samplesamplesamplesample",
                    Model = "samplesamplesamplesample",
                    HasAccessory = true,
                    ApprovalCode = "samplesamplesample",
                    IsPrinted = true,
                    IsForeignCurrency = true,
                    IsSent = true,
                    EquipmentTypeObj = new IMTE.Models.Inventory.EquipmentType
                    {
                        Id = 2
                    }
                }
            };

            MeasuringDeviceDA da = new MeasuringDeviceDA();
            da.CreateMeasuringDevice(md);
        }

        [TestMethod]
        public void SaveMeasuringDeviceWithMachineTool()
        {
            MeasuringDevice md = new MeasuringDevice
            {
                SerialNo = "1110",
                IssuedToEmployee = new Employee
                {
                    Id = 1
                },
                Department = new Department
                {
                    Id = 1
                },
                Location = new Location
                {
                    Id = 1
                },
                Plant = new Plant
                {
                    Id = 4
                },
                CalibratedByEmployee = new Employee
                {
                    Id = 4
                },
                ResultOfCalibration = "Calibration Result 1",
                CalibrationMethod = "Calibration Method 1",
                AcceptanceCriteria = "Acceptance 1",
                FrequencyOfCalibration = "Frequency of Calibration 1",
                LastCalibrationDate = DateTime.Now,
                NextCalibrationDate = DateTime.Now.AddMonths(1),
                CalibrationRemarks = "TestTestTestTestTestTestTest",
                ThreadGaugeRingGaugeUsageNo = 4,
                Status = "Status 1",
                Barcode = "0000022",
                Remarks = "SAMPLESAMPLESAMPLESAMPLESAMPLESAMPLE",
                Date = DateTime.Now.ToString(),
                Maker = "Maker 1",
                Resolution = "Resolution 1",
                DeviceRange = "1",
                Accuracy = "1",
                Unit = new IMTE.Models.Definition.UnitEntity
                {
                    Id = 3,
                },
                MachineTool = new IMTE.Models.Production.MachineTool
                {
                    Item = new Item
                    {
                        ItemCode = "AA222",
                        ShortDescription = "SAMPLESAMPLESAMPLESAMPLE",
                        Description = new Description
                        {
                            Text = "SAMPLESAMPLESAMPLESAMPLE"
                        }
                    },
                    Note = "SAMPLESAMPLESAMPLESAMPLE",
                    ToolName = "SAMPLESAMPLESAMPLESAMPLE",
                    UnitCost = 2333,
                    ToolLifeUsagePcs = 4,
                    MachineToolType = new IMTE.Models.Production.MachineToolType
                    {
                        Id = 1
                    }
                }
            };

            MeasuringDeviceDA da = new MeasuringDeviceDA();
            da.CreateMeasuringDevice(md);
        }

        [TestMethod]
        public void GetAllMD()
        {
            MeasuringDeviceDA da = new MeasuringDeviceDA();
            var a = da.GetAllMeasuringDevices();
        }
    }
}

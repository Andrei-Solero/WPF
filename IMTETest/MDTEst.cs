using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.IMTEEntity;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
using IMTE.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class MDTEst
    {
        [TestMethod]
        public void GetMDByEquipment()
        {
            Equipment eq = new Equipment { Id = 27 };
            MeasuringDeviceDA mdDA = new MeasuringDeviceDA();

            var a = mdDA.GetMeasuringDeviceBySelectedEquipment(eq);
        }

        [TestMethod]
        public void GetMDByMachineTool()
        {
            MachineTool mt = new MachineTool { Id = 1 };
            MeasuringDeviceDA mdDA = new MeasuringDeviceDA();

            var a = mdDA.GetMeasuringDeviceBySelectedMachineTool(mt);
        }

        [TestMethod]
        public void SaveMeasuringDeviceWithEquipment()
        {
            MeasuringDevice md = new MeasuringDevice
            {
                SerialNo = "1110",
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
                DeviceType = new DeviceType { Id = 1 },
                EquipmentSerial = new EquipmentSerial
                {
                    SerialNo = "iahsdioasd",
                    Equipment = new Equipment
                    {
                        Manufacturer = "BAGONG EQUIPMENT",
                        Model = "AHHEHASE",
                        HasAccessory = true,
                        ApprovalCode = "HASHDASD",
                        EquipmentType = new EquipmentType { Id = 2 },
                        IsForeignCurrency = true,
                        IsPrinted = true,
                        IsSent = true,
                        Item = new Item
                        {
                            Id = 938,
                            //ItemCode = "EKSALAMNEYO",
                            //ShortDescription = "HAHA",
                            Description = new Description
                            {
                                Id = 28
                                //Text = "HAHAHA"
                            }
                        }
                    }
                },
                Description = "Sample Description",
                CalibrationMethod = "Calibration Method 1",
                AcceptanceCriteria = "Acceptance 1",
                FrequencyOfCalibration = "Frequency of Calibration 1",
                NextCalibrationDate = DateTime.Now.AddMonths(1),
                Status = "Status 1",
                Barcode = "0000022",
                Remarks = "SAMPLESAMPLESAMPLESAMPLESAMPLESAMPLE",
                Maker = "Maker 1",
                Resolution = "Resolution 1",
                DeviceRange = "1",
                Accuracy = "1",
                Unit = new IMTE.Models.Definition.UnitEntity
                {
                    Id = 3,
                },
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

                CalibrationMethod = "Calibration Method 1",
                AcceptanceCriteria = "Acceptance 1",
                FrequencyOfCalibration = "Frequency of Calibration 1",
                NextCalibrationDate = DateTime.Now.AddMonths(1),
                Status = "Status 1",
                Barcode = "0000022",
                Remarks = "SAMPLESAMPLESAMPLESAMPLESAMPLESAMPLE",
                Maker = "Maker 1",
                Resolution = "Resolution 1",
                DeviceRange = "1",
                Accuracy = "1",
                Unit = new IMTE.Models.Definition.UnitEntity
                {
                    Id = 3,
                },
                
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

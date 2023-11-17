using IMTE.IMTEEntity.Models;
using IMTE.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
    [TestClass]
    public class MDTEst
    {
        [TestMethod]
        public void CanSaveMeauringToIMTEDatabase()
        {
            MeasuringDevice measuringDeviceObj = null;
            //MeasuringDevice measuringDeviceObj = new MeasuringDevice
            //{
            //    Version = 1,
            //    IssuedToEmployeeId = 101,
            //    Type = "Sample Type",
            //    DepartmentId = 1,
            //    LocationId = 13,
            //    CalibrateByEmployeeId = 23,
            //    ResultOfCalibration = "Passed",
            //    CalibrationMethod = "Sample",
            //    AcceptanceCriteria = "Okay",
            //    FrequencyOfCalibration = "Sample",
            //    LastCalibrationDate = DateTime.Today,
            //    NextCalibrationDate = DateTime.Today.AddMonths(1),
            //    CalibrationRemarks = "Sample",
            //    ThreadGaugeRingGaugeUsageNo = 23,
            //    TrgTpgAndSettingsRemarks = "23",
            //    Status = "sample",
            //    Remarks = "Passed",
            //    Date = "Date",
            //    Barcode = "91273hkashdioashdisad",
            //    Maker = "Sample maker",
            //    Resolution = "Sample resolution",
            //    DeviceRange = "Far",
            //    Accuracy = "100",
            //    UnitOfMeasurement = "US"
            //};

            MeasuringDeviceFormViewModel addNewMDVM = new MeasuringDeviceFormViewModel();
            addNewMDVM.ExecuteSave(measuringDeviceObj);
        }

        [TestMethod]
        public void CanUpdateMeasuringDevice()
        {
            //MeasuringDevice measuringDeviceObj = null;
            MeasuringDevice measuringDeviceObj = new MeasuringDevice
            {
                Id = 9,
                Version = 1,
                Type = "Sample Type",
                ResultOfCalibration = "Passed",
                CalibrationMethod = "Sample",
                AcceptanceCriteria = "Okay",
                FrequencyOfCalibration = "Sample",
                LastCalibrationDate = DateTime.Today,
                NextCalibrationDate = DateTime.Today.AddMonths(1),
                CalibrationRemarks = "Sample",
                ThreadGaugeRingGaugeUsageNo = 23,
                TrgTpgAndSettingsRemarks = "23",
                Status = "sample",
                Remarks = "Passed",
                Date = "Date",
                Barcode = "91273hkashdioashdisad",
                Maker = "Sample maker",
                Resolution = "Sample resolution",
                DeviceRange = "Far",
                Accuracy = "100",
            };

            MeasuringDeviceFormViewModel addNewMDVM = new MeasuringDeviceFormViewModel();
            addNewMDVM.ExecuteUpdate(measuringDeviceObj);
        }

        [TestMethod]
        public void CanUDeleteMeasuringDevice()
        {
            //MeasuringDevice measuringDeviceObj = null;
            MeasuringDevice measuringDeviceObj = new MeasuringDevice
            {
                Id = 9,
                Version = 1,
                Type = "Sample Type",
                ResultOfCalibration = "Passed",
                CalibrationMethod = "Sample",
                AcceptanceCriteria = "Okay",
                FrequencyOfCalibration = "Sample",
                LastCalibrationDate = DateTime.Today,
                NextCalibrationDate = DateTime.Today.AddMonths(1),
                CalibrationRemarks = "Sample",
                ThreadGaugeRingGaugeUsageNo = 23,
                TrgTpgAndSettingsRemarks = "23",
                Status = "sample",
                Remarks = "Passed",
                Date = "Date",
                Barcode = "91273hkashdioashdisad",
                Maker = "Sample maker",
                Resolution = "Sample resolution",
                DeviceRange = "Far",
                Accuracy = "100",
            };

            MeasuringDeviceFormViewModel addNewMDVM = new MeasuringDeviceFormViewModel();
            //addNewMDVM.ExecuteDelete(measuringDeviceObj);
        }


        [TestMethod]
        public void GetAll()
        {
            MeasuringDeviceListViewModel mdList = new MeasuringDeviceListViewModel();
            var a = mdList.MeasuringDeviceList;
        }

    }
}

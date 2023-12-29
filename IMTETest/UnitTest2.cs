using IMTE.DataAccess;
using IMTE.IMTEEntity.Models;
using IMTE.Models.IMTEEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace IMTETest
{
	[TestClass]
	public class SmallTest
	{
		[TestMethod]
		public void GetAcceptanceCriterias()
		{
			var da = new AcceptanceCriteriaDA();
			var a = da.GetAcceptanceCriteriasAsync();
		}

		[TestMethod]
		public void GetMachineToolStatus()
		{
			var da = new MachineToolStatusDA();
			var a = da.GetAllMachineToolStatus().Result;
		}

		[TestMethod]
		public void GetEquipmentSerial()
		{
			var da = new EquipmentSerialDA();
			var a = da.GetEquipmentSerialAsync();
		}

		[TestMethod]
		public void GetMachineToolSerial()
		{
			var da = new MachineToolSerialDA();
			var a = da.GetAllMachineToolSerial();
		}


		[TestMethod]
		public void GetDeviceStatus()
		{
			var da = new DeviceStatusDA();
		}


		[TestMethod]
		public void CreateMDLedger()
		{
			var mdLedger = new MeasuringDeviceLedger
			{
				MeasuringDevice = new MeasuringDevice { Id = 23 },
				RefNo = "131ASF12",
				IssuedToEmployee = new IMTE.Models.HumanResources.Employee { Id = 58 },
				IssuedByEmployee = new IMTE.Models.HumanResources.Employee { Id = 68 },
				TransactionDate = DateTime.UtcNow,
				Plant = new IMTE.Models.General.Plant { Id = 4 },
				TransferToDepartment = new IMTE.Models.General.Department {  Id = 11 },
				Remarks = "DTEST REMAKRS DTEST REMAKRSDTEST REMAKRSDTEST REMAKRSDTEST REMAKRSDTEST REMAKRS",
				DeviceStatus = new DeviceStatus { Id = 3 },
				DeviceUsage = 3,
				DeviceRemainUsage = 5
			};

			var da = new MeasuringDeviceLedgerDA();
			da.CreateMeasuringDeviceLedger(mdLedger);
		}


		[TestMethod]
		public void GetMDForLedger()
        {
			var da = new MeasuringDeviceLedgerDA();
			var a = da.GetMeasuringDevicesWithLedgerDetails();
        }


		[TestMethod]
		public void GetMDLedgerByMD()
		{
			var da = new MeasuringDeviceLedgerDA();

			var md = new MeasuringDevice { Id = 24 };
			Task.Run(async () => await da.GetMeasuringDeviceLedgers(md));
		}
	}
}

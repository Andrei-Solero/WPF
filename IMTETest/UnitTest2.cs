using IMTE.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
	[TestClass]
	public class SmallTest
	{
		[TestMethod]
		public void GetAcceptanceCriterias()
		{
			var da = new AcceptanceCriteriaDA();
			var a = da.GetAllAcceptanceCriteria();
		}

		[TestMethod]
		public void GetEquipmentSerial()
		{
			var da = new EquipmentSerialDA();
			var a = da.GetAllEquipmentSerial();
		}

		[TestMethod]
		public void GetMachineToolSerial()
		{
			var da = new MachineToolSerialDA();
			var a = da.GetAllMachineToolSerial();
		}
	}
}

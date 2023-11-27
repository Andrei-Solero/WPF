using IMTE.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IMTETest
{
	[TestClass]
	public class EquipmentTest
	{
		[TestMethod]
		public void GetAllEquipment()
		{
			EquipmentDA equipmentDA = new EquipmentDA();
			var a = equipmentDA.GetAllEquipment();
		}
	}
}

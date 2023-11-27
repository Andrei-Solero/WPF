using IMTE.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTETest
{
	[TestClass]
	public class Itemtest
	{

		[TestMethod]
		public void GetAllItems()
		{
			ItemDA itemDA = new ItemDA();
			var a = itemDA.GetAllItems();
		}

	}
}

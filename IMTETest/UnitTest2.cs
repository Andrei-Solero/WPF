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
	}
}

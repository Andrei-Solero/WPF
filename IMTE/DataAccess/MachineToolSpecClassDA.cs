using IMTE.Models.General;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
	public class MachineToolSpecClassDA : DataAccessFunctions<MachineToolSpecClass>
	{
		public IEnumerable<MachineToolSpecClass> GetAllMachineToolSpecClass()
		{
			var output = new List<MachineToolSpecClass>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"";


				connection.Open();
				command.Connection = connection;
				command.CommandText = query;

				var data = command.ExecuteReader();

				while (data.Read())
				{
					
				}

			}

			return output;
		}


	}
}

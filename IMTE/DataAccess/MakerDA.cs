using IMTE.Models.IMTEEntity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
	public class MakerDA : DataAccessFunctions<Maker>
	{
		public IEnumerable<Maker> GetAllMaker()
		{
			var output = new List<Maker>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				connection.Open();
				command.Connection = connection;
				command.CommandText = @"SELECT * FROM ""IMTE"".""Maker""";

				var data = command.ExecuteReader();

				while (data.Read())
				{
					var json = SerializeDataToJSON(data);
					var jsonToObj = DeserializeJSONToObj(json);
					output.Add(jsonToObj);
				}
			}

			return output;
		}

	}
}

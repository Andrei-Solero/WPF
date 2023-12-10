using IMTE.Models.IMTEEntity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
	public class ResolutionDA : DataAccessFunctions<Resolution>
	{
		public IEnumerable<Resolution> GetAllResolutions() 
		{
			var output = new List<Resolution>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				connection.Open();
				command.Connection = connection;
				command.CommandText = @"SELECT * FROM ""IMTE"".""Resolution""";

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

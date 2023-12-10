using IMTE.Models.IMTEEntity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
	public class AcceptanceCriteriaDA : DataAccessFunctions<AcceptanceCriteria>
	{
		public IEnumerable<AcceptanceCriteria> GetAllAcceptanceCriteria()
		{
			var output = new List<AcceptanceCriteria>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				connection.Open();
				command.Connection = connection;
				command.CommandText = @"SELECT * FROM ""IMTE"".""AcceptanceCriteria""";

				var data = command.ExecuteReader();

				while (data.Read())
				{
					var jsonObj = SerializeDataToJSON(data);
					var jsonToObj = DeserializeJSONToObj(jsonObj);

					output.Add(jsonToObj);
				}
			}

			return output;
		}
	}
}

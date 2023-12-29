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
		public async Task<IEnumerable<AcceptanceCriteria>> GetAcceptanceCriteriasAsync()
		{
			var output = new List<AcceptanceCriteria>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				await connection.OpenAsync();
				command.Connection = connection;
				command.CommandText = @"SELECT * FROM ""IMTE"".""AcceptanceCriteria""";

				var data = await command.ExecuteReaderAsync();

				while (await data.ReadAsync())
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

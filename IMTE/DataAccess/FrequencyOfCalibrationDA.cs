using IMTE.Models.IMTEEntity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
	public class FrequencyOfCalibrationDA : DataAccessFunctions<FrequencyOfCalibration>
	{
		public async Task<IEnumerable<FrequencyOfCalibration>> GetFrequencyOfCalibrationsAsync()
		{
			var output = new List<FrequencyOfCalibration>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				await connection.OpenAsync();
				command.Connection = connection;
				command.CommandText = @"SELECT * FROM ""IMTE"".""FrequencyOfCalibration""";

				var data = await command.ExecuteReaderAsync();

				while (await data.ReadAsync())
				{
					var jsonData = SerializeDataToJSON(data);
					var jsonToObj = DeserializeJSONToObj(jsonData);
					output.Add(jsonToObj);
				}
			}

			return output;
		}
	}
}

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
		public IEnumerable<FrequencyOfCalibration> GetAllFrequencyOfCalibration()
		{
			var output = new List<FrequencyOfCalibration>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				connection.Open();
				command.Connection = connection;
				command.CommandText = @"SELECT * FROM ""IMTE"".""FrequencyOfCalibration""";

				var data = command.ExecuteReader();

				while (data.Read())
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

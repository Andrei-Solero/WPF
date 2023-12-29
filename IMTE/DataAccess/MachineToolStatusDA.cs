using IMTE.Models.Production;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class MachineToolStatusDA : DataAccessFunctions<MachineToolStatus>
    {
		public async Task<IEnumerable<MachineToolStatus>> GetAllMachineToolStatus()
		{
			var output = new List<MachineToolStatus>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"SELECT *
									FROM ""Production"".""MachineToolStatus""";


				await connection.OpenAsync();
				command.Connection = connection;
				command.CommandText = query;

				var data = await command.ExecuteReaderAsync();

				while (await data.ReadAsync())
				{
					output.Add(new MachineToolStatus
					{
						Id = CheckDbNullInt(data, "Id"),
						Status = CheckDbNullString(data, "Status"),
						IsAvailable = CheckDbNullBoolean(data, "IsAvailable")
					});
				}

			}

			return output;
		}
	}
}

using IMTE.Models.HumanResources;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class PositionDA : DataAccessFunctions<Position>
    {
        public async Task<IEnumerable<Position>> GetAllJobPosition()
        {
            var output = new List<Position>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection .OpenAsync();
                string query = @"SELECT * FROM ""HumanResources"".""Position""";

                command.Connection = connection;
                command.CommandText = query;

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    var jsonOutput = SerializeDataToJSON(data);
                    var empTypeObj = DeserializeJSONToObj(jsonOutput);

                    output.Add(empTypeObj);
                }
            }

            return output;
        }

    }
}

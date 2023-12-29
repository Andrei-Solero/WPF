using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class LocationDA : DataAccessFunctions<Location>
    {
        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            var output = new List<Location>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM ""General"".""Location""";

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    var jsonObj = SerializeDataToJSON(data);
                    var locObj = DeserializeJSONToObj(jsonObj);

                    output.Add(locObj);
                }
            }

            return output;
        }
    }
}

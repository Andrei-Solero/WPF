using IMTE.Models.IMTEEntity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class DeviceStatusDA : DataAccessFunctions<DeviceStatus>
    {
        public async Task<IEnumerable<DeviceStatus>> GetAllDeviceStatus()
        {
            var output = new List<DeviceStatus>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM ""IMTE"".""DeviceStatus""";

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    var jsonObj = SerializeDataToJSON(data);
                    var descriptionObj = DeserializeJSONToObj(jsonObj);

                    output.Add(descriptionObj);
                }
            }

            return output;
        }

    }
}

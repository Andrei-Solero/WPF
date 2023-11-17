using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class PlantDA : DataAccessFunctions<Plant>
    {
        public IEnumerable<Plant> GetAllPlant()
        {
            var output = new List<Plant>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM ""General"".""Plant""";

                var data = command.ExecuteReader();

                while (data.Read())
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

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
                command.CommandText = @"SELECT p.""Id"" AS ""PlantId"", p.""PlantName"", p.""Description"",
                                        l.""Id"" AS ""LocationId"", l.""Name"" AS ""LocationName"" 
                                        FROM ""General"".""Plant"" p
                                        INNER JOIN ""General"".""Location"" l ON p.""LocationId"" = l.""Id""";

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    output.Add(new Plant
                    {
                        Id = CheckDbNullInt(data, "PlantId"),
                        PlantName = CheckDbNullString(data, "PlantName"),
                        Description = CheckDbNullString(data, "Description"),
                        Location = new Location
                        {
                            Id = CheckDbNullInt(data, "PlantId"),
                            Name = CheckDbNullString(data, "LocationName")
                        }
                    });
                }
            }

            return output;
        }
    }
}

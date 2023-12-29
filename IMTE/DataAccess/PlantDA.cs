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
        /// <summary>
        /// Will return all the plant
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Plant>> GetPlant()
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

                var data =  await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(DataMapping(data));
                }
            }

            return output;
        }


        /// <summary>
        /// Will return the plant by plant ID
        /// </summary>
        /// <param name="plantObj"></param>
        /// <returns></returns>
        public async Task<Plant> GetPlant(Plant plantObj)
        {
            var output = new Plant();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT p.""Id"" AS ""PlantId"", p.""PlantName"", p.""Description"",
                                        l.""Id"" AS ""LocationId"", l.""Name"" AS ""LocationName"" 
                                        FROM ""General"".""Plant"" p
                                        INNER JOIN ""General"".""Location"" l ON p.""LocationId"" = l.""Id""
                                        WHERE p.""IsDeleted"" = false AND p.""Id"" = @Id";

                command.Parameters.AddWithValue("@Id", plantObj.Id);

                var data = await command.ExecuteReaderAsync();

                if (await data.ReadAsync())
                {
                    output = DataMapping(data);
                }
            }

            return output;
        }

        public async Task<Plant> CreatePlantAsync(Plant plantObj)
        {
            Plant output = plantObj;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    try
                    {
                        string query = @"INSERT INTO ""General"".""Plant""
                                        (""PlantCompanyId"", ""PlantName"", ""Description"", ""LocationId"")
                                    VALUES(1, @PlantName, @Description, @LocationId)
                                    RETURNING ""Id""";


                        command.Connection = connection;
                        command.CommandText = query;

                        command.Parameters.AddWithValue("@PlantName", plantObj.PlantName);
                        command.Parameters.AddWithValue("@Description", plantObj.Description);
                        command.Parameters.AddWithValue("@LocationId", plantObj.Location.Id);

                        output.Id = Convert.ToInt32(await command.ExecuteScalarAsync());

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return output;
        }

        private Plant DataMapping(NpgsqlDataReader data)
        {
            return new Plant
            {
                Id = CheckDbNullInt(data, "PlantId"),
                PlantName = CheckDbNullString(data, "PlantName"),
                Description = CheckDbNullString(data, "Description"),
                Location = new Location
                {
                    Id = CheckDbNullInt(data, "PlantId"),
                    Name = CheckDbNullString(data, "LocationName")
                }
            };
        }

    }
}

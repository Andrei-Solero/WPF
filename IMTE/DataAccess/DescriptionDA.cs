using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class DescriptionDA : DataAccessFunctions<Description>
    {
        public async Task<IEnumerable<Description>> GetDescriptionsAsync()
        {
            var output = new List<Description>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM ""General"".""Description""";

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


        public Description CreateDescription(Description descriptionObj)
        {
            Description output = descriptionObj;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    string query = @"INSERT INTO ""General"".""Description""(""Text"")
                                        VALUES(@Text)
                                        RETURNING ""Id""";

                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Text", descriptionObj.Text);

                    output.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return output;
        }

        public void UpdateDescription(Description descriptionObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                string query = @"UPDATE ""General"".""Description"" SET
                                    ""Text"" = @Text,
                                    ""Version"" = ""Version"" + 1,
                                    ""ModifiedOn"" = @CreatedOn
                                    WHERE ""Id"" = @Id";

                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = query;

                command.Parameters.AddWithValue("@Text", descriptionObj.Text);
                command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", descriptionObj.Id);

                command.ExecuteNonQuery();
            }

        }

    }
}

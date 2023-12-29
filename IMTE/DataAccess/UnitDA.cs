using IMTE.Models.Definition;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace IMTE.DataAccess
{
    public class UnitDA : DataAccessFunctions<Models.Definition.UnitEntity>
    {
        public async Task<IEnumerable<UnitEntity>> GetUnitsAsync()
        {
            var output = new List<Models.Definition.UnitEntity>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM ""Definition"".""Unit""
                                        WHERE ""IsDeleted"" = false";

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(new Models.Definition.UnitEntity
                    {
                        Id = CheckDbNullInt(data, "Id"),
                        UnitCategory = CheckDbNullString(data, "UnitCategory"),
                        UnitVal = CheckDbNullString(data, "UnitVal"),
                    });
                }

                return output;
            }
        }

        public UnitEntity CreateUnit(UnitEntity unitObj)
        {
            var output = new UnitEntity();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    try
                    {
                        string query = @"INSERT INTO ""Definition"".""Unit""(""UnitCategory"", ""UnitVal"")
                                    VALUES(@UnitCategory, @UnitVal)
                                    RETURNING ""Id""";


                        command.Connection = connection;
                        command.CommandText = query;

                        command.Parameters.AddWithValue("@UnitCategory", unitObj.UnitCategory);
                        command.Parameters.AddWithValue("@UnitVal", unitObj.UnitVal);

                        output.Id = Convert.ToInt32(command.ExecuteScalar());

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
    }
}
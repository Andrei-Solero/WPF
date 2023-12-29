using IMTE.Models.General;
using IMTE.Models.Inventory;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class EquipmentTypeDA : DataAccessFunctions<EquipmentType>
    {
        public async Task<IEnumerable<EquipmentType>> GetAllEquipmentType()
        {
            var output = new List<EquipmentType>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT ""et"".*, ""c"".""Id"" as CompanyId, ""c"".""CompanyName""
                                        FROM ""Inventory"".""EquipmentType"" et
                                        INNER JOIN ""General"".""Company"" c ON ""et"".""CompanyId"" = ""c"".""Id""";

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(new EquipmentType
                    {
                        Id = data.GetInt32(data.GetOrdinal("Id")),
                        Name = data.GetString(data.GetOrdinal("Name")),
                        Company = new Company
                        {
                            Id = data.GetInt32(data.GetOrdinal("CompanyId")),
                            CompanyName = data.GetString(data.GetOrdinal("CompanyName"))
                        }
                    });
                }
            }

            return output;
        }
    }
}

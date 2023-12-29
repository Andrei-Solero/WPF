using IMTE.Models.Production;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class MachineToolTypeDA : DataAccessFunctions<MachineToolType>
    {
        public async Task<IEnumerable<MachineToolType>> GetAllMachineToolType()
        {
            var output = new List<MachineToolType>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT *
                                        FROM ""Production"".""MachineToolType""";

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(new MachineToolType
                    {
                        Id = CheckDbNullInt(data, "Id"),
                        Version = CheckDbNullInt(data, "Version"),
                        Description = CheckDbNullString(data, "Description"),
                        ToolTypeName = CheckDbNullString(data, "ToolTypeName"),
                    });
                }
            }

            return output;
        }

    }
}

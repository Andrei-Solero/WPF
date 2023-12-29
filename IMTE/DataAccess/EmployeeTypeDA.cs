using IMTE.Models.HumanResources;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class EmployeeTypeDA : DataAccessFunctions<EmployeeType>
    {
        public async Task<IEnumerable<EmployeeType>> GetAllEmployeeTypes()
        {
            var output = new List<EmployeeType>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection .OpenAsync();
                string query = @"SELECT * FROM ""HumanResources"".""EmployeeType""";

                command.Connection = connection;
                command.CommandText = query;

                var data = await command .ExecuteReaderAsync();

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

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
        public IEnumerable<EmployeeType> GetAllEmployeeTypes()
        {
            var output = new List<EmployeeType>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                string query = @"SELECT * FROM ""HumanResources"".""EmployeeType""";

                command.Connection = connection;
                command.CommandText = query;

                var data = command.ExecuteReader();

                while (data.Read())
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

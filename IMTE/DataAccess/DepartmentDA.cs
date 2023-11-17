using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class DepartmentDA : DataAccessFunctions<Department>
    {
        public IEnumerable<Department> GetAllDepartments()
        {
            var output = new List<Department>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT ""dep"".*, ""com"".""CompanyName"" 
                                        FROM ""General"".""Department"" dep
                                        INNER JOIN ""General"".""Company"" com ON ""dep"".""CompanyId"" = ""com"".""Id""";

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    output.Add(new Department 
                    {
                        Id = data.GetInt32(data.GetOrdinal("Id")),
                        DepartmentName = data.GetString(data.GetOrdinal("DepartmentName")),
                        Company = new Company
                        {
                            CompanyName = data.GetString(data.GetOrdinal("CompanyName"))
                        }
                    });
                }
            }

            return output;
        }
    }
}

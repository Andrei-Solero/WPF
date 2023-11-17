using IMTE.Models.Definition;
using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class CompanyDA : DataAccessFunctions<Company>
    {
        public IEnumerable<Company> GetAllCompanies()
        {
            var output = new List<Company>();

            using (var connection = new NpgsqlConnection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT * FROM ""General"".""Company""";

                    var data = command.ExecuteReader();

                    while (data.Read())
                    {
                        output.Add(new Company
                        {

                        });
                    }
                }
            }

            return output;
        }
    }
}

using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class SpecificationDA : DataAccessFunctions<Specification>
    {
        public IEnumerable<Specification> GetAllSpecifications()
        {
            var output = new List<Specification>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT ""spec"".*, ""specCat"".""CompanyId"", ""specCat"".""Name"", ";

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    
                }
            }

            return output;
        }
    }
}

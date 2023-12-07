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
    public class InstrumentTypeDA : DataAccessFunctions<InstrumentType>
    {
        public IEnumerable<InstrumentType> GetAllInstrumentType()
        {
            var output = new List<InstrumentType>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                string query = @"SELECT 
                                    instType.""Id"" AS ""InstrumentTypeId"",
                                    c.""Id"" as ""CompanyId"", c.""CompanyName"",
                                    instType.""Name"" AS ""InstrumentTypeName""
                                    FROM ""Inventory"".""InstrumentType"" instType
                                    LEFT OUTER JOIN ""General"".""Company"" c ON instType.""CompanyId"" = c.""Id""";
                

                command.Connection = connection;
                command.CommandText = query;

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    output.Add(new InstrumentType
                    {
                        Id = CheckDbNullInt(data, "InstrumentTypeId"),
                        Company = CheckDbNullInt(data, "CompanyId") == 0 ? null : new Company
                        {
                            Id = CheckDbNullInt(data, "CompanyId"),
                            CompanyName = CheckDbNullString(data, "CompanyName")
                        },
                        Name = CheckDbNullString(data, "InstrumentTypeName")
                    });
                }
            }

            return output;
        }
    }
}

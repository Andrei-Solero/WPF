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
        public IEnumerable<Models.Definition.UnitEntity> GetAllUnit()
        {
            var output = new List<Models.Definition.UnitEntity>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                // TODO connection the 'location' here and ohter foreign keys needed
                // TODO After that bind that to the view
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM ""Definition"".""Unit""
                                        WHERE ""IsDeleted"" = false";

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    output.Add(new Models.Definition.UnitEntity
                    {
                        Id = data.GetInt32(data.GetOrdinal("Id")),
                        Version = data.GetInt32(data.GetOrdinal("Version")),
                        //ModifiedByEmployeeId = data.GetInt32(data.GetOrdinal("ModifiedByEmployeeId")),
                        UnitCategory = data.GetString(data.GetOrdinal("UnitCategory")),
                        UnitVal = data.GetString(data.GetOrdinal("UnitVal")),
                        Description = data.GetString(data.GetOrdinal("Description")),
                        //CreatedDate = data.GetDateTime(data.GetOrdinal("CreatedDate")),
                        IsDeleted = data.GetBoolean(data.GetOrdinal("IsDeleted")),
                        CreatedOn = data.GetDateTime(data.GetOrdinal("CreatedOn")),
                        ModifiedOn = data.GetDateTime(data.GetOrdinal("ModifiedOn")),
                    });
                }

                return output;
            }
        }
    }
}
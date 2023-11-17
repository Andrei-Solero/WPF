using IMTE.Models.Inventory;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class EquipmentDA : DataAccessFunctions<Equipment>
    {
        public void CreateEquipment(Equipment equipmentObj)
        {

        }

        public IEnumerable<Equipment> GetAllEquipment()
        {
            var output = new List<Equipment>();

            using(NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using(NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT ""et"".""Name"", 
                                        ""c"".""Id"", ""c"".""CompanyName"", ""c"".""CanLogin"",
";

            }

            return output;
        }

    }
}

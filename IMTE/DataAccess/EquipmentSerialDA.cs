using IMTE.Models.Inventory;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class EquipmentSerialDA : DataAccessFunctions<EquipmentSerial>
    {
        public void UdpateEquipmentSerial(EquipmentSerial equipmentSerialObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                string query = @"UPDATE ""Inventory"".""EquipmentSerial"" SET
									""Version"" = ""Version"" + 1,
                                    ""EquipmentId"" = @EquipmentId,
                                    ""SerialNo"" = @SerialNo,
                                    ""ModifiedOn"" = @ModifiedOn
                                    WHERE ""Id"" = @Id";

                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = query;

                command.Parameters.AddWithValue("@EquipmentId", equipmentSerialObj.Equipment.Id);
                command.Parameters.AddWithValue("@SerialNo", equipmentSerialObj.SerialNo);
                command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", equipmentSerialObj.Id);


                command.ExecuteNonQuery();
            }
        }
    }
}

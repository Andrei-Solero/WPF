using IMTE.Models.Production;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class MachineToolSerialDA : DataAccessFunctions<MachineToolSerial>
    {
        public void UpdateMachineToolSerial(MachineToolSerial machineToolSerialObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                string query = @"UPDATE ""Production"".""MachineToolSerial"" SET
									""Version"" = ""Version"" + 1,
                                    ""MachineToolId"" = @MachineToolId,
                                    ""SerialNo"" = @SerialNo,
                                    ""ToolUsageLifePcs"" = @ToolUsageLifePcs,
                                    ""Quantity"" = @Quantity,
                                    ""ModifiedOn"" = @ModifiedOn
                                    WHERE ""Id"" = @Id";

                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = query;

                command.Parameters.AddWithValue("@MachineToolId", machineToolSerialObj.MachineTool.Id);
                command.Parameters.AddWithValue("@SerialNo", machineToolSerialObj.SerialNo);
                command.Parameters.AddWithValue("@ToolUsageLifePcs", machineToolSerialObj.ToolLifeUsagePcs);
                command.Parameters.AddWithValue("@Quantity", machineToolSerialObj.Quantity);
                command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", machineToolSerialObj.Id);


                command.ExecuteNonQuery();
            }
        }
    }
}

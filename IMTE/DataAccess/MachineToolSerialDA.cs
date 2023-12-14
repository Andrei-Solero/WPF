using IMTE.Models.General;
using IMTE.Models.Inventory;
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
		public IEnumerable<MachineToolSerial> GetAllMachineToolSerial()
		{
			var output = new List<MachineToolSerial>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"
                                SELECT 
                                    mtSerial.""Id"" AS ""MachineToolSerialId"", mtSerial.""SerialNo"", mtSerial.""ToolUsageLifePcs"", mtSerial.""Quantity"",
                                    mt.""Id"" AS ""MachineToolId"", mt.""Description"" AS ""MachineToolDescription"", mt.""Note"", mt.""ToolName"", mt.""UnitCost"", mt.""ToolLifeUsagePcs"",
                                    mtType.""Id"" AS ""MachineToolTypeId"", mtType.""ToolTypeName"" AS ""MachineToolTypeName"", mtType.""Description"",
                                    it.""Id"" AS ""ItemId"", it.""ItemCode"", it.""ShortDescription"",
                                    des.""Id"" AS ""ItemDescriptionId"", des.""Text"" AS ""ItemDescriptiontext""
                                FROM ""Production"".""MachineToolSerial"" mtSerial
                                LEFT OUTER JOIN ""Production"".""MachineTool"" mt ON mtSerial.""MachineToolId"" = mt.""Id""
                                LEFT OUTER JOIN ""Production"".""MachineToolType"" mtType ON mt.""MachineToolTypeId"" = mtType.""Id""
                                LEFT OUTER JOIN ""General"".""Item"" it ON mt.""ItemId"" = it.""Id""
                                LEFT OUTER JOIN ""General"".""Department"" dept ON mt.""ItemId"" = dept.""Id""
								INNER JOIN ""General"".""Description"" des ON it.""DescriptionId"" = des.""Id"";";

				connection.Open();
				command.Connection = connection;
				command.CommandText = query;

				var data = command.ExecuteReader();

				while (data.Read())
				{
					output.Add(new MachineToolSerial
					{
						Id = CheckDbNullInt(data, "MachineToolSerialId"),
						SerialNo = CheckDbNullString(data, "SerialNo"),
						ToolLifeUsagePcs= CheckDbNullInt(data, "ToolUsageLifePcs"),
						Quantity = CheckDbNullInt(data, "Quantity"),
						MachineTool = CheckDbNullInt(data, "MachineToolId") == 0 ? null : new MachineTool
						{
							Id = CheckDbNullInt(data, "MachineToolId"),
							
							MachineToolType = CheckDbNullInt(data, "MachineToolTypeId") == 0 ? null : new MachineToolType
							{
								Id = CheckDbNullInt(data, "MachineToolTypeId"),
								ToolTypeName = CheckDbNullString(data, "MachineToolTypeName"),
							},
							Item = CheckDbNullInt(data, "ItemId") == 0 ? null : new Item
							{
								Id = CheckDbNullInt(data, "ItemId"),
								ItemCode = CheckDbNullString(data, "ItemCode"),
								ShortDescription = CheckDbNullString(data, "ShortDescription"),
								Description = CheckDbNullInt(data, "ItemDescriptionId") == 0 ? null : new Description
								{
									Id = CheckDbNullInt(data, "ItemDescriptionId"),
									Text = CheckDbNullString(data, "ItemDescriptiontext"),
								}
							}
						}
					});
				}

			}

			return output;
		}

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

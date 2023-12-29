using IMTE.Models.General;
using IMTE.Models.Production;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class MachineToolDA : DataAccessFunctions<MachineTool>
    {
        public async Task<IEnumerable<MachineTool>> GetAllMachineTool()
        {
            var output = new List<MachineTool>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                string query = @"
                                    SELECT 
                                        mt.""Id"" as ""MachineToolId"", mt.""Description"", mt.""Note"", mt.""ToolName"", 
                                        mt.""UnitCost"", mt.""ToolLifeUsagePcs"", 
                                        mtType.""Id"" as ""MachineToolTypeId"", mtType.""Description"" as ""MachineToolTypeDescription"",
                                        mtType.""ToolTypeName"" as ""MachineToolTypeName"",
                                        it.""Id"" as ""ItemId"", it.""ItemCode"", it.""ShortDescription"", 
                                        des.""Id"" as ""ItemDescriptionId"", des.""Text"" as ""ItemDescriptionText""
                                    FROM ""Production"".""MachineTool"" mt
                                    LEFT OUTER JOIN ""Production"".""MachineToolType"" mtType ON mt.""MachineToolTypeId"" = mtType.""Id""
                                    LEFT OUTER JOIN ""General"".""Item"" it ON mt.""ItemId"" = it.""Id""
                                    INNER JOIN ""General"".""Description"" des ON it.""DescriptionId"" = des.""Id"";";



                await connection .OpenAsync();
                command.Connection = connection;
                command.CommandText = query;

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(new MachineTool
                    {
                        Id = CheckDbNullInt(data, "MachineToolId"),
                        Description = CheckDbNullString(data, "Description"),
                        Note = CheckDbNullString(data, "Note"),
                        ToolName = CheckDbNullString(data, "ToolName"),
                        UnitCost = CheckDbNullDecimal(data, "UnitCost"),
                        ToolLifeUsagePcs = CheckDbNullInt(data, "ToolLifeUsagePcs"),
                        MachineToolType = new MachineToolType
                        {
                            Id = CheckDbNullInt(data, "MachineToolTypeId"),
                            Description = CheckDbNullString(data, "MachineToolTypeDescription"),
                            ToolTypeName = CheckDbNullString(data, "MachineToolTypeName"),
                        },
                        Item = new Item
                        {
                            Id = CheckDbNullInt(data, "ItemId"),
                            ItemCode = CheckDbNullString(data, "ItemCode"),
                            ShortDescription = CheckDbNullString(data, "ShortDescription"),
                            Description = new Description
                            {
                                Id = CheckDbNullInt(data, "ItemDescriptionId"),
                                Text = CheckDbNullString(data, "ItemDescriptionText")
                            }
                        }
                    });
                }
            }

            return output;
        }

        public void UpdateMachineTool(MachineTool machineToolObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"UPDATE ""Production"".""MachineTool"" SET
									""Version"" = ""Version"" + 1,
                                    ""ItemId"" = @ItemId,
                                    ""MachineToolTypeId"" = @MachineToolTypeId,
                                    ""Description"" = @Description,
                                    ""Note"" = @Note,
                                    ""ToolName"" = @ToolName,
                                    ""UnitCost"" = @UnitCost,
                                    ""ToolLifeUsagePcs"" = @ToolLifeUsagePcs,
                                    ""ModifiedOn"" = @ModifiedOn
                                    WHERE ""Id"" = @Id";

				command.Connection = connection;
				command.Transaction = transaction;
				command.CommandText = query;

				command.Parameters.AddWithValue("@ItemId", machineToolObj.Item.Id);
				command.Parameters.AddWithValue("@MachineToolTypeId", machineToolObj.MachineToolType.Id);
				command.Parameters.AddWithValue("@Description", machineToolObj.Description);
				command.Parameters.AddWithValue("@Note", machineToolObj.Note);
				command.Parameters.AddWithValue("@ToolName", machineToolObj.ToolName);
				command.Parameters.AddWithValue("@UnitCost", machineToolObj.UnitCost);
				command.Parameters.AddWithValue("@ToolLifeUsagePcs", machineToolObj.ToolLifeUsagePcs);
				command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", machineToolObj.Id);


				command.ExecuteNonQuery();
			}
		}

    }
}

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
        public IEnumerable<MachineTool> GetAllMachineTool()
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



                connection.Open();
                command.Connection = connection;
                command.CommandText = query;

                var data = command.ExecuteReader();

                while (data.Read())
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
    }
}

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
				string query = @"
                                    SELECT 
                                        eq.""Id"" as ""EquipmentId"", eq.""Manufacturer"", eq.""Model"", eq.""HasAccessory"", 
                                        eq.""ApprovalCode"", eq.""IsPrinted"", eq.""IsForeignCurrency"", eq.""IsSent"",
                                        eqType.""Id"" as ""EquipmentTypeId"", eqType.""Name"" as ""EquipmentTypeName"",
                                        it.""Id"" as ""ItemId"", it.""ItemCode"", it.""ShortDescription"",
                                        des.""Id"" as ""ItemDescriptionId"", des.""Text"" as ""ItemDescriptiontext""
                                    FROM ""Inventory"".""Equipment"" eq
                                    LEFT OUTER JOIN ""Inventory"".""EquipmentType"" eqType ON eq.""EquipmentTypeId"" = eqType.""Id""
                                    LEFT OUTER JOIN ""General"".""Item"" it ON eq.""ItemId"" = it.""Id""
                                    INNER JOIN ""General"".""Description"" des ON it.""DescriptionId"" = des.""Id""";


				connection.Open();
                command.Connection = connection;
                command.CommandText = query;

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    output.Add(new Equipment
                    {
                        Id = CheckDbNullInt(data, "EquipmentId"),
                        Manufacturer = CheckDbNullString(data, "Manufacturer"),
                        Model = CheckDbNullString(data, "Model"),
                        HasAccessory = CheckDbNullBoolean(data, "HasAccessory"),
                        ApprovalCode = CheckDbNullString(data, "ApprovalCode"),
                        IsPrinted = CheckDbNullBoolean(data, "IsPrinted"),
                        IsForeignCurrency = CheckDbNullBoolean(data, "IsForeignCurrency"),
                        IsSent = CheckDbNullBoolean(data, "IsSent"),
                        EquipmentType = new EquipmentType
                        {
                            Id = CheckDbNullInt(data, "EquipmentTypeId"),
                            Name = CheckDbNullString(data, "EquipmentTypeName"),
                        },
                        Item = new Item
                        {
                            Id = CheckDbNullInt(data, "ItemId"),
                            ItemCode = CheckDbNullString(data, "ItemCode"),
                            ShortDescription = CheckDbNullString(data, "ShortDescription"),
                            Description = new Description
                            {
								Id = CheckDbNullInt(data, "ItemDescriptionId"),
								Text = CheckDbNullString(data, "ItemDescriptiontext"),
							}
						}
                    }); ;
                }

            }

            return output;
        }

        public void UpdateEquipment(Equipment equipmentObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                string query = @"UPDATE ""Inventory"".""Equipment"" SET
									""Version"" = ""Version"" + 1,
                                    ""EquipmentTypeId"" = @EquipmentTypeId,
                                    ""ItemId"" = @ItemId,
                                    ""Model"" = @Model,
                                    ""HasAccessory"" = @HasAccessory,
                                    ""ApprovalCode"" = @ApprovalCode,
                                    ""IsPrinted"" = @IsPrinted,
                                    ""IsSent"" = @IsSent,
                                    ""IsForeignCurrency"" = @IsForeignCurrency,
                                    ""ModifiedOn"" = @ModifiedOn
                                    WHERE ""Id"" = @Id";

                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = query;

                command.Parameters.AddWithValue("@EquipmentTypeId", equipmentObj.EquipmentType.Id);
                command.Parameters.AddWithValue("@ItemId", equipmentObj.Item.Id);
                command.Parameters.AddWithValue("@Model", equipmentObj.Model);
                command.Parameters.AddWithValue("@HasAccessory", equipmentObj.HasAccessory);
                command.Parameters.AddWithValue("@ApprovalCode", equipmentObj.ApprovalCode);
                command.Parameters.AddWithValue("@IsPrinted", equipmentObj.IsPrinted);
                command.Parameters.AddWithValue("@IsSent", equipmentObj.IsSent);
                command.Parameters.AddWithValue("@IsForeignCurrency", equipmentObj.IsForeignCurrency);
                command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", equipmentObj.Id);

                command.ExecuteNonQuery();
            }
        }

    }
}

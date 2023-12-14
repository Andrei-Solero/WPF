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
    public class EquipmentSerialDA : DataAccessFunctions<EquipmentSerial>
    {
        public IEnumerable<EquipmentSerial> GetAllEquipmentSerial()
        {
			var output = new List<EquipmentSerial>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"SELECT 
                                eqSerial.""Id"" AS ""EquipmentSerialId"", eqSerial.""SerialNo"",
                                eq.""Id"" AS ""EquipmentId"", eq.""Manufacturer"", eq.""Model"", eq.""HasAccessory"", 
                                eq.""ApprovalCode"", eq.""IsPrinted"", eq.""IsForeignCurrency"", eq.""IsSent"",
                                eqType.""Id"" AS ""EquipmentTypeId"", eqType.""Name"" AS ""EquipmentTypeName"",
                                it.""Id"" AS ""ItemId"", it.""ItemCode"", it.""ShortDescription"",
                                des.""Id"" AS ""ItemDescriptionId"", des.""Text"" AS ""ItemDescriptiontext""
                            FROM ""Inventory"".""EquipmentSerial"" eqSerial
                            LEFT OUTER JOIN ""Inventory"".""Equipment"" eq ON eqSerial.""EquipmentId"" = eq.""Id""
                            LEFT OUTER JOIN ""Inventory"".""EquipmentType"" eqType ON eq.""EquipmentTypeId"" = eqType.""Id""
                            LEFT OUTER JOIN ""General"".""Item"" it ON eq.""ItemId"" = it.""Id""
                            LEFT OUTER JOIN ""General"".""Department"" dept ON eq.""ItemId"" = dept.""Id""
                            INNER JOIN ""General"".""Description"" des ON it.""DescriptionId"" = des.""Id"";
                            ";

				connection.Open();
				command.Connection = connection;
				command.CommandText = query;

				var data = command.ExecuteReader();

				while (data.Read())
				{
					output.Add(new EquipmentSerial
					{
						Id = CheckDbNullInt(data, "EquipmentSerialId"),
						SerialNo = CheckDbNullString(data, "SerialNo"),
						Equipment = CheckDbNullInt(data, "EquipmentId") == 0 ? null : new Equipment
						{
							Id = CheckDbNullInt(data, "EquipmentId"),
							Manufacturer = CheckDbNullString(data, "Manufacturer"),
							Model = CheckDbNullString(data, "Model"),
							HasAccessory = CheckDbNullBoolean(data, "HasAccessory"),
							ApprovalCode = CheckDbNullString(data, "ApprovalCode"),
							IsPrinted = CheckDbNullBoolean(data, "IsPrinted"),
							IsForeignCurrency = CheckDbNullBoolean(data, "IsForeignCurrency"),
							IsSent = CheckDbNullBoolean(data, "IsSent"),
							EquipmentType = CheckDbNullInt(data, "EquipmentTypeId") == 0 ? null : new EquipmentType
							{
								Id = CheckDbNullInt(data, "EquipmentTypeId"),
								Name = CheckDbNullString(data, "EquipmentTypeName"),
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

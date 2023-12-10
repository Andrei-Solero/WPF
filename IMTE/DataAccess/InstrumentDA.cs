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
    public class InstrumentDA : DataAccessFunctions<Instrument>
    {
        public IEnumerable<Instrument> GetAllInstruments()
        {
            var output = new List<Instrument>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                string query = @"
                                    SELECT 
                                        instrument.""Id"" as ""InstrumentId"", instrument.""Manufacturer"", instrument.""Model"", instrument.""HasAccessory"", 
                                        instrument.""ApprovalCode"", instrument.""IsPrinted"", instrument.""IsForeignCurrency"", instrument.""IsSent"",
                                        instType.""Id"" as ""InstrumentTypeId"", instType.""Name"" as ""InstrumentTypeName"",
                                        dept.""Id"" as ""DepartmentId"", dept.""DepartmentName"", dept.""Description"" as ""DepartmentDescription"",
                                        it.""Id"" as ""ItemId"", it.""ItemCode"", it.""ShortDescription"",
                                        des.""Id"" as ""ItemDescriptionId"", des.""Text"" as ""ItemDescriptiontext""
                                    FROM ""Inventory"".""Instrument"" instrument
                                    LEFT OUTER JOIN ""Inventory"".""InstrumentType"" instType ON instrument.""InstrumentTypeId"" = instType.""Id""
                                    LEFT OUTER JOIN ""General"".""Item"" it ON instrument.""ItemId"" = it.""Id""
                                    LEFT OUTER JOIN ""General"".""Department"" dept ON instrument.""ItemId"" = dept.""Id""
                                    INNER JOIN ""General"".""Description"" des ON it.""DescriptionId"" = des.""Id""";


                command.Connection = connection;
                command.CommandText = query;

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    output.Add(new Instrument
                    {
                        Id = CheckDbNullInt(data, "InstrumentId"),
                        Manufacturer = CheckDbNullString(data, "Manufacturer"),
                        Model = CheckDbNullString(data, "Model"),
                        HasAccessory = CheckDbNullBoolean(data, "HasAccessory"),
                        ApprovalCode = CheckDbNullString(data, "ApprovalCode"),
                        IsPrinted = CheckDbNullBoolean(data, "IsPrinted"),
                        IsForeignCurrency = CheckDbNullBoolean(data, "IsForeignCurrency"),
                        IsSent = CheckDbNullBoolean(data, "IsSent"),
                        InstrumentType = CheckDbNullInt(data, "InstrumentTypeId") == 0 ? null : new InstrumentType
                        {
                            Id = CheckDbNullInt(data, "InstrumentTypeId"),
                            Name = CheckDbNullString(data, "InstrumentTypeName"),
                        },
                        Department = CheckDbNullInt(data, "DepartmentId") == 0 ? null : new Department
                        {
                            Id = CheckDbNullInt(data, "DepartmentId"),
                            DepartmentName = CheckDbNullString(data, "DepartmentName"),
                            Description = CheckDbNullString(data, "DepartmentDescription"),
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
                    });
                }
            }

            return output;
        }

        public void UpdateInstrument(Instrument instrumentObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"UPDATE ""Inventory"".""Instrument"" SET
									""Version"" = ""Version"" + 1,
                                    ""InstrumentTypeId"" = @InstrumentTypeId,
                                    ""ItemId"" = @ItemId,
                                    ""DepartmentId"" = @DepartmentId,
                                    ""Manufacturer"" = @Manufacturer,
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

				command.Parameters.AddWithValue("@InstrumentTypeId", instrumentObj.InstrumentType.Id);
				command.Parameters.AddWithValue("@ItemId", instrumentObj.Item.Id);
				command.Parameters.AddWithValue("@DepartmentId", instrumentObj.Department.Id);
				command.Parameters.AddWithValue("@Manufacturer", instrumentObj.Manufacturer);
				command.Parameters.AddWithValue("@Model", instrumentObj.Model);
				command.Parameters.AddWithValue("@HasAccessory", instrumentObj.HasAccessory);
				command.Parameters.AddWithValue("@ApprovalCode", instrumentObj.ApprovalCode);
				command.Parameters.AddWithValue("@IsPrinted", instrumentObj.IsPrinted);
				command.Parameters.AddWithValue("@IsSent", instrumentObj.IsSent);
				command.Parameters.AddWithValue("@IsForeignCurrency", instrumentObj.IsForeignCurrency);
				command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
				command.Parameters.AddWithValue("@Id", instrumentObj.Id);

				command.ExecuteNonQuery();
			}
		}

    }
}

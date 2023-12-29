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
    public class InstrumentSerialDA : DataAccessFunctions<InstrumentSerial>
    {
        public async  Task<IEnumerable<InstrumentSerial>> GetAllInstrumentSerial()
        {
            var output = new List<InstrumentSerial>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                string query = @"
                                SELECT 
                                    instrumentSerial.""Id"" AS ""InstrumentSerialId"", instrumentSerial.""SerialNo"",
                                    instrument.""Id"" AS ""InstrumentId"", instrument.""Manufacturer"", instrument.""Model"", instrument.""HasAccessory"", 
                                    instrument.""ApprovalCode"", instrument.""IsPrinted"", instrument.""IsForeignCurrency"", instrument.""IsSent"",
                                    instType.""Id"" AS ""InstrumentTypeId"", instType.""Name"" AS ""InstrumentTypeName"",
                                    dept.""Id"" AS ""DepartmentId"", dept.""DepartmentName"", dept.""Description"" AS ""DepartmentDescription"",
                                    it.""Id"" AS ""ItemId"", it.""ItemCode"", it.""ShortDescription"",
                                    des.""Id"" AS ""ItemDescriptionId"", des.""Text"" AS ""ItemDescriptiontext""
                                FROM ""Inventory"".""InstrumentSerial"" instrumentSerial
                                LEFT OUTER JOIN ""Inventory"".""Instrument"" instrument ON instrumentSerial.""InstrumentId"" = instrument.""Id""
                                LEFT OUTER JOIN ""Inventory"".""InstrumentType"" instType ON instrument.""InstrumentTypeId"" = instType.""Id""
                                LEFT OUTER JOIN ""General"".""Item"" it ON instrument.""ItemId"" = it.""Id""
                                LEFT OUTER JOIN ""General"".""Department"" dept ON instrument.""ItemId"" = dept.""Id""
                                INNER JOIN ""General"".""Description"" des ON it.""DescriptionId"" = des.""Id"";";



                await connection .OpenAsync();
                command.Connection = connection;
                command.CommandText = query;

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(new InstrumentSerial
                    {
                        Id = CheckDbNullInt(data, "InstrumentSerialId"),
                        SerialNo = CheckDbNullString(data, "SerialNo"),
                        Instrument = CheckDbNullInt(data, "InstrumentId") == 0 ? null : new Instrument
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
                        }
                    });
                }

            }

            return output;
        }

        public void UpdateInstrumentSerial(InstrumentSerial instrumentSerialObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"UPDATE ""Inventory"".""InstrumentSerial"" SET
									""Version"" = ""Version"" + 1,
                                    ""InstrumentId"" = @InstrumentId,
                                    ""SerialNo"" = @SerialNo,
                                    ""ModifiedOn"" = @ModifiedOn
                                    WHERE ""Id"" = @Id";

				command.Connection = connection;
				command.Transaction = transaction;
				command.CommandText = query;

				command.Parameters.AddWithValue("@InstrumentId", instrumentSerialObj.Instrument.Id);
				command.Parameters.AddWithValue("@SerialNo", instrumentSerialObj.SerialNo);
                command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", instrumentSerialObj.Id);


				command.ExecuteNonQuery();
			}
		}
    }
}

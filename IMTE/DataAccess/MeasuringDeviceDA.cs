using IMTE.General.Models;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.Inventory;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.DataAccess
{
    public class MeasuringDeviceDA : DataAccessFunctions<MeasuringDevice>
    {
        public IEnumerable<MeasuringDevice> GetAllMeasuringDevices()
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = null;

            try
            {
                var output = new List<MeasuringDevice>();

                using (connection = new NpgsqlConnection(ConnectionString))
                using (command = new NpgsqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    string query = @"
                                SELECT 
                                    md.""Id"" as ""MDId"", 
                                    eq.""Id"" as ""EqId"", eq.""Manufacturer"", eq.""Model"", eq.""HasAccessory"", 
                                    eq.""ApprovalCode"", eq.""IsPrinted"", eq.""IsSent"", eq.""IsForeignCurrency"",
                                    eqType.""Id"" as ""EquipmentTypeId"", eqType.""Name"" as ""EquipmentName"",
                                    it.""Id"" as ""ItemId"", it.""ItemCode"", it.""ShortDescription"", 
                                    des.""Id"" as ""DesId"", des.""Text"" as ""DescriptionText"",
                                    pl.""Id"" as ""PlId"", pl.""PlantName"",
                                    empIssuedTo.""Id"" as ""IssuedToEmployeeId"", 
                                    empIssuedToPerson.""Id"" as ""IssuedToEmployeePersonId"", empIssuedToPerson.""First"" as ""IssuedToEmpFirst"", 
                                    empIssuedToPerson.""Last"" as ""IssuedToEmpLast"", empIssuedToPerson.""Middle"" as ""IssuedToEmpMiddle"",
                                    empIssuedToPosition.""Id"" as ""IssuedToEmployeePositionId"", 
                                    empIssuedToPosition.""PositionName"" as ""IssuedToEmployeePosition"", 
                                    empIssuedToPosition.""DutiesDescription"" as ""IssuedToEmployeeDuties"",
        
                                    empCalibratedBy.""Id"" as ""CalibratedByEmployeeId"",
                                    empCalibratedByPerson.""Id"" as ""CalibratedToEmployeePersonId"", 
                                    empCalibratedByPerson.""First"" as ""CalibratedByEmployeeFirst"", 
                                    empCalibratedByPerson.""Last"" as ""CalibratedByEmployeeLast"", empCalibratedByPerson.""Middle"" as ""CalibratedByEmployeeMiddle"",
                                    empCalibratedByPosition.""Id"" as ""CalibratedByEmployeePositionId"", 
                                    empCalibratedByPosition.""PositionName"" as ""CalibratedByEmployeePosition"", 
                                    empCalibratedByPosition.""DutiesDescription"" as ""CalibratedByEmployeeDuties"",
        
                                    dept.""Id"" as ""DepartmentId"", dept.""DepartmentName"",
                                    loc.""Id"" as ""LocationId"", loc.""Name"" as ""LocationName"",
                                    md.""Version"" as ""MDVersion"", md.""FrequencyOfCalibration"", md.""LastCalibrationDate"",
                                    md.""ResultOfCalibration"", md.""NextCalibrationDate"", md.""Status"", md.""ThreadGaugeRIngGaugeUsageNo"",
                                    md.""CalibrationRemarks"", md.""Remarks"", md.""Date"", md.""Maker"",
                                    md.""Resolution"", md.""DeviceRange"", md.""Accuracy"", md.""CalibrationCertificate"", md.""Barcode"", 
                                    md.""CalibrationMethod"", md.""AcceptanceCriteria"",  md.""SerialNo"",
                                    unit.""Id"" as ""UnitId"", unit.""UnitCategory"", unit.""UnitVal"", unit.""Description""
                                FROM ""IMTE"".""MeasuringDevices"" md
                                LEFT OUTER JOIN ""Inventory"".""Equipment"" eq ON md.""EquipmentId"" = eq.""Id""
                                LEFT OUTER JOIN ""Inventory"".""EquipmentType"" eqType ON eq.""EquipmentTypeId"" = eqType.""Id""
                                LEFT OUTER JOIN ""General"".""Item"" it ON eq.""ItemId"" = it.""Id""
                                LEFT OUTER JOIN ""General"".""Description"" des ON it.""DescriptionId"" = des.""Id""
                                LEFT OUTER JOIN ""General"".""Plant"" pl ON md.""PlantId"" = pl.""Id""
    
                                LEFT OUTER JOIN ""HumanResources"".""Employee"" empIssuedTo ON md.""IssuedToEmployeeId"" = empIssuedTo.""Id""
                                LEFT OUTER JOIN ""HumanResources"".""Position"" empIssuedToPosition ON empIssuedTo.""PositionId"" = empIssuedToPosition.""Id""
                                LEFT OUTER JOIN ""General"".""Person"" empIssuedToPerson ON empIssuedTo.""PersonId"" = empIssuedToPerson.""Id""
    
                                LEFT OUTER JOIN ""HumanResources"".""Employee"" empCalibratedBy ON md.""CalibratedByEmployeeId"" = empCalibratedBy.""Id""
                                LEFT OUTER JOIN ""HumanResources"".""Position"" empCalibratedByPosition ON empCalibratedBy.""PositionId"" = empCalibratedByPosition.""Id""
                                LEFT OUTER JOIN ""General"".""Person"" empCalibratedByPerson ON empCalibratedBy.""PersonId"" = empCalibratedByPerson.""Id""
    
                                LEFT OUTER JOIN ""General"".""Department"" dept ON md.""DepartmentId"" = dept.""Id""
                                LEFT OUTER JOIN ""General"".""Location"" loc ON md.""LocationId"" = loc.""Id""
                                LEFT OUTER JOIN ""Definition"".""Unit"" unit ON md.""UnitId"" = unit.""Id""
                                WHERE md.""IsDeleted"" = false;";

                    command.CommandText = query;

                    var data = command.ExecuteReader();

                    while (data.Read())
                    {
                        output.Add(new MeasuringDevice
                        {
                            Id = CheckDbNullInt(data, "MDId"),
                            Equipment = new Equipment
                            {
                                Id = CheckDbNullInt(data, "EqId"),
                                Manufacturer = CheckDbNullString(data, "Manufacturer"),
                                Model = CheckDbNullString(data, "Model"),
                                HasAccessory = data.GetBoolean(data.GetOrdinal("HasAccessory")),
                                ApprovalCode = CheckDbNullString(data, "ApprovalCode"),
                                IsPrinted = data.GetBoolean(data.GetOrdinal("IsPrinted")),
                                IsSent = data.GetBoolean(data.GetOrdinal("IsSent")),
                                IsForeignCurrency = data.GetBoolean(data.GetOrdinal("IsForeignCurrency")),
                                EquipmentTypeObj = new EquipmentType
                                {
                                    Id = CheckDbNullInt(data, "EquipmentTypeId"),
                                    Name = CheckDbNullString(data, "EquipmentName")
                                },
                                Item = new Item
                                {
                                    Id = CheckDbNullInt(data, "ItemId"),
                                    ItemCode = CheckDbNullString(data, "ItemCode"),
                                    ShortDescription = CheckDbNullString(data, "ShortDescription"),
                                    Description = new Description
                                    {
                                        Id = CheckDbNullInt(data, "DesId"),
                                        Text = CheckDbNullString(data, "DescriptionText"),
                                    }
                                },
                            },
                            Plant = new Plant
                            {
                                Id = CheckDbNullInt(data, "PlId"),
                                PlantName = CheckDbNullString(data, "PlantName"),
                            },
                            IssuedToEmployee = new Employee
                            {
                                Id = CheckDbNullInt(data, "IssuedToEmployeeId"),
                                Person = new Person
                                {
                                    Id = CheckDbNullInt(data, "IssuedToEmployeePersonId"),
                                    First = CheckDbNullString(data, "IssuedToEmpFirst"),
                                    Last = CheckDbNullString(data, "IssuedToEmpLast"),
                                    Middle = CheckDbNullString(data, "IssuedToEmpMiddle"),
                                },
                                Position = new Position
                                {
                                    Id = CheckDbNullInt(data, "IssuedToEmployeePositionId"),
                                    PositionName = CheckDbNullString(data, "IssuedToEmployeePosition"),
                                    DutiesDescription = CheckDbNullString(data, "IssuedToEmployeeDuties"),
                                }
                            },
                            CalibratedByEmployee = new Employee
                            {
                                Id = CheckDbNullInt(data, "CalibratedByEmployeeId"),
                                Person = new Person
                                {
                                    Id = CheckDbNullInt(data, "CalibratedToEmployeePersonId"),
                                    First = CheckDbNullString(data, "CalibratedByEmployeeFirst"),
                                    Last = CheckDbNullString(data, "CalibratedByEmployeeLast"),
                                    Middle = CheckDbNullString(data, "CalibratedByEmployeeMiddle"),
                                },
                                Position = new Position
                                {
                                    Id = CheckDbNullInt(data, "CalibratedByEmployeePositionId"),
                                    PositionName = CheckDbNullString(data, "CalibratedByEmployeePosition"),
                                    DutiesDescription = CheckDbNullString(data, "CalibratedByEmployeeDuties"),
                                }
                            },
                            Department = new Department
                            {
                                Id = CheckDbNullInt(data, "DepartmentId"),
                                DepartmentName = CheckDbNullString(data, "DepartmentName")
                            },
                            Location = new Location
                            {
                                Id = CheckDbNullInt(data, "LocationId"),
                                Name = CheckDbNullString(data, "LocationName")
                            },
                            Version = CheckDbNullInt(data, "MDVersion"),
                            FrequencyOfCalibration = CheckDbNullString(data, "FrequencyOfCalibration"),
                            LastCalibrationDate = data.GetDateTime(data.GetOrdinal("LastCalibrationDate")),
                            ResultOfCalibration = CheckDbNullString(data, "ResultOfCalibration"),
                            NextCalibrationDate = data.GetDateTime(data.GetOrdinal("NextCalibrationDate")),
                            Status = CheckDbNullString(data, "Status"),
                            ThreadGaugeRingGaugeUsageNo = CheckDbNullInt(data, "ThreadGaugeRIngGaugeUsageNo"),
                            CalibrationRemarks = CheckDbNullString(data, "CalibrationRemarks"),
                            Remarks = CheckDbNullString(data, "Remarks"),
                            Date = CheckDbNullString(data, "Date"),
                            Maker = CheckDbNullString(data, "Maker"),
                            Resolution = CheckDbNullString(data, "Resolution"),
                            DeviceRange = CheckDbNullString(data, "DeviceRange"),
                            Accuracy = CheckDbNullString(data, "Accuracy"),
                            Barcode = CheckDbNullString(data, "Barcode"),
                            CalibrationMethod = CheckDbNullString(data, "CalibrationMethod"),
                            AcceptanceCriteria = CheckDbNullString(data, "AcceptanceCriteria"),
                            SerialNo = CheckDbNullString(data, "SerialNo"),
                            Unit = new UnitEntity
                            {
                                Id = CheckDbNullInt(data, "UnitId"),
                                UnitCategory = CheckDbNullString(data, "UnitCategory"),
                                UnitVal = CheckDbNullString(data, "UnitVal"),
                                Description = CheckDbNullString(data, "Description"),
                            }
                        });
                    }
                }

                return output;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
                connection.Dispose();

                command.Dispose();
            }
        }

        public Description CreateDescriptionForItemMeasuringDevice(Description descriptionObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            Description description = descriptionObj;

            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                try
                {
                    string query = @"INSERT INTO ""General"".""Description"" (""Text"", ""CreatedDate"", ""CreatedOn"")
                                    VALUES(@Text, @CreatedDate, @CreatedOn)
                                    RETURNING ""Id""";

                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Text", descriptionObj.Text);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    description.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType()}: {ex.Message}");
                }
            }


            return description;
        }

        public Item CreateItemEquipmentForMeasuringDevice(Item itemObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            Item item = itemObj;

            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                try
                {
                    string query = @"INSERT INTO ""General"".""Item"" (""StockingUnitId"", ""CurrencyId"", ""CompanyId"", ""DescriptionId"", ""ItemCode"", ""ShortDescription"", ""Inactive"", ""IsSold"", ""IsBought"", ""IsMfgComponent"", ""IsMfgConsumable"", ""IsObsolete""
                            )
                                    VALUES(1, 2, 1, @DescriptionId, @ItemCode, @ShortDescription, false, false, false, false, false, false)
                                    RETURNING ""Id""";

                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@DescriptionId", itemObj.Description.Id);
                    command.Parameters.AddWithValue("@ItemCode", itemObj.ItemCode);
                    command.Parameters.AddWithValue("@ShortDescription", itemObj.ShortDescription);

                    item.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType()}: {ex.Message}");
                }
            }


            return item;
        }

        public Equipment CreateEquipmentForMeasuringDevice(Equipment equipmentObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            Equipment equipment = equipmentObj;

            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                try
                {
                    string query = @"INSERT INTO ""Inventory"".""Equipment"" (""CompanyId"", ""ItemId"", ""Manufacturer"", ""Model"",
                                    ""HasAccessory"", ""ApprovalCode"", ""IsPrinted"", ""IsSent"", ""IsForeignCurrency"",
                                    ""EquipmentTypeId"")
                                    VALUES(2, @ItemId, @Manufacturer, @Model, @HasAccessory, @ApprovalCode, @IsPrinted, @IsSent, @IsForeignCurrency,
                                    @EquipmentTypeId)
                                    RETURNING ""Id""";

                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@ItemId", equipmentObj.Item.Id);
                    command.Parameters.AddWithValue("@Manufacturer", equipmentObj.Manufacturer);
                    command.Parameters.AddWithValue("@Model", equipmentObj.Model);
                    command.Parameters.AddWithValue("@HasAccessory", equipmentObj.HasAccessory);
                    command.Parameters.AddWithValue("@ApprovalCode", equipmentObj.ApprovalCode);
                    command.Parameters.AddWithValue("@IsPrinted", equipmentObj.IsPrinted);
                    command.Parameters.AddWithValue("@IsSent", equipmentObj.IsSent);
                    command.Parameters.AddWithValue("@IsForeignCurrency", equipmentObj.IsForeignCurrency);
                    command.Parameters.AddWithValue("@EquipmentTypeId", equipmentObj.EquipmentTypeObj.Id);

                    equipment.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType()}: {ex.Message}");
                }

            }


            return equipment;
        }

        public MeasuringDevice CreateMeasuringDevice(MeasuringDevice measuringDeviceObj)
        {
            MeasuringDevice measuringDevice = measuringDeviceObj;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    try
                    {
                        Description description = CreateDescriptionForItemMeasuringDevice(measuringDeviceObj.Equipment.Item.Description, transaction, connection);
                        Item item = CreateItemEquipmentForMeasuringDevice(measuringDeviceObj.Equipment.Item, transaction, connection);
                        Equipment equipment = CreateEquipmentForMeasuringDevice(measuringDeviceObj.Equipment, transaction, connection);

                        string query = @"INSERT INTO ""IMTE"".""MeasuringDevices"" (""Version"", ""EquipmentId"", ""PlantId"", ""IssuedToEmployeeId"", ""CalibratedByEmployeeId"", ""DepartmentId"",
                                    ""LocationId"", ""FrequencyOfCalibration"", ""LastCalibrationDate"", ""ResultOfCalibration"", ""NextCalibrationDate"",
                                    ""Status"", ""ThreadGaugeRIngGaugeUsageNo"", ""CalibrationRemarks"", ""Remarks"", ""Date"", ""Maker"", ""Resolution"",
                                    ""DeviceRange"", ""Accuracy"", ""Barcode"", ""CalibrationMethod"", ""AcceptanceCriteria"", ""SerialNo"", ""UnitId"") VALUES
                                    (@Version, @EquipmentId, @PlantId, @IssuedToEmployeeId, @CalibratedByEmployeeId, @DepartmentId, @LocationId, @FrequencyOfCalibration, @LastCalibrationDate,
                                    @ResultOfCalibration, @NextCalibrationDate, @Status, @ThreadGaugeRIngGaugeUsageNo, @CalibrationRemarks, @Remarks, @Date,
                                    @Maker, @Resolution, @DeviceRange, @Accuracy, @Barcode, @CalibrationMethod, @AcceptanceCriteria, @SerialNo, @UnitId)";

                        command.Connection = connection;
                        command.CommandText = query;

                        command.Parameters.AddWithValue("@Version", 1);
                        command.Parameters.AddWithValue("@EquipmentId", equipment.Id);
                        command.Parameters.AddWithValue("@PlantId", measuringDeviceObj.Plant.Id);
                        command.Parameters.AddWithValue("@IssuedToEmployeeId", measuringDeviceObj.IssuedToEmployee.Id);
                        command.Parameters.AddWithValue("@CalibratedByEmployeeId", measuringDeviceObj.CalibratedByEmployee.Id);
                        command.Parameters.AddWithValue("@DepartmentId", measuringDeviceObj.Department.Id);
                        command.Parameters.AddWithValue("@LocationId", measuringDeviceObj.Location.Id);
                        command.Parameters.AddWithValue("@FrequencyOfCalibration", measuringDeviceObj.FrequencyOfCalibration);
                        command.Parameters.AddWithValue("@LastCalibrationDate", measuringDeviceObj.LastCalibrationDate);
                        command.Parameters.AddWithValue("@ResultOfCalibration", measuringDeviceObj.ResultOfCalibration);
                        command.Parameters.AddWithValue("@NextCalibrationDate", measuringDeviceObj.NextCalibrationDate);
                        command.Parameters.AddWithValue("@Status", measuringDeviceObj.Status);
                        command.Parameters.AddWithValue("@ThreadGaugeRIngGaugeUsageNo", measuringDeviceObj.ThreadGaugeRingGaugeUsageNo);
                        command.Parameters.AddWithValue("@CalibrationRemarks", measuringDeviceObj.CalibrationRemarks);
                        command.Parameters.AddWithValue("@Remarks", measuringDeviceObj.Remarks);
                        command.Parameters.AddWithValue("@Date", measuringDeviceObj.Date);
                        command.Parameters.AddWithValue("@Maker", measuringDeviceObj.Maker);
                        command.Parameters.AddWithValue("@Resolution", measuringDeviceObj.Resolution);
                        command.Parameters.AddWithValue("@DeviceRange", measuringDeviceObj.DeviceRange);
                        command.Parameters.AddWithValue("@Accuracy", measuringDeviceObj.Accuracy);
                        command.Parameters.AddWithValue("@Barcode", measuringDeviceObj.Barcode);
                        command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceObj.CalibrationMethod);
                        command.Parameters.AddWithValue("@AcceptanceCriteria", measuringDeviceObj.AcceptanceCriteria);
                        command.Parameters.AddWithValue("@SerialNo", measuringDeviceObj.SerialNo);
                        command.Parameters.AddWithValue("@UnitId", measuringDeviceObj.Unit.Id);

                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"{ex.GetType()}: {ex.Message}");
                    }
                }
            }


            return measuringDevice;
        }

        public void UpdateMeasuringDevice(MeasuringDevice measuringDeviceObj)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                string sql = @" UPDATE ""IMTE"".""MeasuringDevices"" SET
                            ""Version"" = @Version,
                            ""Type"" = @Type,
                            ""FrequencyOfCalibration"" = @FrequencyOfCalibration,
                            ""LastCalibrationDate"" = @LastCalibrationDate,
                            ""ResultOfCalibration"" = @ResultOfCalibration,
                            ""NextCalibrationDate"" = @NextCalibrationDate,
                            ""Status"" = @Status,
                            ""CalibrationRemarks"" = @CalibrationRemarks,
                            ""TrgTpgAndSettingRemarks"" = @TrgTpgAndSettingsRemark,
                            ""Remarks"" = @Remarks,
                            ""Date"" = @Date,
                            ""Maker"" = @Maker,
                            ""Resolution"" = @Resolution,
                            ""DeviceRange"" = @DeviceRange,
                            ""Accuracy"" = @Accuracy,
                            ""UnitOfMeasurement"" = @UnitOfMeasurement,
                            ""Barcode"" = @Barcode,
                            ""CalibrationMethod"" = @CalibrationMethod
                            WHERE ""Id"" = @Id";

                command.CommandText = sql;

                command.Parameters.AddWithValue("@Version", measuringDeviceObj.Version++);
                command.Parameters.AddWithValue("@Type", measuringDeviceObj.Type);
                command.Parameters.AddWithValue("@FrequencyOfCalibration", measuringDeviceObj.FrequencyOfCalibration);
                command.Parameters.AddWithValue("@LastCalibrationDate", measuringDeviceObj.LastCalibrationDate ?? DateTime.Now.Date);
                command.Parameters.AddWithValue("@NextCalibrationDate", measuringDeviceObj.NextCalibrationDate ?? DateTime.Now.Date);
                command.Parameters.AddWithValue("@ResultOfCalibration", measuringDeviceObj.ResultOfCalibration);
                command.Parameters.AddWithValue("@Status", measuringDeviceObj.Status);
                command.Parameters.AddWithValue("@CalibrationRemarks", measuringDeviceObj.CalibrationRemarks);
                command.Parameters.AddWithValue("@TrgTpgAndSettingsRemark", measuringDeviceObj.CalibrationRemarks);
                command.Parameters.AddWithValue("@Remarks", measuringDeviceObj.Remarks);
                command.Parameters.AddWithValue("@Date", measuringDeviceObj.Date);
                command.Parameters.AddWithValue("@Maker", measuringDeviceObj.Maker);
                command.Parameters.AddWithValue("@Resolution", measuringDeviceObj.Resolution);
                command.Parameters.AddWithValue("@DeviceRange", measuringDeviceObj.DeviceRange);
                command.Parameters.AddWithValue("@Accuracy", measuringDeviceObj.Accuracy);
                command.Parameters.AddWithValue("@UnitOfMeasurement", measuringDeviceObj.Unit);
                command.Parameters.AddWithValue("@Barcode", measuringDeviceObj.Barcode);
                command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceObj.CalibrationMethod);
                command.Parameters.AddWithValue("@Id", measuringDeviceObj.Id);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteMeasuringDevice(MeasuringDevice measuringDeviceObj)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand command = new NpgsqlCommand())
                {

                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"UPDATE ""IMTE"".""MeasuringDevices"" SET ""IsDeleted"" = @IsDeleted 
                                            WHERE ""Id"" = @Id";
                    command.Parameters.AddWithValue("@IsDeleted", true);
                    command.Parameters.AddWithValue("@Id", measuringDeviceObj.Id);

                    command.ExecuteNonQuery();

                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Debug.WriteLine(npgsqlEx.GetType().ToString());
                Debug.WriteLine(npgsqlEx.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetType().ToString());
                Debug.WriteLine(ex.Message);
            }
        }

    }
}

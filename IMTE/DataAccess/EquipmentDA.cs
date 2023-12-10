using IMTE.General.Models;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
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

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
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
                    });
                }

            }

            return output;
        }

        public MeasuringDevice GetMeasuringDeviceBySelectedEquipment(Equipment equipmentObj)
        {
            var output = new MeasuringDevice();

            NpgsqlConnection connection = null;
            NpgsqlCommand command = null;

            try
            {
                using (connection = new NpgsqlConnection(ConnectionString))
                using (command = new NpgsqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    string query = @"
                                    SELECT 
                                        md.""Id"" as ""MDId"", 
                                        eq.""Id"" as ""EqId"", eq.""Manufacturer"" as ""EquipmentManufacturer"", eq.""Model"" as ""EquipmentModel"", 
                                        eq.""HasAccessory"" as ""EquipmentHasAccessory"", eq.""ApprovalCode"" as ""EquipmentApprovalCode"", 
                                        eq.""IsPrinted"" as ""EquipmentIsPrinted"", eq.""IsSent"" as ""EquipmentIsSent"", 
                                        eq.""IsForeignCurrency"" as ""EquipmentIsForeignCurrency"",
                                        eqType.""Id"" as ""EquipmentTypeId"", eqType.""Name"" as ""EquipmentTypeName"",
                                        eqIt.""Id"" as ""EquipmentItemId"", eqIt.""ItemCode"" as ""EquipmentItemCode"", eqIt.""ShortDescription"" as ""EquipmentItemShortDescription"", 
                                        eqDes.""Id"" as ""EquipmentItemDescriptionId"", eqDes.""Text"" as ""EquipmentItemDescriptionText"",
                                        
        
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
                                    FROM ""IMTE"".""MeasuringDevice"" md

                                    LEFT OUTER JOIN ""Inventory"".""Equipment"" eq ON md.""EquipmentId"" = eq.""Id""
                                    LEFT OUTER JOIN ""Inventory"".""EquipmentType"" eqType ON eq.""EquipmentTypeId"" = eqType.""Id""
                                    LEFT OUTER JOIN ""General"".""Item"" eqIt ON eq.""ItemId"" = eqIt.""Id""
                                    LEFT OUTER JOIN ""General"".""Description"" eqDes ON eqIt.""DescriptionId"" = eqDes.""Id""
    
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
                                    WHERE md.""IsDeleted"" = false AND md.""EquipmentId"" = @EquipmentId;
                                ";

                    command.CommandText = query;

                    command.Parameters.AddWithValue("EquipmentId", equipmentObj.Id);

                    var data = command.ExecuteReader();

                    if (data.Read())
                    {
                        output = new MeasuringDevice
                        {
                            Id = CheckDbNullInt(data, "MDId"),
                            //Equipment = CheckDbNullInt(data, "EqId") == 0 ? null : new Equipment
                            //{
                            //    Id = CheckDbNullInt(data, "EqId"),
                            //    Manufacturer = CheckDbNullString(data, "EquipmentManufacturer"),
                            //    Model = CheckDbNullString(data, "EquipmentModel"),
                            //    HasAccessory = CheckDbNullBoolean(data, "EquipmentHasAccessory"),
                            //    ApprovalCode = CheckDbNullString(data, "EquipmentApprovalCode"),
                            //    IsPrinted = CheckDbNullBoolean(data, "EquipmentIsPrinted"),
                            //    IsSent = CheckDbNullBoolean(data, "EquipmentIsSent"),
                            //    IsForeignCurrency = CheckDbNullBoolean(data, "EquipmentIsForeignCurrency"),
                            //    EquipmentType = new EquipmentType
                            //    {
                            //        Id = CheckDbNullInt(data, "EquipmentTypeId"),
                            //        Name = CheckDbNullString(data, "EquipmentTypeName")
                            //    },
                            //    Item = new Item
                            //    {
                            //        Id = CheckDbNullInt(data, "EquipmentItemId"),
                            //        ItemCode = CheckDbNullString(data, "EquipmentItemCode"),
                            //        ShortDescription = CheckDbNullString(data, "EquipmentItemShortDescription"),
                            //        Description = new Description
                            //        {
                            //            Id = CheckDbNullInt(data, "EquipmentItemDescriptionId"),
                            //            Text = CheckDbNullString(data, "EquipmentItemDescriptionText"),
                            //        }
                            //    },
                            //},
                            Plant = new Plant
                            {
                                Id = CheckDbNullInt(data, "PlId"),
                                PlantName = CheckDbNullString(data, "PlantName"),
                            },
                            //IssuedToEmployee = new Employee
                            //{
                            //    Id = CheckDbNullInt(data, "IssuedToEmployeeId"),
                            //    Person = new Person
                            //    {
                            //        Id = CheckDbNullInt(data, "IssuedToEmployeePersonId"),
                            //        First = CheckDbNullString(data, "IssuedToEmpFirst"),
                            //        Last = CheckDbNullString(data, "IssuedToEmpLast"),
                            //        Middle = CheckDbNullString(data, "IssuedToEmpMiddle"),
                            //    },
                            //    Position = new Position
                            //    {
                            //        Id = CheckDbNullInt(data, "IssuedToEmployeePositionId"),
                            //        PositionName = CheckDbNullString(data, "IssuedToEmployeePosition"),
                            //        DutiesDescription = CheckDbNullString(data, "IssuedToEmployeeDuties"),
                            //    }
                            //},
                            //CalibratedByEmployee = new Employee
                            //{
                            //    Id = CheckDbNullInt(data, "CalibratedByEmployeeId"),
                            //    Person = new Person
                            //    {
                            //        Id = CheckDbNullInt(data, "CalibratedToEmployeePersonId"),
                            //        First = CheckDbNullString(data, "CalibratedByEmployeeFirst"),
                            //        Last = CheckDbNullString(data, "CalibratedByEmployeeLast"),
                            //        Middle = CheckDbNullString(data, "CalibratedByEmployeeMiddle"),
                            //    },
                            //    Position = new Position
                            //    {
                            //        Id = CheckDbNullInt(data, "CalibratedByEmployeePositionId"),
                            //        PositionName = CheckDbNullString(data, "CalibratedByEmployeePosition"),
                            //        DutiesDescription = CheckDbNullString(data, "CalibratedByEmployeeDuties"),
                            //    }
                            //},
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
                            //LastCalibrationDate = data.GetDateTime(data.GetOrdinal("LastCalibrationDate")),
                            //ResultOfCalibration = CheckDbNullString(data, "ResultOfCalibration"),
                            NextCalibrationDate = data.GetDateTime(data.GetOrdinal("NextCalibrationDate")),
                            Status = CheckDbNullString(data, "Status"),
                            //ThreadGaugeRingGaugeUsageNo = CheckDbNullInt(data, "ThreadGaugeRIngGaugeUsageNo"),
                            //CalibrationRemarks = CheckDbNullString(data, "CalibrationRemarks"),
                            Remarks = CheckDbNullString(data, "Remarks"),
                            Maker = CheckDbNullString(data, "Maker"),
                            Resolution = CheckDbNullString(data, "Resolution"),
                            DeviceRange = CheckDbNullString(data, "DeviceRange"),
                            Accuracy = CheckDbNullString(data, "Accuracy"),
                            Barcode = CheckDbNullString(data, "Barcode"),
                            CalibrationMethod = CheckDbNullString(data, "CalibrationMethod"),
                            //AcceptanceCriteria = CheckDbNullString(data, "AcceptanceCriteria"),
                            SerialNo = CheckDbNullString(data, "SerialNo"),
                            Unit = new UnitEntity
                            {
                                Id = CheckDbNullInt(data, "UnitId"),
                                UnitCategory = CheckDbNullString(data, "UnitCategory"),
                                UnitVal = CheckDbNullString(data, "UnitVal"),
                                Description = CheckDbNullString(data, "Description"),
                            }
                        };
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

        public void UpdateEquipment(Equipment equipmentObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                string query = @"UPDATE ""Inventory"".""Equipment"" SET
									""Version"" = ""Version"" + 1,
                                    ""EquipmentTypeId"" = @EquipmentTypeId,
                                    ""ItemId"" = @ItemId,
                                    ""Model"" = @Model,
                                    ""Manufacturer"" = @Manufacturer,
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
                command.Parameters.AddWithValue("@Manufacturer", equipmentObj.Manufacturer);
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

using IMTE.General.Models;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
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
        private readonly DescriptionDA descriptionDA;
        private readonly ItemDA itemDA;
        private readonly EquipmentDA equipmentDA;

        public MeasuringDeviceDA()
        {
            descriptionDA = new DescriptionDA();
            itemDA = new ItemDA();
            equipmentDA = new EquipmentDA();
        }

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
                                        eq.""Id"" as ""EqId"", eq.""Manufacturer"" as ""EquipmentManufacturer"", eq.""Model"" as ""EquipmentModel"", 
                                        eq.""HasAccessory"" as ""EquipmentHasAccessory"", eq.""ApprovalCode"" as ""EquipmentApprovalCode"", 
                                        eq.""IsPrinted"" as ""EquipmentIsPrinted"", eq.""IsSent"" as ""EquipmentIsSent"", 
                                        eq.""IsForeignCurrency"" as ""EquipmentIsForeignCurrency"",
                                        eqType.""Id"" as ""EquipmentTypeId"", eqType.""Name"" as ""EquipmentTypeName"",
                                        eqIt.""Id"" as ""EquipmentItemId"", eqIt.""ItemCode"" as ""EquipmentItemCode"", eqIt.""ShortDescription"" as ""EquipmentItemShortDescription"", 
                                        eqDes.""Id"" as ""EquipmentItemDescriptionId"", eqDes.""Text"" as ""EquipmentItemDescriptionText"",
                                        mt.""Id"" as ""MachineToolId"", 
                                        mtIt.""Id"" as ""MachineToolItemId"", mtIt.""ItemCode"" as ""MachineToolItemCode"", 
                                        mtIt.""ShortDescription"" as ""MachineToolItemShortDescription"",
                                        mtDes.""Id"" as ""MachineToolItemDescriptionId"", mtDes.""Text"" as ""MachineToolItemDescriptionText"",
                                        mtType.""Id"" as ""MachineToolTypeId"", 
                                        mtType.""Description"" as ""MachineToolTypeDescription"", mtType.""ToolTypeName"" as ""MachineToolTypeName"",
                                        mt.""Description"" as ""MachineToolDescription"", mt.""Note"" as ""MachineToolNote"",
                                        mt.""ToolName"" as ""MachineToolName"", mt.""UnitCost"" as ""MachineToolUnitCost"",
                                        mt.""ToolLifeUsagePcs"" as ""MachineToolLifeUsagePcs"",
        
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
                                    LEFT OUTER JOIN ""General"".""Item"" eqIt ON eq.""ItemId"" = eqIt.""Id""
                                    LEFT OUTER JOIN ""General"".""Description"" eqDes ON eqIt.""DescriptionId"" = eqDes.""Id""
    
                                    LEFT OUTER JOIN ""General"".""Plant"" pl ON md.""PlantId"" = pl.""Id""
    
                                    LEFT OUTER JOIN ""Production"".""MachineTool"" mt ON md.""MachineToolId"" = mt.""Id""
                                    LEFT OUTER JOIN ""Production"".""MachineToolType"" mtType ON mt.""MachineToolTypeId"" = mtType.""Id""
                                    LEFT OUTER JOIN ""General"".""Item"" mtIt ON mt.""ItemId"" = mtIt.""Id""
                                    LEFT OUTER JOIN ""General"".""Description"" mtDes ON mtIt.""DescriptionId"" = mtDes.""Id""
    
                                    LEFT OUTER JOIN ""HumanResources"".""Employee"" empIssuedTo ON md.""IssuedToEmployeeId"" = empIssuedTo.""Id""
                                    LEFT OUTER JOIN ""HumanResources"".""Position"" empIssuedToPosition ON empIssuedTo.""PositionId"" = empIssuedToPosition.""Id""
                                    LEFT OUTER JOIN ""General"".""Person"" empIssuedToPerson ON empIssuedTo.""PersonId"" = empIssuedToPerson.""Id""
    
                                    LEFT OUTER JOIN ""HumanResources"".""Employee"" empCalibratedBy ON md.""CalibratedByEmployeeId"" = empCalibratedBy.""Id""
                                    LEFT OUTER JOIN ""HumanResources"".""Position"" empCalibratedByPosition ON empCalibratedBy.""PositionId"" = empCalibratedByPosition.""Id""
                                    LEFT OUTER JOIN ""General"".""Person"" empCalibratedByPerson ON empCalibratedBy.""PersonId"" = empCalibratedByPerson.""Id""
    
                                    LEFT OUTER JOIN ""General"".""Department"" dept ON md.""DepartmentId"" = dept.""Id""
                                    LEFT OUTER JOIN ""General"".""Location"" loc ON md.""LocationId"" = loc.""Id""
                                    LEFT OUTER JOIN ""Definition"".""Unit"" unit ON md.""UnitId"" = unit.""Id""
                                    WHERE md.""IsDeleted"" = false;
                                ";

                    command.CommandText = query;

                    var data = command.ExecuteReader();

                    while (data.Read())
                    {
                        output.Add(new MeasuringDevice
                        {
                            Id = CheckDbNullInt(data, "MDId"),
                            Equipment = CheckDbNullInt(data, "EqId") == 0 ? null : new Equipment
                            {
                                Id = CheckDbNullInt(data, "EqId"),
                                Manufacturer = CheckDbNullString(data, "EquipmentManufacturer"),
                                Model = CheckDbNullString(data, "EquipmentModel"),
                                HasAccessory = CheckDbNullBoolean(data, "EquipmentHasAccessory"),
                                ApprovalCode = CheckDbNullString(data, "EquipmentApprovalCode"),
                                IsPrinted = CheckDbNullBoolean(data, "EquipmentIsPrinted"),
                                IsSent = CheckDbNullBoolean(data, "EquipmentIsSent"),
                                IsForeignCurrency = CheckDbNullBoolean(data, "EquipmentIsForeignCurrency"),
                                EquipmentType = new EquipmentType
                                {
                                    Id = CheckDbNullInt(data, "EquipmentTypeId"),
                                    Name = CheckDbNullString(data, "EquipmentTypeName")
                                },
                                Item = new Item
                                {
                                    Id = CheckDbNullInt(data, "EquipmentItemId"),
                                    ItemCode = CheckDbNullString(data, "EquipmentItemCode"),
                                    ShortDescription = CheckDbNullString(data, "EquipmentItemShortDescription"),
                                    Description = new Description
                                    {
                                        Id = CheckDbNullInt(data, "EquipmentItemDescriptionId"),
                                        Text = CheckDbNullString(data, "EquipmentItemDescriptionText"),
                                    }
                                },
                            },
                            MachineTool = CheckDbNullInt(data, "MachineToolId") == 0 ? null : new MachineTool
                            {
                                Id = CheckDbNullInt(data, "MachineToolId"),
                                Item = new Item
                                {
                                    Id = CheckDbNullInt(data, "MachineToolItemId"),
                                    ItemCode = CheckDbNullString(data, "MachineToolItemCode"),
                                    ShortDescription = CheckDbNullString(data, "MachineToolItemShortDescription"),
                                    Description = new Description
                                    {
                                        Id = CheckDbNullInt(data, "MachineToolItemDescriptionId"),
                                        Text = CheckDbNullString(data, "MachineToolItemDescriptionText"),
                                    }
                                },
                                MachineToolType = new MachineToolType
                                {
                                    Id = CheckDbNullInt(data, "MachineToolTypeId"),
                                    Description = CheckDbNullString(data, "MachineToolTypeDescription"),
                                    ToolTypeName = CheckDbNullString(data, "MachineToolTypeName"),
                                },
                                Description = CheckDbNullString(data, "MachineToolDescription"),
                                Note = CheckDbNullString(data, "MachineToolNote"),
                                ToolName = CheckDbNullString(data, "MachineToolName"),
                                UnitCost = CheckDbNullDecimal(data, "MachineToolUnitCost"),
                                ToolLifeUsagePcs = CheckDbNullInt(data, "MachineToolLifeUsagePcs")
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

        public MeasuringDevice GetMeasuringDeviceBySelectedMachineTool(MachineTool machineTool)
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
                                        mt.""Id"" as ""MachineToolId"", 
                                        mtIt.""Id"" as ""MachineToolItemId"", mtIt.""ItemCode"" as ""MachineToolItemCode"", 
                                        mtIt.""ShortDescription"" as ""MachineToolItemShortDescription"",
                                        mtDes.""Id"" as ""MachineToolItemDescriptionId"", mtDes.""Text"" as ""MachineToolItemDescriptionText"",
                                        mtType.""Id"" as ""MachineToolTypeId"", 
                                        mtType.""Description"" as ""MachineToolTypeDescription"", mtType.""ToolTypeName"" as ""MachineToolTypeName"",
                                        mt.""Description"" as ""MachineToolDescription"", mt.""Note"" as ""MachineToolNote"",
                                        mt.""ToolName"" as ""MachineToolName"", mt.""UnitCost"" as ""MachineToolUnitCost"",
                                        mt.""ToolLifeUsagePcs"" as ""MachineToolLifeUsagePcs"",
        
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
    
                                    LEFT OUTER JOIN ""General"".""Plant"" pl ON md.""PlantId"" = pl.""Id""
    
                                    LEFT OUTER JOIN ""Production"".""MachineTool"" mt ON md.""MachineToolId"" = mt.""Id""
                                    LEFT OUTER JOIN ""Production"".""MachineToolType"" mtType ON mt.""MachineToolTypeId"" = mtType.""Id""
                                    LEFT OUTER JOIN ""General"".""Item"" mtIt ON mt.""ItemId"" = mtIt.""Id""
                                    LEFT OUTER JOIN ""General"".""Description"" mtDes ON mtIt.""DescriptionId"" = mtDes.""Id""
    
                                    LEFT OUTER JOIN ""HumanResources"".""Employee"" empIssuedTo ON md.""IssuedToEmployeeId"" = empIssuedTo.""Id""
                                    LEFT OUTER JOIN ""HumanResources"".""Position"" empIssuedToPosition ON empIssuedTo.""PositionId"" = empIssuedToPosition.""Id""
                                    LEFT OUTER JOIN ""General"".""Person"" empIssuedToPerson ON empIssuedTo.""PersonId"" = empIssuedToPerson.""Id""
    
                                    LEFT OUTER JOIN ""HumanResources"".""Employee"" empCalibratedBy ON md.""CalibratedByEmployeeId"" = empCalibratedBy.""Id""
                                    LEFT OUTER JOIN ""HumanResources"".""Position"" empCalibratedByPosition ON empCalibratedBy.""PositionId"" = empCalibratedByPosition.""Id""
                                    LEFT OUTER JOIN ""General"".""Person"" empCalibratedByPerson ON empCalibratedBy.""PersonId"" = empCalibratedByPerson.""Id""
    
                                    LEFT OUTER JOIN ""General"".""Department"" dept ON md.""DepartmentId"" = dept.""Id""
                                    LEFT OUTER JOIN ""General"".""Location"" loc ON md.""LocationId"" = loc.""Id""
                                    LEFT OUTER JOIN ""Definition"".""Unit"" unit ON md.""UnitId"" = unit.""Id""
                                    WHERE md.""IsDeleted"" = false AND md.""MachineToolId"" = @MachineToolId;
                                ";

                    command.CommandText = query;

                    command.Parameters.AddWithValue("MachineToolId", machineTool.Id);

                    var data = command.ExecuteReader();

                    if (data.Read())
                    {
                        output = new MeasuringDevice
                        {
                            Id = CheckDbNullInt(data, "MDId"),

                            MachineTool = CheckDbNullInt(data, "MachineToolId") == 0 ? null : new MachineTool
                            {
                                Id = CheckDbNullInt(data, "MachineToolId"),
                                Item = new Item
                                {
                                    Id = CheckDbNullInt(data, "MachineToolItemId"),
                                    ItemCode = CheckDbNullString(data, "MachineToolItemCode"),
                                    ShortDescription = CheckDbNullString(data, "MachineToolItemShortDescription"),
                                    Description = new Description
                                    {
                                        Id = CheckDbNullInt(data, "MachineToolItemDescriptionId"),
                                        Text = CheckDbNullString(data, "MachineToolItemDescriptionText")
                                    },
                                },
                                MachineToolType = new MachineToolType
                                {
                                    Id = CheckDbNullInt(data, "MachineToolTypeId"),
                                    Description = CheckDbNullString(data, "MachineToolTypeDescription"),
                                    ToolTypeName = CheckDbNullString(data, "MachineToolTypeName"),
                                },
                                Description = CheckDbNullString(data, "MachineToolDescription"),
                                Note = CheckDbNullString(data, "MachineToolNote"),
                                ToolName = CheckDbNullString(data, "MachineToolName"),
                                UnitCost = CheckDbNullDecimal(data, "MachineToolUnitCost"),
                                ToolLifeUsagePcs = CheckDbNullInt(data, "MachineToolLifeUsagePcs"),
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
                                    FROM ""IMTE"".""MeasuringDevices"" md

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
                            Equipment = CheckDbNullInt(data, "EqId") == 0 ? null : new Equipment
                            {
                                Id = CheckDbNullInt(data, "EqId"),
                                Manufacturer = CheckDbNullString(data, "EquipmentManufacturer"),
                                Model = CheckDbNullString(data, "EquipmentModel"),
                                HasAccessory = CheckDbNullBoolean(data, "EquipmentHasAccessory"),
                                ApprovalCode = CheckDbNullString(data, "EquipmentApprovalCode"),
                                IsPrinted = CheckDbNullBoolean(data, "EquipmentIsPrinted"),
                                IsSent = CheckDbNullBoolean(data, "EquipmentIsSent"),
                                IsForeignCurrency = CheckDbNullBoolean(data, "EquipmentIsForeignCurrency"),
                                EquipmentType = new EquipmentType
                                {
                                    Id = CheckDbNullInt(data, "EquipmentTypeId"),
                                    Name = CheckDbNullString(data, "EquipmentTypeName")
                                },
                                Item = new Item
                                {
                                    Id = CheckDbNullInt(data, "EquipmentItemId"),
                                    ItemCode = CheckDbNullString(data, "EquipmentItemCode"),
                                    ShortDescription = CheckDbNullString(data, "EquipmentItemShortDescription"),
                                    Description = new Description
                                    {
                                        Id = CheckDbNullInt(data, "EquipmentItemDescriptionId"),
                                        Text = CheckDbNullString(data, "EquipmentItemDescriptionText"),
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
                    command.Parameters.AddWithValue("@EquipmentTypeId", equipmentObj.EquipmentType.Id);

                    equipment.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType()}: {ex.Message}");
                }

            }

            return equipment;
        }

        public MachineTool CreateMachineToolForMeasuringDevice(MachineTool machineToolObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            MachineTool machineTool = machineToolObj;

            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                try
                {
                    string query = @"INSERT INTO ""Production"".""MachineTool"" (""CompanyId"", ""ItemId"", ""MachineToolTypeId"", ""Description"",
                                    ""Note"", ""ToolName"", ""UnitCost"", ""ToolLifeUsagePcs"")
                                    VALUES(2, @ItemId, @MachineToolTypeId, @Description, @Note, @ToolName, @UnitCost, @ToolLifeUsagePcs)
                                    RETURNING ""Id""";

                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@ItemId", machineTool.Item.Id);
                    command.Parameters.AddWithValue("@MachineToolTypeId", machineTool.MachineToolType.Id);
                    command.Parameters.AddWithValue("@Description", machineTool.Item.Description.Text);
                    command.Parameters.AddWithValue("@Note", machineTool.Note);
                    command.Parameters.AddWithValue("@ToolName", machineTool.ToolName);
                    command.Parameters.AddWithValue("@UnitCost", machineTool.UnitCost);
                    command.Parameters.AddWithValue("@ToolLifeUsagePcs", machineTool.ToolLifeUsagePcs);

                    machineToolObj.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType()}: {ex.Message}");
                }
            }

            return machineTool;
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
                        Description description = null;
                        Item item = null;
                        Equipment equipment = null;
                        MachineTool machineTool = null;

                        // Will check if the data for measuring device is equipment
                        if (measuringDeviceObj.Equipment != null)
                        {
                            // Will check if the user selected a description from lookup
                            // (Non 0 value from the Equipment.Item.Description.Id && Equipment.Item.Id means that the user selected a Description or Item in lookup) same concept for machine tool logic
                            if (measuringDeviceObj.Equipment.Item.Description.Id == 0) // Will create new entry for description since the id is 0
                                description = CreateDescriptionForItemMeasuringDevice(measuringDeviceObj.Equipment.Item.Description, transaction, connection);
                            if (measuringDevice.Equipment.Item.Id == 0) // Will create new entry for Item since the id is 0
                                item = CreateItemEquipmentForMeasuringDevice(measuringDeviceObj.Equipment.Item, transaction, connection);

                            equipment = CreateEquipmentForMeasuringDevice(measuringDeviceObj.Equipment, transaction, connection);
                        }
                        // Will check if the data for measuring device is machine tool

                        else if (measuringDeviceObj.MachineTool != null)
                        {
                            if (measuringDeviceObj.MachineTool.Item.Description.Id == 0)
                                description = CreateDescriptionForItemMeasuringDevice(measuringDeviceObj.MachineTool.Item.Description, transaction, connection);
                            if (measuringDeviceObj.MachineTool.Item.Id == 0)
                                item = CreateItemEquipmentForMeasuringDevice(measuringDeviceObj.MachineTool.Item, transaction, connection);

                            machineTool = CreateMachineToolForMeasuringDevice(measuringDeviceObj.MachineTool, transaction, connection);
                        }

                        string query = @"INSERT INTO ""IMTE"".""MeasuringDevices"" (""Version"", ""EquipmentId"", ""PlantId"", ""IssuedToEmployeeId"", ""CalibratedByEmployeeId"", ""DepartmentId"",
                                    ""LocationId"", ""FrequencyOfCalibration"", ""LastCalibrationDate"", ""ResultOfCalibration"", ""NextCalibrationDate"",
                                    ""Status"", ""ThreadGaugeRIngGaugeUsageNo"", ""CalibrationRemarks"", ""Remarks"", ""Date"", ""Maker"", ""Resolution"",
                                    ""DeviceRange"", ""Accuracy"", ""Barcode"", ""CalibrationMethod"", ""AcceptanceCriteria"", ""SerialNo"", ""UnitId"", 
                                    ""MachineToolId"", ""CreatedOn"") VALUES
                                    (@Version, @EquipmentId, @PlantId, @IssuedToEmployeeId, @CalibratedByEmployeeId, @DepartmentId, @LocationId, @FrequencyOfCalibration, @LastCalibrationDate,
                                    @ResultOfCalibration, @NextCalibrationDate, @Status, @ThreadGaugeRIngGaugeUsageNo, @CalibrationRemarks, @Remarks, @Date,
                                    @Maker, @Resolution, @DeviceRange, @Accuracy, @Barcode, @CalibrationMethod, @AcceptanceCriteria, @SerialNo, @UnitId, 
                                    @MachineToolId, @CreatedOn)";

                        command.Connection = connection;
                        command.CommandText = query;

                        command.Parameters.AddWithValue("@Version", 1);

                        if (equipment != null)
                            command.Parameters.AddWithValue("@EquipmentId", equipment.Id);
                        else
                            command.Parameters.AddWithValue("@EquipmentId", DBNull.Value);

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
                        command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

                        if (machineTool != null)
                            command.Parameters.AddWithValue("@MachineToolId", machineTool.Id);
                        else
                            command.Parameters.AddWithValue("@MachineToolId", DBNull.Value);


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
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        string sql = @" UPDATE ""IMTE"".""MeasuringDevices"" SET
                            ""Version"" = @Version,
                            ""SerialNo"" = @SerialNo,
                            ""DepartmentId"" = @DepartmentId,
                            ""LocationId"" = @LocationId,
                            ""PlantId"" = @PlantId,
                            ""IssuedToEmployeeId"" = @IssuedToEmployeeId,
                            ""CalibratedByEmployeeId"" = @CalibratedByEmployeeId,
                            ""ResultOfCalibration"" = @ResultOfCalibration,
                            ""CalibrationMethod"" = @CalibrationMethod,
                            ""AcceptanceCriteria"" = @AcceptanceCriteria,
                            ""FrequencyOfCalibration"" = @FrequencyOfCalibration,
                            ""LastCalibrationDate"" = @LastCalibrationDate,
                            ""NextCalibrationDate"" = @NextCalibrationDate,
                            ""CalibrationRemarks"" = @CalibrationRemarks,
                            ""ThreadGaugeRIngGaugeUsageNo"" = @ThreadGaugeRIngGaugeUsageNo,
                            ""Status"" = @Status,
                            ""Barcode"" = @Barcode,
                            ""Remarks"" = @Remarks,
                            ""Date"" = @Date,
                            ""Maker"" = @Maker,
                            ""Resolution"" = @Resolution,
                            ""DeviceRange"" = @Range,
                            ""Accuracy"" = @Accuracy,
                            ""UnitId"" = @UnitId,
                            ""EquipmentId"" = @EquipmentId,
                            ""MachineToolId"" = @MachineToolId
                            WHERE ""Id"" = @Id";

                        command.CommandText = sql;

                        command.Parameters.AddWithValue("@Version", measuringDeviceObj.Version++);
                        command.Parameters.AddWithValue("@SerialNo", measuringDeviceObj.SerialNo);
                        command.Parameters.AddWithValue("@DepartmentId", measuringDeviceObj.Department.Id);
                        command.Parameters.AddWithValue("@LocationId", measuringDeviceObj.Location.Id);
                        command.Parameters.AddWithValue("@PlantId", measuringDeviceObj.Plant.Id);
                        command.Parameters.AddWithValue("@IssuedToEmployeeId", measuringDeviceObj.IssuedToEmployee.Id);
                        command.Parameters.AddWithValue("@CalibratedByEmployeeId", measuringDeviceObj.CalibratedByEmployee.Id);
                        command.Parameters.AddWithValue("@ResultOfCalibration", measuringDeviceObj.ResultOfCalibration);
                        command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceObj.CalibrationMethod);
                        command.Parameters.AddWithValue("@AcceptanceCriteria", measuringDeviceObj.AcceptanceCriteria);
                        command.Parameters.AddWithValue("@FrequencyOfCalibration", measuringDeviceObj.FrequencyOfCalibration);
                        command.Parameters.AddWithValue("@LastCalibrationDate", measuringDeviceObj.LastCalibrationDate.Value.ToUniversalTime());
                        command.Parameters.AddWithValue("@NextCalibrationDate", measuringDeviceObj.NextCalibrationDate.Value.ToUniversalTime());
                        command.Parameters.AddWithValue("@CalibrationRemarks", measuringDeviceObj.CalibrationRemarks);
                        command.Parameters.AddWithValue("@ThreadGaugeRIngGaugeUsageNo", measuringDeviceObj.ThreadGaugeRingGaugeUsageNo);
                        command.Parameters.AddWithValue("@Status", measuringDeviceObj.Status);
                        command.Parameters.AddWithValue("@Barcode", measuringDeviceObj.Barcode);
                        command.Parameters.AddWithValue("@Remarks", measuringDeviceObj.Remarks);
                        command.Parameters.AddWithValue("@Date", measuringDeviceObj.Date);
                        command.Parameters.AddWithValue("@Maker", measuringDeviceObj.Maker);
                        command.Parameters.AddWithValue("@Resolution", measuringDeviceObj.Resolution);
                        command.Parameters.AddWithValue("@Range", measuringDeviceObj.DeviceRange);
                        command.Parameters.AddWithValue("@Accuracy", measuringDeviceObj.Accuracy);
                        command.Parameters.AddWithValue("@UnitId", measuringDeviceObj.Unit.Id);

                        // Check if equipment
                        if (measuringDeviceObj.Equipment != null)
                        {
                            // will check if the user created a new Equipment's Item's Description(Equipment.Item.Description.Id==0 = created new)
                            if (measuringDeviceObj.Equipment.Item.Description.Id != 0)
                                descriptionDA.UpdateDescription(measuringDeviceObj.Equipment.Item.Description, transaction, connection);
                            else
                                measuringDeviceObj.Equipment.Item.Description = CreateDescriptionForItemMeasuringDevice(measuringDeviceObj.Equipment.Item.Description, transaction, connection);

                            // will check if the user created a new Equipment's Item(Equipment.Item.Id==0 = created new)
                            if (measuringDeviceObj.Equipment.Item.Id != 0)
                                itemDA.UpdateItem(measuringDeviceObj.Equipment.Item, transaction, connection);
                            else
                                measuringDeviceObj.Equipment.Item = CreateItemEquipmentForMeasuringDevice(measuringDeviceObj.Equipment.Item, transaction, connection);

                            // will check if the user created a new Equipment(Equipment.Id==0 = created new)
                            if (measuringDeviceObj.Equipment.Id != 0 && measuringDeviceObj.Equipment.Id != null)
                                equipmentDA.UpdateEquipment(measuringDeviceObj.Equipment, transaction, connection);
                            else
                                measuringDeviceObj.Equipment = CreateEquipmentForMeasuringDevice(measuringDeviceObj.Equipment, transaction, connection);

                            command.Parameters.AddWithValue("@EquipmentId", measuringDeviceObj.Equipment.Id);
                        }
                        else
                            command.Parameters.AddWithValue("@EquipmentId", DBNull.Value);

                        // Check if machine tool
                        if (measuringDeviceObj.MachineTool != null)
                        {
                            if (measuringDeviceObj.MachineTool.Id == 0)
                            {
                                measuringDeviceObj.MachineTool = CreateMachineToolForMeasuringDevice(measuringDeviceObj.MachineTool, transaction, connection);
                            }

                            command.Parameters.AddWithValue("@MachineToolId", measuringDeviceObj.MachineTool.Id);
                            descriptionDA.UpdateDescription(measuringDeviceObj.MachineTool.Item.Description, transaction, connection);
                        }
                        else
                            command.Parameters.AddWithValue("@MachineToolId", DBNull.Value);

                        command.Parameters.AddWithValue("@Id", measuringDeviceObj.Id);
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}: {ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
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

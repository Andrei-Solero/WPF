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
		private readonly MachineToolDA machineToolDA;

		public MeasuringDeviceDA()
		{
			descriptionDA = new DescriptionDA();
			itemDA = new ItemDA();
			equipmentDA = new EquipmentDA();
			machineToolDA = new MachineToolDA();
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
										md.""Id"" AS ""MDId"", md.""Version"" AS ""MDVersion"", 
										--Equipment
										eq.""Id"" AS ""EqId"", eq.""Manufacturer"" AS ""EquipmentManufacturer"", eq.""Model"" AS ""EquipmentModel"", 
										eq.""HasAccessory"" AS ""EquipmentHasAccessory"", eq.""ApprovalCode"" AS ""EquipmentApprovalCode"", 
										eq.""IsPrinted"" AS ""EquipmentIsPrinted"", eq.""IsSent"" AS ""EquipmentIsSent"", 
										eq.""IsForeignCurrency"" AS ""EquipmentIsForeignCurrency"",
										eqType.""Id"" AS ""EquipmentTypeId"", eqType.""Name"" AS ""EquipmentTypeName"",
										eqIt.""Id"" AS ""EquipmentItemId"", eqIt.""ItemCode"" AS ""EquipmentItemCode"", eqIt.""ShortDescription"" AS ""EquipmentItemShortDescription"", 
										eqDes.""Id"" AS ""EquipmentItemDescriptionId"", eqDes.""Text"" AS ""EquipmentItemDescriptionText"",
    
										--Plant
										pl.""Id"" AS ""PlId"", pl.""PlantName"",
    
										--Issued To Employee
										empIssuedTo.""Id"" AS ""IssuedToEmployeeId"", 
										empIssuedToPerson.""Id"" AS ""IssuedToEmployeePersonId"", empIssuedToPerson.""First"" AS ""IssuedToEmpFirst"", 
										empIssuedToPerson.""Last"" AS ""IssuedToEmpLast"", empIssuedToPerson.""Middle"" AS ""IssuedToEmpMiddle"",
										empIssuedToPosition.""Id"" AS ""IssuedToEmployeePositionId"", 
										empIssuedToPosition.""PositionName"" AS ""IssuedToEmployeePosition"", 
										empIssuedToPosition.""DutiesDescription"" AS ""IssuedToEmployeeDuties"",
    
										--Calibrated By Employee
										empCalibratedBy.""Id"" AS ""CalibratedByEmployeeId"",
										empCalibratedByPerson.""Id"" AS ""CalibratedToEmployeePersonId"", 
										empCalibratedByPerson.""First"" AS ""CalibratedByEmployeeFirst"", 
										empCalibratedByPerson.""Last"" AS ""CalibratedByEmployeeLast"", empCalibratedByPerson.""Middle"" AS ""CalibratedByEmployeeMiddle"",
										empCalibratedByPosition.""Id"" AS ""CalibratedByEmployeePositionId"", 
										empCalibratedByPosition.""PositionName"" AS ""CalibratedByEmployeePosition"", 
										empCalibratedByPosition.""DutiesDescription"" AS ""CalibratedByEmployeeDuties"",
    
										--Department
										dept.""Id"" AS ""DepartmentId"", dept.""DepartmentName"",
    
										--Location
										loc.""Id"" AS ""LocationId"", loc.""Name"" AS ""LocationName"",
    
										--Instrument Serial
										instSerial.""Id"" AS ""InstrumentSerialId"",
										instSerialInstrument.""Id"" AS ""InstrumentSerialInstrumentId"",
										instSerial.""SerialNo"" AS ""InstrumentSerialSerialNo"",
    
										--Instrument
										instrument.""Id"" AS ""InstrumentId"", instrumentType.""Id"" AS ""InstrumentTypeId"", instrumentType.""Name"" AS ""InstrumentTypeName"",
										instIt.""Id"" AS ""InstrumentItemId"", instIt.""ItemCode"" AS ""InstrumentItemCode"", instIt.""ShortDescription"" AS ""InstrumentItemShortDescription"",
										instDes.""Id"" AS ""InstrumentItemDescriptionId"", instDes.""Text"" AS ""InstrumentItemDescriptionText"",
										instDept.""Id"" AS ""InstrumentDepartmentId"", instDept.""DepartmentName"" AS ""InstrumentDepartmentName"", 
										instDept.""Description"" AS ""InstrumentDepartmentDescription"",
										instrument.""Manufacturer"" AS ""InstrumentManufacturer"", instrument.""Model"" AS ""InstrumentModel"", 
										instrument.""HasAccessory"" AS ""InstrumentHasAccessory"", instrument.""ApprovalCode"" AS ""InstrumentApprovalCode"",
										instrument.""IsPrinted"" AS ""InstrumentIsPrinted"", instrument.""IsSent"" AS ""InstrumentIsSent"",
										instrument.""IsForeignCurrency"" AS ""InstrumentIsForeignCurrency"",
    
										--Unit
										unit.""Id"" AS ""UnitId"", unit.""UnitCategory"", unit.""UnitVal"", unit.""Description"",
    
										--Machine Tool
										mt.""Id"" AS ""MachineToolId"", 
										mtIt.""Id"" AS ""MachineToolItemId"", mtIt.""ItemCode"" AS ""MachineToolItemCode"", 
										mtIt.""ShortDescription"" AS ""MachineToolItemShortDescription"",
										mtDes.""Id"" AS ""MachineToolItemDescriptionId"", mtDes.""Text"" AS ""MachineToolItemDescriptionText"",
										mtType.""Id"" AS ""MachineToolTypeId"", 
										mtType.""Description"" AS ""MachineToolTypeDescription"", mtType.""ToolTypeName"" AS ""MachineToolTypeName"",
										mt.""Description"" AS ""MachineToolDescription"", mt.""Note"" AS ""MachineToolNote"",
										mt.""ToolName"" AS ""MachineToolName"", mt.""UnitCost"" AS ""MachineToolUnitCost"",
										mt.""ToolLifeUsagePcs"" AS ""MachineToolLifeUsagePcs"",
    
										md.""FrequencyOfCalibration"", md.""LastCalibrationDate"",
										md.""ResultOfCalibration"", md.""NextCalibrationDate"", md.""Status"", md.""ThreadGaugeRIngGaugeUsageNo"",
										md.""CalibrationRemarks"", md.""TrgTpgAndSettingRemarks"", md.""Remarks"", md.""Maker"",
										md.""Resolution"", md.""DeviceRange"", md.""Accuracy"", md.""CalibrationCertificate"", md.""Barcode"", 
										md.""CalibrationMethod"", md.""AcceptanceCriteria"", md.""Description"", md.""SerialNo""
    
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

									LEFT OUTER JOIN ""Inventory"".""InstrumentSerial"" instSerial ON md.""InstrumentSerialId"" = instSerial.""Id""
									LEFT OUTER JOIN ""Inventory"".""Instrument"" instSerialInstrument ON instSerial.""InstrumentId"" = instSerialInstrument.""Id""

									LEFT OUTER JOIN ""Inventory"".""Instrument"" instrument ON md.""InstrumentId"" = instrument.""Id""
									LEFT OUTER JOIN ""Inventory"".""InstrumentType"" instrumentType ON instrument.""InstrumentTypeId"" = instrument.""Id""
									LEFT OUTER JOIN ""General"".""Item"" instIt ON instrument.""ItemId"" = instIt.""Id""
									LEFT OUTER JOIN ""General"".""Description"" instDes ON instIt.""DescriptionId"" = instDes.""Id""
									LEFT OUTER JOIN ""General"".""Department"" instDept ON instrument.""DepartmentId"" = instDept.""Id""

									LEFT OUTER JOIN ""Production"".""MachineTool"" mt ON md.""MachineToolId"" = mt.""Id""
									LEFT OUTER JOIN ""Production"".""MachineToolType"" mtType ON mt.""MachineToolTypeId"" = mtType.""Id""
									LEFT OUTER JOIN ""General"".""Item"" mtIt ON mt.""ItemId"" = mtIt.""Id""
									LEFT OUTER JOIN ""General"".""Description"" mtDes ON mtIt.""DescriptionId"" = mtDes.""Id""

									LEFT OUTER JOIN ""Definition"".""Unit"" unit ON md.""UnitId"" = unit.""Id""
									WHERE md.""IsDeleted"" = FALSE;
									";

					command.CommandText = query;

					var data = command.ExecuteReader();

					while (data.Read())
					{
						output.Add(new MeasuringDevice
						{
							Id = CheckDbNullInt(data, "MDId"),
							Version = CheckDbNullInt(data, "MDVersion"),
							
							Plant = new Plant
							{
								Id = CheckDbNullInt(data, "PlId"),
								PlantName = CheckDbNullString(data, "PlantName"),
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
							InstrumentSerial = CheckDbNullInt(data, "InstrumentSerialId") == 0 ? null : new InstrumentSerial
                            {
								Id = CheckDbNullInt(data, "InstrumentSerialId"),
								Instrument = CheckDbNullInt(data, "InstrumentSerialInstrumentId") == 0 ? null : new Instrument
                                {

                                },
								SerialNo = CheckDbNullString(data, "InstrumentSerialSerialNo")
							},
							
							
							FrequencyOfCalibration = CheckDbNullString(data, "FrequencyOfCalibration"),
							NextCalibrationDate = data.GetDateTime(data.GetOrdinal("NextCalibrationDate")),
							Status = CheckDbNullString(data, "Status"),
							Remarks = CheckDbNullString(data, "Remarks"),
							Maker = CheckDbNullString(data, "Maker"),
							Resolution = CheckDbNullString(data, "Resolution"),
							DeviceRange = CheckDbNullString(data, "DeviceRange"),
							Accuracy = CheckDbNullString(data, "Accuracy"),
							Barcode = CheckDbNullString(data, "Barcode"),
							CalibrationMethod = CheckDbNullString(data, "CalibrationMethod"),
							AcceptanceCriteria = CheckDbNullString(data, "AcceptanceCriteria"),
                            SerialNo = CheckDbNullString(data, "SerialNo")
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
                                    FROM ""IMTE"".""MeasuringDevice"" md
    
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
							NextCalibrationDate = data.GetDateTime(data.GetOrdinal("NextCalibrationDate")),
							Status = CheckDbNullString(data, "Status"),
							Remarks = CheckDbNullString(data, "Remarks"),
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
							Plant = new Plant
							{
								Id = CheckDbNullInt(data, "PlId"),
								PlantName = CheckDbNullString(data, "PlantName"),
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
							NextCalibrationDate = data.GetDateTime(data.GetOrdinal("NextCalibrationDate")),
							Status = CheckDbNullString(data, "Status"),
							Remarks = CheckDbNullString(data, "Remarks"),
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

		public EquipmentSerial CreateEquipmentSerialForMeasuringDevice(EquipmentSerial equipmentSerialObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
			EquipmentSerial equipmentSerial = equipmentSerialObj;

			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				try
				{
					string query = @"INSERT INTO ""Inventory"".""EquipmentSerial"" (""EquipmentId"", ""SerialNo"", ""CreatedOn"")VALUES
									(@EquipmentId, @SerialNo, @CreatedOn) RETURNING ""Id""";

					command.Connection = connection;
					command.Transaction = transaction;
					command.CommandText = query;

					command.Parameters.AddWithValue("@EquipmentId", equipmentSerial.Equipment.Id);
					command.Parameters.AddWithValue("@SerialNo", equipmentSerial.SerialNo);
					command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

					equipmentSerial.Id = Convert.ToInt32(command.ExecuteScalar());
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.GetType()}: {ex.Message}");
				}

			}

			return equipmentSerial;
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
                                    ""EquipmentTypeId"", ""CreatedOn"")
                                    VALUES(2, @ItemId, @Manufacturer, @Model, @HasAccessory, @ApprovalCode, @IsPrinted, @IsSent, @IsForeignCurrency,
                                    @EquipmentTypeId, @CreatedOn)
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
					command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

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
					// TODO : MANUALLY SAVING OF THE COMPANY
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

		public MachineToolSerial CreateMachineTooLSerialForMeasuringDevice(MachineToolSerial machineToolSerialObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
			MachineToolSerial machineToolSerial = machineToolSerialObj;

			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				try
				{
					string query = @"INSERT INTO ""Inventory"".""EquipmentSerial"" (""EquipmentId"", ""SerialNo"", ""CreatedOn"")VALUES
									(@EquipmentId, @SerialNo, @CreatedOn) RETURNING ""Id""";

					command.Connection = connection;
					command.Transaction = transaction;
					command.CommandText = query;

					// TODO: CREATE THE MAIN FOR MACHINE TOOLSERIAL

					machineToolSerial.Id = Convert.ToInt32(command.ExecuteScalar());
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.GetType()}: {ex.Message}");
				}

			}

			return machineToolSerial;
		}


		public Instrument CreateInstrumentForMeasuringDevice(Instrument instrumentObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
			var instrument = instrumentObj;

			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				try
				{
					// TODO : MANUALLY SAVING OF THE ASSET AND THE COMPANY
					string query = @"INSERT INTO ""Inventory"".""Instrument""(""CompanyId"",""AssetId"", ""InstrumentTypeId"", ""ItemId"", ""DepartmentId"", ""Manufacturer"",
									""Model"", ""HasAccessory"", ""ApprovalCode"", ""IsPrinted"", ""IsSent"", ""IsForeignCurrency"", ""CreatedDate"", ""CreatedOn"")VALUES
									(2, 1, @InstrumentTypeId, @ItemId, @DepartmentId, @Manufacturer, @Model, @HasAccessory, @ApprovalCode, @IsPrinted, @IsSent, @IsForeignCurrency, @CreatedDate,
									@CreatedOn)
									RETURNING ""Id""";

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
					command.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);
					command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

					instrument.Id = Convert.ToInt32(command.ExecuteScalar());
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.GetType()}: {ex.Message}");
				}
			}

			return instrument;
        }

		public InstrumentSerial CreateInstrumentSerialForMeasuringDevice(InstrumentSerial instrumentSerialObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
		{
			InstrumentSerial instrumentSerial = instrumentSerialObj;

			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				try
				{
					string query = @"INSERT INTO ""Inventory"".""InstrumentSerial"" (""InstrumentId"", ""SerialNo"", ""CreatedOn"")VALUES
									(@InstrumentId, @SerialNo, @CreatedOn) RETURNING ""Id""";



					command.Connection = connection;
					command.Transaction = transaction;
					command.CommandText = query;

					command.Parameters.AddWithValue("@InstrumentId", instrumentSerial.Instrument.Id);
					command.Parameters.AddWithValue("@SerialNo", instrumentSerial.SerialNo);
					command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

					// TODO: CREATE THE MAIN FOR MACHINE TOOLSERIAL

					instrumentSerial.Id = Convert.ToInt32(command.ExecuteScalar());
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.GetType()}: {ex.Message}");
				}

			}

			return instrumentSerial;
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
						string query = @"
										INSERT INTO ""IMTE"".""MeasuringDevice""(
										""Version"", ""PlantId"", ""DepartmentId"", ""LocationId"",
										""InstrumentSerialId"", ""UnitId"", ""FrequencyOfCalibration"", ""NextCalibrationDate"", ""Status"",
										""Remarks"", ""Maker"", ""Resolution"", ""DeviceRange"", ""Accuracy"", ""Barcode"",
										""CalibrationMethod"", ""AcceptanceCriteria"", ""CreatedOn"", ""Description"",
										""SerialNo"", ""DeviceTypeId"", ""EquipmentSerialId"", ""MachineToolSerialId"",
										""EndOfLife""
									)
									VALUES (
										1, @PlantId, @DepartmentId, @LocationId,
										@InstrumentSerialId, @UnitId, @FrequencyOfCalibration, @NextCalibrationDate, @Status,
										@Remarks, @Maker, @Resolution, @DeviceRange, @Accuracy, @Barcode,
										@CalibrationMethod, @AcceptanceCriteria, @CreatedOn, @Description,
										@SerialNo, @DeviceTypeId, @EquipmentSerialId, @MachineToolSerialId,
										@EndOfLife
									);";

						command.Connection = connection;
						command.CommandText = query;

						command.Parameters.AddWithValue("@PlantId", measuringDevice.Plant.Id);
						command.Parameters.AddWithValue("@DepartmentId", measuringDevice.Department.Id);
						command.Parameters.AddWithValue("@LocationId", measuringDevice.Location.Id);

						command.Parameters.AddWithValue("@UnitId", measuringDevice.Unit.Id);
						command.Parameters.AddWithValue("@FrequencyOfCalibration", measuringDevice.FrequencyOfCalibration);
						command.Parameters.AddWithValue("@NextCalibrationDate", measuringDevice.NextCalibrationDate);
						command.Parameters.AddWithValue("@Status", measuringDevice.Status);
						command.Parameters.AddWithValue("@Remarks", measuringDevice.Remarks);
						command.Parameters.AddWithValue("@Maker", measuringDevice.Maker);
						command.Parameters.AddWithValue("@Resolution", measuringDevice.Maker);
						command.Parameters.AddWithValue("@DeviceRange", measuringDevice.DeviceRange);
						command.Parameters.AddWithValue("@Accuracy", measuringDevice.Accuracy);
						command.Parameters.AddWithValue("@Barcode", measuringDevice.Barcode);
						command.Parameters.AddWithValue("@CalibrationMethod", measuringDevice.CalibrationMethod);
						command.Parameters.AddWithValue("@AcceptanceCriteria", measuringDevice.AcceptanceCriteria);
						command.Parameters.AddWithValue("@CreatedOn", DateTime.Now.ToUniversalTime());
						command.Parameters.AddWithValue("@Description", measuringDevice.Description);
						command.Parameters.AddWithValue("@SerialNo", measuringDevice.SerialNo);
						command.Parameters.AddWithValue("@DeviceTypeId", measuringDevice.DeviceType.Id);


                        // check if the tool to be saved is equipment serial
                        var equipmentSerialData = measuringDevice.EquipmentSerial;
						if (equipmentSerialData != null)
                        {
							// check first the value for equipment item's description
							// if the id of equipmentItemDescription obj is equal to 0, then create new description to the table
							var equipmentItemDescription = equipmentSerialData.Equipment.Item.Description;
							if (equipmentItemDescription.Id == 0 || equipmentItemDescription.Id == null)
								measuringDevice.EquipmentSerial.Equipment.Item.Description = CreateDescriptionForItemMeasuringDevice(equipmentItemDescription, transaction, connection);

							// check the value of the equipment's item
							// if the id of equipmentItem obj is equal to 0, then create new item to the table
							var equipmentItem = equipmentSerialData.Equipment.Item;
							if (equipmentItem.Id == 0 || equipmentItem.Id == null)
								measuringDevice.EquipmentSerial.Equipment.Item = CreateItemEquipmentForMeasuringDevice(equipmentItem, transaction, connection);

							// check for the value of the entire equipment's data
							var equipment = equipmentSerialData.Equipment;
							if (equipment.Id == 0 || equipment.Id == null)
								measuringDevice.EquipmentSerial.Equipment = CreateEquipmentForMeasuringDevice(equipment, transaction, connection);

							measuringDevice.EquipmentSerial = CreateEquipmentSerialForMeasuringDevice(measuringDevice.EquipmentSerial, transaction, connection);

							command.Parameters.AddWithValue("@EquipmentSerialId", measuringDevice.EquipmentSerial.Id);
						}
						else
							command.Parameters.AddWithValue("@EquipmentSerialId", DBNull.Value);

						// check if the tool to be saved is machine tool serial
						var machineToolSerialData = measuringDevice.MachineToolSerial;
						if (machineToolSerialData != null)
                        {
							// check first the value for machine tool item's description
							// if the id of machineToolItemDescription obj is equal to 0, then create new description to the table
							var machineToolItemDescription = machineToolSerialData.MachineTool.Item.Description;
							if (machineToolItemDescription.Id == 0 || machineToolItemDescription.Id == null)
								measuringDevice.MachineToolSerial.MachineTool.Item.Description = CreateDescriptionForItemMeasuringDevice(machineToolItemDescription, transaction, connection);

							// check the value of the equipment's item
							// if the id of equipmentItem obj is equal to 0, then create new item to the table
							var machineToolItem = machineToolSerialData.MachineTool.Item;
							if (machineToolItem.Id == 0 || machineToolItem.Id == null)
								measuringDevice.MachineToolSerial.MachineTool.Item = CreateItemEquipmentForMeasuringDevice(machineToolItem, transaction, connection);

							// check for the value of the entire equipment's data
							var machineTool = machineToolSerialData.MachineTool;
							if (machineTool.Id == 0 || machineTool.Id == null)
								measuringDevice.MachineToolSerial.MachineTool = CreateMachineToolForMeasuringDevice(machineTool, transaction, connection);

							command.Parameters.AddWithValue("@EquipmentSerialId", measuringDevice.EquipmentSerial.Id);
						}
						else
							command.Parameters.AddWithValue("@MachineToolSerialId", DBNull.Value);

						// check if the tool to be saved is instrument serial
						var instrumentSerialData = measuringDevice.InstrumentSerial;
						if (instrumentSerialData.Id != 0)
						{
							// check first the value for equipment item's description
							// if the id of equipmentItemDescription obj is equal to 0, then create new description to the table
							var instrumentItemDescription = instrumentSerialData.Instrument.Item.Description;
							if (instrumentItemDescription.Id == 0 || instrumentItemDescription.Id == null)
								measuringDevice.InstrumentSerial.Instrument.Item.Description = CreateDescriptionForItemMeasuringDevice(instrumentItemDescription, transaction, connection);

							// check the value of the equipment's item
							// if the id of equipmentItem obj is equal to 0, then create new item to the table
							var instrumentItem = instrumentSerialData.Instrument.Item;
							if (instrumentItem.Id == 0 || instrumentItem.Id == null)
								measuringDevice.InstrumentSerial.Instrument.Item = CreateItemEquipmentForMeasuringDevice(instrumentItem, transaction, connection);

							// check for the value of the entire equipment's data
							var instrument = instrumentSerialData.Instrument;
							if (instrument.Id == 0 || instrument.Id == null)
								measuringDevice.InstrumentSerial.Instrument = CreateInstrumentForMeasuringDevice(instrument, transaction, connection);

							measuringDevice.InstrumentSerial = CreateInstrumentSerialForMeasuringDevice(measuringDevice.InstrumentSerial, transaction, connection);

							command.Parameters.AddWithValue("@InstrumentSerialId", measuringDevice.InstrumentSerial.Id);
						}
						else
							command.Parameters.AddWithValue("@InstrumentSerialId", DBNull.Value);

						command.Parameters.AddWithValue("@EndOfLife", measuringDeviceObj.EndOfLife);

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

		// TODO: INSTRUMENT SERIAL REQUIRED => LOOKUP
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
						string sql = @" UPDATE ""IMTE"".""MeasuringDevice"" SET
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
						//command.Parameters.AddWithValue("@IssuedToEmployeeId", measuringDeviceObj.IssuedToEmployee.Id);
						//command.Parameters.AddWithValue("@CalibratedByEmployeeId", measuringDeviceObj.CalibratedByEmployee.Id);
						//command.Parameters.AddWithValue("@ResultOfCalibration", measuringDeviceObj.ResultOfCalibration);
						//command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceObj.CalibrationMethod);
						//command.Parameters.AddWithValue("@AcceptanceCriteria", measuringDeviceObj.AcceptanceCriteria);
						//command.Parameters.AddWithValue("@FrequencyOfCalibration", measuringDeviceObj.FrequencyOfCalibration);
						//command.Parameters.AddWithValue("@LastCalibrationDate", measuringDeviceObj.LastCalibrationDate.Value.ToUniversalTime());
						//command.Parameters.AddWithValue("@NextCalibrationDate", measuringDeviceObj.NextCalibrationDate.Value.ToUniversalTime());
						//command.Parameters.AddWithValue("@CalibrationRemarks", measuringDeviceObj.CalibrationRemarks);
						//command.Parameters.AddWithValue("@ThreadGaugeRIngGaugeUsageNo", measuringDeviceObj.ThreadGaugeRingGaugeUsageNo);
						command.Parameters.AddWithValue("@Status", measuringDeviceObj.Status);
						command.Parameters.AddWithValue("@Barcode", measuringDeviceObj.Barcode);
						command.Parameters.AddWithValue("@Remarks", measuringDeviceObj.Remarks);
						command.Parameters.AddWithValue("@Maker", measuringDeviceObj.Maker);
						command.Parameters.AddWithValue("@Resolution", measuringDeviceObj.Resolution);
						command.Parameters.AddWithValue("@Range", measuringDeviceObj.DeviceRange);
						command.Parameters.AddWithValue("@Accuracy", measuringDeviceObj.Accuracy);
						command.Parameters.AddWithValue("@UnitId", measuringDeviceObj.Unit.Id);


						// Check if equipment
						//if (measuringDeviceObj.Equipment != null)
						//{
						//	// will check if the user created a new Equipment's Item's Description(Equipment.Item.Description.Id==0 = created new)
						//	if (measuringDeviceObj.Equipment.Item.Description.Id != 0)
						//		descriptionDA.UpdateDescription(measuringDeviceObj.Equipment.Item.Description, transaction, connection);
						//	else
						//		measuringDeviceObj.Equipment.Item.Description = CreateDescriptionForItemMeasuringDevice(measuringDeviceObj.Equipment.Item.Description, transaction, connection);

						//	// will check if the user created a new Equipment's Item(Equipment.Item.Id==0 = created new)
						//	if (measuringDeviceObj.Equipment.Item.Id != 0)
						//		itemDA.UpdateItem(measuringDeviceObj.Equipment.Item, transaction, connection);
						//	else
						//		measuringDeviceObj.Equipment.Item = CreateItemEquipmentForMeasuringDevice(measuringDeviceObj.Equipment.Item, transaction, connection);

						//	// will check if the user created a new Equipment(Equipment.Id==0 = created new)
						//	if (measuringDeviceObj.Equipment.Id != 0 && measuringDeviceObj.Equipment.Id != null)
						//		equipmentDA.UpdateEquipment(measuringDeviceObj.Equipment, transaction, connection);
						//	else
						//		measuringDeviceObj.Equipment = CreateEquipmentForMeasuringDevice(measuringDeviceObj.Equipment, transaction, connection);

						//	command.Parameters.AddWithValue("@EquipmentId", measuringDeviceObj.Equipment.Id);
						//}
						//else
						//	command.Parameters.AddWithValue("@EquipmentId", DBNull.Value);

						//// Check if machine tool
						//if (measuringDeviceObj.MachineTool != null)
						//{
						//	var machineTool = measuringDeviceObj.MachineTool;

						//	if (machineTool.Item.Description.Id != 0)
						//		descriptionDA.UpdateDescription(machineTool.Item.Description, transaction, connection);
						//	else
						//		machineTool.Item.Description = CreateDescriptionForItemMeasuringDevice(machineTool.Item.Description, transaction, connection);

						//	if (machineTool.Item.Id != 0)
						//		itemDA.UpdateItem(machineTool.Item, transaction, connection);
						//	else
						//		machineTool.Item = CreateItemEquipmentForMeasuringDevice(machineTool.Item, transaction, connection);

						//	if (machineTool.Id != 0 && machineTool.Id != null)
						//		machineToolDA.UpdateMachineTool(machineTool, transaction, connection);
						//	else
						//		machineTool = CreateMachineToolForMeasuringDevice(measuringDeviceObj.MachineTool, transaction, connection);

						//	command.Parameters.AddWithValue("@MachineToolId", measuringDeviceObj.MachineTool.Id);
						//}
						//else
						//	command.Parameters.AddWithValue("@MachineToolId", DBNull.Value);

						//command.Parameters.AddWithValue("@Id", measuringDeviceObj.Id);
						//command.ExecuteNonQuery();
						//transaction.Commit();
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
					command.CommandText = @"UPDATE ""IMTE"".""MeasuringDevice"" SET ""IsDeleted"" = @IsDeleted 
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

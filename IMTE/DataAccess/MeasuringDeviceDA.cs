using IMTE.General.Models;
using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.IMTEEntity;
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
		private readonly InstrumentDA instrumentDA;
		private readonly InstrumentSerialDA instrumentSerialDA;
		private readonly EquipmentSerialDA equipmentSerialDA;
		private readonly MachineToolSerialDA machineToolSerialDA;

		public MeasuringDeviceDA()
		{
			descriptionDA = new DescriptionDA();
			itemDA = new ItemDA();
			equipmentDA = new EquipmentDA();
			machineToolDA = new MachineToolDA();
			instrumentDA = new InstrumentDA();
			instrumentSerialDA = new InstrumentSerialDA();
			equipmentSerialDA = new EquipmentSerialDA();
			machineToolSerialDA = new MachineToolSerialDA();
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
									-- Equipment
    
									-- Plant
									pl.""Id"" AS ""PlId"", pl.""PlantName"",
    
									-- Department
									dept.""Id"" AS ""DepartmentId"", dept.""DepartmentName"",
    
									-- Location
									loc.""Id"" AS ""LocationId"", loc.""Name"" AS ""LocationName"",
    
									-- Instrument Serial
									instSerial.""Id"" AS ""InstrumentSerialId"",
									instSerial.""SerialNo"" AS ""InstrumentSerialSerialNo"",
    
									-- Instrument Serial's Instrument
									instSerialInstrument.""Id"" AS ""InstrumentSerialInstrumentId"", instType.""Id"" AS ""InstrumentTypeId"", instType.""Name"" AS ""InstrumentTypeName"",
									instIt.""Id"" AS ""InstrumentItemId"", instIt.""ItemCode"" AS ""InstrumentItemCode"", instIt.""ShortDescription"" AS ""InstrumentItemShortDescription"",
									instDes.""Id"" AS ""InstrumentItemDescriptionId"", instDes.""Text"" AS ""InstrumentItemDescriptionText"",
									instDept.""Id"" AS ""InstrumentDepartmentId"", instDept.""DepartmentName"" AS ""InstrumentDepartmentName"", 
									instDept.""Description"" AS ""InstrumentDepartmentDescription"",
									instSerialInstrument.""Manufacturer"" AS ""InstrumentManufacturer"", instSerialInstrument.""Model"" AS ""InstrumentModel"", 
									instSerialInstrument.""HasAccessory"" AS ""InstrumentHasAccessory"", instSerialInstrument.""ApprovalCode"" AS ""InstrumentApprovalCode"",
									instSerialInstrument.""IsPrinted"" AS ""InstrumentIsPrinted"", instSerialInstrument.""IsSent"" AS ""InstrumentIsSent"",
									instSerialInstrument.""IsForeignCurrency"" AS ""InstrumentIsForeignCurrency"",
    
									-- Unit
									unit.""Id"" AS ""UnitId"", unit.""UnitCategory"", unit.""UnitVal"", unit.""Description"" AS ""UnitDescription"",
    
									md.""FrequencyOfCalibration"", md.""NextCalibrationDate"",
									md.""Status"" AS ""MeasuringDeviceStatus"", md.""Remarks"" AS ""MeasuringDeviceRemarks"", md.""Maker"" AS ""MeasuringDeviceMaker"", md.""Resolution"" AS ""MeasuringDeviceResolution"",
									md.""DeviceRange"" AS ""MeasuringDeviceRange"", md.""Accuracy"" AS ""MeasuringDeviceAccuracy"", md.""Barcode"" AS ""MeasuringDeviceBarcode"", md.""CalibrationMethod"" AS ""MeasuringDeviceCalibrationMethod"",
									md.""AcceptanceCriteria"" AS ""MeasuringDeviceAcceptanceCriteria"", md.""Description"" AS ""MeasuringDeviceDescription"", md.""SerialNo"" AS ""MeasuringDeviceSerialNo"", 
    
									-- Device Type
									devType.""Id"" AS ""DeviceTypeId"", devType.""DeviceTypeName"",
    
									-- Equipment Serial
									equipSerial.""Id"" AS ""EquipmentSerialId"", equipSerial.""SerialNo"" AS ""EquipmentSerialSerialNo"",
    
									-- Equipment Serial's Equipment
									eq.""Id"" AS ""EquipmentSerialEquipmentId"", eq.""Manufacturer"" AS ""EquipmentManufacturer"", eq.""Model"" AS ""EquipmentModel"", 
									eq.""HasAccessory"" AS ""EquipmentHasAccessory"", eq.""ApprovalCode"" AS ""EquipmentApprovalCode"", 
									eq.""IsPrinted"" AS ""EquipmentIsPrinted"", eq.""IsSent"" AS ""EquipmentIsSent"", 
									eq.""IsForeignCurrency"" AS ""EquipmentIsForeignCurrency"",
									eqType.""Id"" AS ""EquipmentTypeId"", eqType.""Name"" AS ""EquipmentTypeName"",
									eqIt.""Id"" AS ""EquipmentItemId"", eqIt.""ItemCode"" AS ""EquipmentItemCode"", eqIt.""ShortDescription"" AS ""EquipmentItemShortDescription"", 
									eqDes.""Id"" AS ""EquipmentItemDescriptionId"", eqDes.""Text"" AS ""EquipmentItemDescriptionText"",

									-- Machine Tool Serial
									mtSerial.""Id"" AS ""MachineToolSerialId"", mtSerial.""SerialNo"" AS ""MachineToolSerialNo"",
									mtSerial.""ToolUsageLifePcs"" AS ""MachineToolToolUsageLifePcs"", mtSerial.""Quantity"" AS ""MachineToolSerialQuantity"",
    
									-- Machine Tool Serial Status
									mtSerialStatus.""Id"" AS ""MachineTooLSerialStatusId"", mtSerialStatus.""Status"" AS ""MachineToolSerialStatusStatus"",
    
									-- Machine Tool Serial's Machine Tool
									mt.""Id"" AS ""MachineToolId"", 
									mtIt.""Id"" AS ""MachineToolItemId"", mtIt.""ItemCode"" AS ""MachineToolItemCode"", 
									mtIt.""ShortDescription"" AS ""MachineToolItemShortDescription"",
									mtDes.""Id"" AS ""MachineToolItemDescriptionId"", mtDes.""Text"" AS ""MachineToolItemDescriptionText"",
									mtType.""Id"" AS ""MachineToolTypeId"", 
									mtType.""Description"" AS ""MachineToolTypeDescription"", mtType.""ToolTypeName"" AS ""MachineToolTypeName"",
									mt.""Description"" AS ""MachineToolDescription"", mt.""Note"" AS ""MachineToolNote"",
									mt.""ToolName"" AS ""MachineToolName"", mt.""UnitCost"" AS ""MachineToolUnitCost"",
									mt.""ToolLifeUsagePcs"" AS ""MachineToolLifeUsagePcs"",
    
									md.""EndOfLife"",
    
									-- Acceptance Criteria
									acc.""Id"" AS ""AcceptanceCriteriaId"", acc.""Title"" AS ""AcceptanceCriteriaTitle"", acc.""Description"" AS ""AcceptanceCriteriaDescription"",
    
									-- Maker
									mak.""Id"" AS ""MakerId"", mak.""Name"" AS ""MakerName"",
    
									-- Resolution
									res.""Id"" AS ""ResolutionId"", res.""Title"" AS ""ResolutionTitle"", res.""Description"" AS ""ResolutionDescription"", res.""ResolutionValue"" AS ""ResolutionValue"",
    
									--Frequency OF Calibration
    								freq.""Id"" AS ""FrequencyOfCalibrationId"", freq.""Title"" AS ""FrequencyOfCalibrationTitle"", freq.""Frequency"" AS ""FrequencyOfCalibrationFrequency"", 
									freq.""Description"" AS ""FrequencyOfCalibrationDescription""
    
								FROM ""IMTE"".""MeasuringDevice"" md

								-- Plant joins
								LEFT OUTER JOIN ""General"".""Plant"" pl ON md.""PlantId"" = pl.""Id""

								-- Department joins
								LEFT OUTER JOIN ""General"".""Department"" dept ON md.""DepartmentId"" = dept.""Id""

								-- Location joins
								LEFT OUTER JOIN ""General"".""Location"" loc ON md.""LocationId"" = loc.""Id""

								-- Instrument Serial joins
								LEFT OUTER JOIN ""Inventory"".""InstrumentSerial"" instSerial ON md.""InstrumentSerialId"" = instSerial.""Id""

								-- Instrument Serial's Instrument joins
								LEFT OUTER JOIN ""Inventory"".""Instrument"" instSerialInstrument ON instSerial.""InstrumentId"" = instSerialInstrument.""Id""
								LEFT OUTER JOIN ""Inventory"".""InstrumentType"" instType ON instSerialInstrument.""InstrumentTypeId"" = instType.""Id""
								LEFT OUTER JOIN ""General"".""Item"" instIt ON instSerialInstrument.""ItemId"" = instIt.""Id""
								LEFT OUTER JOIN ""General"".""Description"" instDes ON instIt.""DescriptionId"" = instDes.""Id""
								LEFT OUTER JOIN ""General"".""Department"" instDept ON instSerialInstrument.""DepartmentId"" = instDept.""Id""

								-- Unit joins
								LEFT OUTER JOIN ""Definition"".""Unit"" unit ON md.""UnitId"" = unit.""Id""

								-- Device type joins
								LEFT OUTER JOIN ""IMTE"".""DeviceType"" devType ON md.""DeviceTypeId"" = devType.""Id""

								-- Equipment Serial joins
								LEFT OUTER JOIN ""Inventory"".""EquipmentSerial"" equipSerial ON md.""EquipmentSerialId"" = equipSerial.""Id""

								-- Equipment Serial's Equipment joins
								LEFT OUTER JOIN ""Inventory"".""Equipment"" eq ON equipSerial.""EquipmentId"" = eq.""Id""
								LEFT OUTER JOIN ""Inventory"".""EquipmentType"" eqType ON eq.""EquipmentTypeId"" = eqType.""Id""
								LEFT OUTER JOIN ""General"".""Item"" eqIt ON eq.""ItemId"" = eqIt.""Id""
								LEFT OUTER JOIN ""General"".""Description"" eqDes ON eqIt.""DescriptionId"" = eqDes.""Id""

								-- Machine tool serial joins
								LEFT OUTER JOIN ""Production"".""MachineToolSerial"" mtSerial ON md.""MachineToolSerialId"" = mtSerial.""Id""

								-- Machine tool serial status joins
								LEFT OUTER JOIN ""Production"".""MachineToolStatus"" mtSerialStatus ON mtSerial.""MachineToolSpecClassId"" = mtSerialStatus.""Id""

								-- Machine Tool Serial's Machine Tool joins
								LEFT OUTER JOIN ""Production"".""MachineTool"" mt ON mtSerial.""MachineToolId"" = mt.""Id""
								LEFT OUTER JOIN ""Production"".""MachineToolType"" mtType ON mt.""MachineToolTypeId"" = mtType.""Id""
								LEFT OUTER JOIN ""General"".""Item"" mtIt ON mt.""ItemId"" = mtIt.""Id""
								LEFT OUTER JOIN ""General"".""Description"" mtDes ON mtIt.""DescriptionId"" = mtDes.""Id""

								-- Acceptance Crtieria Joins
								LEFT OUTER JOIN ""IMTE"".""AcceptanceCriteria"" acc ON md.""AcceptanceCriteriaId"" = acc.""Id""

								-- Resolution joins
								LEFT OUTER JOIN ""IMTE"".""Resolution"" res ON md.""ResolutionId"" = res.""Id""

								-- Maker joins
								LEFT OUTER JOIN ""IMTE"".""Maker"" mak ON md.""MakerId"" = mak.""Id""

								-- Frequency of Calibration joins
								LEFT OUTER JOIN ""IMTE"".""FrequencyOfCalibration"" freq ON md.""FrequencyOfCalibrationId"" = freq.""Id""

								WHERE md.""IsDeleted"" = FALSE;
								";



					command.CommandText = query;

					object objectCheckerValue = 0;

					var data = command.ExecuteReader();

					while (data.Read())
					{
						output.Add(new MeasuringDevice 
						{ 
							Id = CheckDbNullInt(data, "MDId"),
							Plant = CheckDbNullInt(data, "PlId").Equals(objectCheckerValue) ? null : new Plant
							{
								Id = CheckDbNullInt(data, "PlId"),
								PlantName = CheckDbNullString(data, "PlantName")
							},
							Department = CheckDbNullInt(data, "DepartmentId").Equals(objectCheckerValue) ? null : new Department
							{
								Id = CheckDbNullInt(data, "DepartmentId"),
								DepartmentName = CheckDbNullString(data, "DepartmentName")
							},
							Location = CheckDbNullInt(data, "LocationId").Equals(objectCheckerValue) ? null : new Location
							{
								Id = CheckDbNullInt(data, "LocationId"),
								Name = CheckDbNullString(data, "LocationName")
							},
							InstrumentSerial = CheckDbNullInt(data, "InstrumentSerialId").Equals(objectCheckerValue) ? null : new InstrumentSerial
							{
								Id = CheckDbNullInt(data, "InstrumentSerialId"),
								SerialNo = CheckDbNullString(data, "InstrumentSerialSerialNo"),
								Instrument = CheckDbNullInt(data, "InstrumentSerialInstrumentId").Equals(objectCheckerValue) ? null : new Instrument 
								{ 
									Id = CheckDbNullInt(data, "InstrumentSerialInstrumentId"),
									InstrumentType = CheckDbNullInt(data, "InstrumentTypeId").Equals(objectCheckerValue) ? null : new InstrumentType
									{
										Id = CheckDbNullInt(data, "InstrumentTypeId"),
										Name = CheckDbNullString(data, "InstrumentTypeName")
									},
									Item = CheckDbNullInt(data, "InstrumentItemId").Equals(objectCheckerValue) ? null : new Item
									{
										Id = CheckDbNullInt(data, "InstrumentItemId"),
										ItemCode = CheckDbNullString(data, "InstrumentItemCode"),
										ShortDescription = CheckDbNullString(data, "InstrumentItemShortDescription"),
										Description = CheckDbNullInt(data, "InstrumentItemDescriptionId").Equals(objectCheckerValue) ? null : new Description
										{
											Id = CheckDbNullInt(data, "InstrumentItemDescriptionId"),
											Text = CheckDbNullString(data, "InstrumentItemDescriptionText")
										}
									},
									Department = CheckDbNullInt(data, "InstrumentDepartmentId").Equals(objectCheckerValue) ? null : new Department
									{
										Id = CheckDbNullInt(data, "InstrumentDepartmentId"),
										DepartmentName = CheckDbNullString(data, "InstrumentDepartmentName"),
										Description = CheckDbNullString(data, "InstrumentDepartmentDescription")
									},
									Manufacturer = CheckDbNullString(data, "InstrumentManufacturer"),
									Model = CheckDbNullString(data, "InstrumentModel"),
									HasAccessory = CheckDbNullBoolean(data, "InstrumentHasAccessory"),
									ApprovalCode = CheckDbNullString(data, "InstrumentApprovalCode"),
									IsPrinted = CheckDbNullBoolean(data, "InstrumentIsPrinted"),
									IsSent = CheckDbNullBoolean(data, "InstrumentIsSent"),
									IsForeignCurrency = CheckDbNullBoolean(data, "InstrumentIsForeignCurrency")
								},
							},
							Unit = CheckDbNullInt(data, "UnitId").Equals(objectCheckerValue) ? null : new UnitEntity
							{
								Id = CheckDbNullInt(data, "UnitId"),
								UnitCategory = CheckDbNullString(data, "UnitCategory"),
								UnitVal = CheckDbNullString(data, "UnitVal"),
								Description = CheckDbNullString(data, "UnitDescription")
							},
							FrequencyOfCalibration = CheckDbNullString(data, "FrequencyOfCalibration"),
							NextCalibrationDate = CheckDbNullDateTime(data, "NextCalibrationDate"),
							Status = CheckDbNullString(data, "MeasuringDeviceStatus"),
							Remarks = CheckDbNullString(data, "MeasuringDeviceRemarks"),
							Maker = CheckDbNullString(data, "MeasuringDeviceMaker"),
							Resolution = CheckDbNullString(data, "MeasuringDeviceResolution"),
							DeviceRange = CheckDbNullString(data, "MeasuringDeviceRange"),
							Accuracy = CheckDbNullString(data, "MeasuringDeviceAccuracy"),
							Barcode = CheckDbNullString(data, "MeasuringDeviceBarcode"),
							CalibrationMethod = CheckDbNullString(data, "MeasuringDeviceCalibrationMethod"),
							//AcceptanceCriteria = CheckDbNullString(data, "MeasuringDeviceAcceptanceCriteria"),
							Description = CheckDbNullString(data, "MeasuringDeviceDescription"),
							SerialNo = CheckDbNullString(data, "MeasuringDeviceSerialNo"),
							DeviceType = CheckDbNullInt(data, "DeviceTypeId").Equals(objectCheckerValue) ? null : new DeviceType
							{
								Id = CheckDbNullInt(data, "DeviceTypeId"),
								DeviceTypeName = CheckDbNullString(data, "DeviceTypeName")
							},
							EquipmentSerial = CheckDbNullInt(data, "EquipmentSerialId").Equals(objectCheckerValue) ? null : new EquipmentSerial
							{
								Id = CheckDbNullInt(data, "EquipmentSerialId"),
								SerialNo = CheckDbNullString(data, "EquipmentSerialSerialNo"),
								Equipment = CheckDbNullInt(data, "EquipmentSerialEquipmentId").Equals(objectCheckerValue) ? null : new Equipment
								{
									Id = CheckDbNullInt(data, "EquipmentSerialEquipmentId"),
									EquipmentType = CheckDbNullInt(data, "EquipmentTypeId").Equals(objectCheckerValue) ? null : new EquipmentType
									{
										Id = CheckDbNullInt(data, "EquipmentTypeId"),
										Name = CheckDbNullString(data, "EquipmentTypeName")
									},
									Item = CheckDbNullInt(data, "EquipmentItemId").Equals(objectCheckerValue) ? null : new Item
									{
										Id = CheckDbNullInt(data, "EquipmentItemId"),
										ItemCode = CheckDbNullString(data, "EquipmentItemCode"),
										ShortDescription = CheckDbNullString(data, "EquipmentItemShortDescription"),
										Description = CheckDbNullInt(data, "EquipmentItemDescriptionId").Equals(objectCheckerValue) ? null : new Description
										{
											Id = CheckDbNullInt(data, "EquipmentItemDescriptionId"),
											Text = CheckDbNullString(data, "EquipmentItemDescriptionText")
										}
									},
									
									Manufacturer = CheckDbNullString(data, "EquipmentManufacturer"),
									Model = CheckDbNullString(data, "EquipmentModel"),
									HasAccessory = CheckDbNullBoolean(data, "EquipmentHasAccessory"),
									ApprovalCode = CheckDbNullString(data, "EquipmentApprovalCode"),
									IsPrinted = CheckDbNullBoolean(data, "EquipmentIsPrinted"),
									IsSent = CheckDbNullBoolean(data, "EquipmentIsSent"),
									IsForeignCurrency = CheckDbNullBoolean(data, "EquipmentIsForeignCurrency")
								}
							},
							MachineToolSerial = CheckDbNullInt(data, "MachineToolSerialId").Equals(objectCheckerValue) ? null : new MachineToolSerial
							{
								Id = CheckDbNullInt(data, "MachineToolSerialId"),
								SerialNo = CheckDbNullString(data, "MachineToolSerialNo"),
								ToolLifeUsagePcs = CheckDbNullInt(data, "MachineToolToolUsageLifePcs"),
								Quantity = CheckDbNullInt(data, "MachineToolSerialQuantity"),
								MachineToolStatus = CheckDbNullInt(data, "MachineTooLSerialStatusId").Equals(objectCheckerValue) ? null : new MachineToolStatus
								{
									Id = CheckDbNullInt(data, "MachineTooLSerialStatusId"),
									Status = CheckDbNullString(data, "MachineToolSerialStatusStatus")
								},
								MachineTool = CheckDbNullInt(data, "MachineToolId").Equals(objectCheckerValue) ? null : new MachineTool
								{
									Id = CheckDbNullInt(data, "MachineToolId"),
									Item = CheckDbNullInt(data, "MachineToolItemId").Equals(objectCheckerValue) ? null : new Item
									{
										Id = CheckDbNullInt(data, "MachineToolItemId"),
										ItemCode = CheckDbNullString(data, "MachineToolItemCode"),
										ShortDescription = CheckDbNullString(data, "MachineToolItemShortDescription"),
										Description = CheckDbNullInt(data, "MachineToolItemDescriptionId").Equals(objectCheckerValue) ? null : new Description
										{
											Id = CheckDbNullInt(data, "MachineToolItemDescriptionId"),
											Text = CheckDbNullString(data, "MachineToolItemDescriptionText")
										}
									},
									MachineToolType = CheckDbNullInt(data, "MachineToolTypeId").Equals(objectCheckerValue) ? null : new MachineToolType
									{
										Id = CheckDbNullInt(data, "MachineToolTypeId"),
										Description = CheckDbNullString(data, "MachineToolTypeDescription"),
										ToolTypeName = CheckDbNullString(data, "MachineToolTypeName")
									},
									Description = CheckDbNullString(data, "MachineToolDescription"),
									Note = CheckDbNullString(data, "MachineToolNote"),
									ToolName = CheckDbNullString(data, "MachineToolName"),
									UnitCost = CheckDbNullDecimal(data, "MachineToolUnitCost"),
									ToolLifeUsagePcs = CheckDbNullInt(data, "MachineToolLifeUsagePcs"),
								}


							},
							EndOfLife = CheckDbNullDateTime(data, "EndOfLife"),
							AcceptanceCriteria = CheckDbNullInt(data, "AcceptanceCriteriaId").Equals(objectCheckerValue) ? null : new AcceptanceCriteria
							{
								Id = CheckDbNullInt(data, "AcceptanceCriteriaId"),
								Title = CheckDbNullString(data, "AcceptanceCriteriaTitle"),
								Description = CheckDbNullString(data, "AcceptanceCriteriaDescription")
							},
							_Maker = CheckDbNullInt(data, "MakerId").Equals(objectCheckerValue) ? null : new Maker
							{
								Id = CheckDbNullInt(data, "MakerId"),
								Name = CheckDbNullString(data, "MakerName")
							},
							_Resolution = CheckDbNullInt(data, "ResolutionId").Equals(objectCheckerValue) ? null : new Resolution
							{
								Id = CheckDbNullInt(data, "ResolutionId"),
								Title = CheckDbNullString(data, "ResolutionTitle"),
								Description = CheckDbNullString(data, "ResolutionDescription"),
								ResolutionValue = CheckDbNullString(data, "ResolutionValue"),
							},
							_FrequencyOfCalibration = CheckDbNullInt(data, "FrequencyOfCalibrationId").Equals(objectCheckerValue) ? null : new FrequencyOfCalibration
							{
								Id = CheckDbNullInt(data, "FrequencyOfCalibrationId"),
								Title = CheckDbNullString(data, "FrequencyOfCalibrationTitle"),
								Frequency = CheckDbNullString(data, "FrequencyOfCalibrationFrequency"),
								Description = CheckDbNullString(data, "FrequencyOfCalibrationDescription"),
							},
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

		// 10 night shift

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
					string query = @"INSERT INTO ""General"".""Item"" (""StockingUnitId"", ""CurrencyId"", ""CompanyId"", 
									""DescriptionId"", ""ItemCode"", ""ShortDescription"", 
									""Inactive"", ""IsSold"", ""IsBought"", 
									""IsMfgComponent"", ""IsMfgConsumable"", ""IsObsolete"", ""CreatedOn"")
                                    VALUES(1, 2, 1, @DescriptionId, @ItemCode, @ShortDescription, false, false, false, false, false, false, @CreatedOn)
                                    RETURNING ""Id""";

					command.Connection = connection;
					command.Transaction = transaction;
					command.CommandText = query;

					command.Parameters.AddWithValue("@DescriptionId", itemObj.Description.Id);
					command.Parameters.AddWithValue("@ItemCode", itemObj.ItemCode);
					command.Parameters.AddWithValue("@ShortDescription", itemObj.ShortDescription);
					command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

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
                                    ""Note"", ""ToolName"", ""UnitCost"", ""ToolLifeUsagePcs"", ""CreatedOn"")
                                    VALUES(2, @ItemId, @MachineToolTypeId, @Description, @Note, @ToolName, @UnitCost, @ToolLifeUsagePcs, @CreatedOn)
                                    RETURNING ""Id""";

					command.Connection = connection;
					command.Transaction = transaction;
					command.CommandText = query;

					command.Parameters.AddWithValue("@ItemId", machineTool.Item.Id);
					command.Parameters.AddWithValue("@MachineToolTypeId", machineTool.MachineToolType.Id);
					command.Parameters.AddWithValue("@Description", machineTool.Description);
					command.Parameters.AddWithValue("@Note", machineTool.Note);
					command.Parameters.AddWithValue("@ToolName", machineTool.ToolName);
					command.Parameters.AddWithValue("@UnitCost", machineTool.UnitCost);
					command.Parameters.AddWithValue("@ToolLifeUsagePcs", machineTool.ToolLifeUsagePcs);
					command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

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
					string query = @"INSERT INTO ""Production"".""MachineToolSerial"" (""MachineToolId"", ""SerialNo"",
									""ToolUsageLifePcs"", ""Quantity"", ""CreatedOn"") VALUES
									(@MachineToolId, @SerialNo, @ToolUsageLifePcs, @Quantity, @CreatedOn) RETURNING ""Id""";

					command.Connection = connection;
					command.Transaction = transaction;
					command.CommandText = query;

					command.Parameters.AddWithValue("@MachineToolId", machineToolSerial.MachineTool.Id);
					command.Parameters.AddWithValue("@SerialNo", machineToolSerial.SerialNo);
					command.Parameters.AddWithValue("@ToolUsageLifePcs", machineToolSerial.ToolLifeUsagePcs);
					command.Parameters.AddWithValue("@Quantity", machineToolSerial.Quantity);
					command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

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
										""EndOfLife"", ""AcceptanceCriteriaId"", ""FrequencyOfCalibrationId""
									)
									VALUES (
										1, @PlantId, @DepartmentId, @LocationId,
										@InstrumentSerialId, @UnitId, @FrequencyOfCalibration, @NextCalibrationDate, @Status,
										@Remarks, @Maker, @Resolution, @DeviceRange, @Accuracy, @Barcode,
										@CalibrationMethod, @AcceptanceCriteria, @CreatedOn, @Description,
										@SerialNo, @DeviceTypeId, @EquipmentSerialId, @MachineToolSerialId,
										@EndOfLife, @AcceptanceCriteriaId, @FrequencyOfCalibrationId
									);";

						command.Connection = connection;
						command.CommandText = query;

						command.Parameters.AddWithValue("@PlantId", measuringDevice.Plant.Id);
						command.Parameters.AddWithValue("@DepartmentId", measuringDevice.Department.Id);
						command.Parameters.AddWithValue("@LocationId", measuringDevice.Location.Id);

						command.Parameters.AddWithValue("@UnitId", measuringDevice.Unit.Id);
						command.Parameters.AddWithValue("@FrequencyOfCalibration", measuringDevice._FrequencyOfCalibration.Title);
						command.Parameters.AddWithValue("@NextCalibrationDate", measuringDevice.NextCalibrationDate);
						command.Parameters.AddWithValue("@Status", measuringDevice.Status);
						command.Parameters.AddWithValue("@Remarks", measuringDevice.Remarks);
						command.Parameters.AddWithValue("@Maker", measuringDevice.Maker);
						command.Parameters.AddWithValue("@Resolution", measuringDevice.Resolution);
						command.Parameters.AddWithValue("@DeviceRange", measuringDevice.DeviceRange);
						command.Parameters.AddWithValue("@Accuracy", measuringDevice.Accuracy);
						command.Parameters.AddWithValue("@Barcode", measuringDevice.Barcode);
						command.Parameters.AddWithValue("@CalibrationMethod", measuringDevice.CalibrationMethod);
						command.Parameters.AddWithValue("@AcceptanceCriteria", measuringDevice.AcceptanceCriteria.Title);
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

							measuringDevice.MachineToolSerial = CreateMachineTooLSerialForMeasuringDevice(measuringDevice.MachineToolSerial, transaction, connection);

							command.Parameters.AddWithValue("@MachineToolSerialId", measuringDevice.MachineToolSerial.Id);
						}
						else
							command.Parameters.AddWithValue("@MachineToolSerialId", DBNull.Value);

						// check if the tool to be saved is instrument serial
						var instrumentSerialData = measuringDevice.InstrumentSerial;
						if (instrumentSerialData != null)
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
						command.Parameters.AddWithValue("@AcceptanceCriteriaId", measuringDeviceObj.AcceptanceCriteria.Id);
						command.Parameters.AddWithValue("@FrequencyOfCalibrationId", measuringDeviceObj._FrequencyOfCalibration.Id);

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
						string sql = @" UPDATE ""IMTE"".""MeasuringDevice"" SET
                            ""Version"" = ""Version"" + 1,
                            ""PlantId"" = @PlantId,
                            ""DepartmentId"" = @DepartmentId,
                            ""LocationId"" = @LocationId,
                            ""InstrumentSerialId"" = @InstrumentSerial,
                            ""UnitId"" = @UnitId,
                            ""NextCalibrationDate"" = @NextCalibrationDate,
                            ""Status"" = @Status,
                            ""Remarks"" = @Remarks,
                            ""DeviceRange"" = @DeviceRange,
                            ""Accuracy"" = @Accuracy,
                            ""Barcode"" = @Barcode,
                            ""CalibrationMethod"" = @CalibrationMethod,
                            ""ModifiedOn"" = @ModifiedOn,
                            ""Description"" = @Description,
                            ""SerialNo"" = @SerialNo,
                            ""DeviceTypeId"" = @DeviceTypeId,
                            ""EquipmentSerialId"" = @EquipmentSerialId,
                            ""MachineToolSerialId"" = @MachineToolSerialId,
                            ""EndOfLife"" = @EndOfLife,
                            ""AcceptanceCriteriaId"" = @AcceptanceCriteriaId,
                            ""Maker"" = @Maker,
                            ""Resolution"" = @Resolution,
                            ""FrequencyOfCalibrationId"" = @FrequencyOfCalibrationId                         
                            WHERE ""Id"" = @Id";

						command.CommandText = sql;

						command.Parameters.AddWithValue("@PlantId", measuringDeviceObj.Plant.Id);
						command.Parameters.AddWithValue("@SerialNo", measuringDeviceObj.SerialNo);
						command.Parameters.AddWithValue("@DepartmentId", measuringDeviceObj.Department.Id);
						command.Parameters.AddWithValue("@LocationId", measuringDeviceObj.Location.Id);
						command.Parameters.AddWithValue("@UnitId", measuringDeviceObj.Unit.Id);
						command.Parameters.AddWithValue("@NextCalibrationDate", measuringDeviceObj.NextCalibrationDate);
						command.Parameters.AddWithValue("@Status", measuringDeviceObj.Status);
						command.Parameters.AddWithValue("@Remarks", measuringDeviceObj.Remarks);
						command.Parameters.AddWithValue("@DeviceRange", measuringDeviceObj.DeviceRange);
						command.Parameters.AddWithValue("@Accuracy", measuringDeviceObj.Accuracy);
						command.Parameters.AddWithValue("@Barcode", measuringDeviceObj.Barcode);
						command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceObj.CalibrationMethod);
						command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
						command.Parameters.AddWithValue("@Description", measuringDeviceObj.Description);
						command.Parameters.AddWithValue("@SerialNo", measuringDeviceObj.SerialNo);
						command.Parameters.AddWithValue("@DeviceTypeId", measuringDeviceObj.DeviceType.Id);
						command.Parameters.AddWithValue("@EndOfLife", measuringDeviceObj.EndOfLife);
						command.Parameters.AddWithValue("@AcceptanceCriteriaId", measuringDeviceObj.AcceptanceCriteria.Id);
						command.Parameters.AddWithValue("@Maker", measuringDeviceObj.Maker);
						command.Parameters.AddWithValue("@Resolution", measuringDeviceObj.Resolution);
						command.Parameters.AddWithValue("@FrequencyOfCalibrationId", measuringDeviceObj._FrequencyOfCalibration.Id);

						// updating logic for istrumentserial's instrument, item, and description
						var instrumentSerial = measuringDeviceObj.InstrumentSerial;
						if (instrumentSerial != null)
						{
							// check if instrument serial's instrument's item's description has changed
							// if the id is 0, then the user created a new description, else, user updated the existing item's description or selected a new description
							var instrumentSerialItemDescription = instrumentSerial.Instrument.Item.Description;
							if (instrumentSerialItemDescription.Id != 0)
								descriptionDA.UpdateDescription(instrumentSerialItemDescription, transaction, connection);
							else
								measuringDeviceObj.InstrumentSerial.Instrument.Item.Description =
									CreateDescriptionForItemMeasuringDevice(instrumentSerialItemDescription, transaction, connection);

							// check if instrument serial's instrument's item has changed
							// if the id is 0, then the user created a new item, else, user updated the existing item's description or selected a new description
							var instrumentSerialItem  = measuringDeviceObj.InstrumentSerial.Instrument.Item;
							if (instrumentSerialItem.Id != 0)
								itemDA.UpdateItem(instrumentSerialItem, transaction, connection);
							else
								measuringDeviceObj.InstrumentSerial.Instrument.Item = CreateItemEquipmentForMeasuringDevice(instrumentSerialItem, transaction, connection);

							// check if the user created a new Instrument
							var instrumentSerialInstrument = measuringDeviceObj.InstrumentSerial.Instrument;
							if (instrumentSerialInstrument.Id != 0 && instrumentSerialInstrument.Id != null)
								instrumentDA.UpdateInstrument(instrumentSerialInstrument, transaction, connection);
							
							else
								measuringDeviceObj.InstrumentSerial.Instrument = CreateInstrumentForMeasuringDevice(instrumentSerialInstrument, transaction, connection);

							// this will trigger if the user change the device type of the measuring device
							// will create a new instrument serial
							if (instrumentSerial.Id != 0 && instrumentSerial.Id != null)
								instrumentSerialDA.UpdateInstrumentSerial(measuringDeviceObj.InstrumentSerial, transaction, connection);
							else
								measuringDeviceObj.InstrumentSerial = CreateInstrumentSerialForMeasuringDevice(measuringDeviceObj.InstrumentSerial, transaction, connection);

							command.Parameters.AddWithValue("@InstrumentSerial", measuringDeviceObj.InstrumentSerial.Id);
						}
						else
							command.Parameters.AddWithValue("@InstrumentSerial", DBNull.Value);


						var equipmentSerial = measuringDeviceObj.EquipmentSerial;
						if (equipmentSerial != null)
                        {
							// check if equipment serial's equipment's item's description has changed
							// if the id is 0, then the user created a new description, else, user updated the existing item's description or selected a new description
							var equipmentSerialItemDescription = equipmentSerial.Equipment.Item.Description;
							if (equipmentSerialItemDescription.Id != 0)
								descriptionDA.UpdateDescription(equipmentSerialItemDescription, transaction, connection);
							else
								measuringDeviceObj.EquipmentSerial.Equipment.Item.Description = CreateDescriptionForItemMeasuringDevice(equipmentSerialItemDescription, transaction, connection);

							// check if equipment serial's instrument's item has changed
							// if the id is 0, then the user created a new item, else, user updated the existing item's description or selected a new description
							var equipmentSerialEquipmentItem = measuringDeviceObj.EquipmentSerial.Equipment.Item;
							if (equipmentSerialEquipmentItem.Id != 0)
								itemDA.UpdateItem(equipmentSerialEquipmentItem, transaction, connection);
							else
								measuringDeviceObj.EquipmentSerial.Equipment.Item = CreateItemEquipmentForMeasuringDevice(equipmentSerialEquipmentItem, transaction, connection);

							// chef if the user created a new equipment
							var equipmentSerialEquipment = measuringDeviceObj.EquipmentSerial.Equipment;
							if (equipmentSerialEquipment.Id != 0 && equipmentSerialEquipment.Id != null)
								equipmentDA.UpdateEquipment(equipmentSerialEquipment, transaction, connection);
							else
								measuringDeviceObj.EquipmentSerial.Equipment = CreateEquipmentForMeasuringDevice(equipmentSerialEquipment, transaction, connection);

							// this will trigger if the user change the device type of the measuring device
							// will create a new equipment serial
							if (equipmentSerial.Id != 0 && equipmentSerial.Id != null)
								equipmentSerialDA.UdpateEquipmentSerial(measuringDeviceObj.EquipmentSerial, transaction, connection);
							else
								measuringDeviceObj.EquipmentSerial = CreateEquipmentSerialForMeasuringDevice(measuringDeviceObj.EquipmentSerial, transaction, connection);

							equipmentSerialDA.UdpateEquipmentSerial(measuringDeviceObj.EquipmentSerial, transaction, connection);
							command.Parameters.AddWithValue("@EquipmentSerialId", measuringDeviceObj.EquipmentSerial.Id);
						}
						else
							command.Parameters.AddWithValue("@EquipmentSerialId", DBNull.Value);


						var machineToolSerial = measuringDeviceObj.MachineToolSerial;
						if (machineToolSerial != null)
						{
							// check if machine tool serial's equipment's item's description has changed
							// if the id is 0, then the user created a new description, else, user updated the existing item's description or selected a new description
							var machineToolSerialItemDescription = machineToolSerial.MachineTool.Item.Description;
							if (machineToolSerialItemDescription.Id != 0)
								descriptionDA.UpdateDescription(machineToolSerialItemDescription, transaction, connection);
							else
								measuringDeviceObj.MachineToolSerial.MachineTool.Item.Description = CreateDescriptionForItemMeasuringDevice(machineToolSerialItemDescription, transaction, connection);

							// check if machine tool serial's machine tool's item has changed
							// if the id is 0, then the user created a new item, else, user updated the existing item's description or selected a new description
							var machineToolSerialItem = measuringDeviceObj.MachineToolSerial.MachineTool.Item;
							if (machineToolSerialItem.Id != 0)
								itemDA.UpdateItem(machineToolSerialItem, transaction, connection);
							else
								measuringDeviceObj.MachineToolSerial.MachineTool.Item = CreateItemEquipmentForMeasuringDevice(machineToolSerialItem, transaction, connection);

							// check if user created a new machine tool
							var machineToolSerialMachineTool = measuringDeviceObj.MachineToolSerial.MachineTool;
							if (machineToolSerialMachineTool.Id != 0 && machineToolSerialMachineTool.Id != null)
								machineToolDA.UpdateMachineTool(machineToolSerialMachineTool, transaction, connection);
							else
								measuringDeviceObj.MachineToolSerial.MachineTool = CreateMachineToolForMeasuringDevice(machineToolSerialMachineTool, transaction, connection);

							// this will trigger if the user change the device type of the measuring device
							// will create a new machinetool serial
							if (machineToolSerial.Id != 0 && machineToolSerial.Id != null)
								machineToolSerialDA.UpdateMachineToolSerial(measuringDeviceObj.MachineToolSerial, transaction, connection);
							else
								measuringDeviceObj.MachineToolSerial = CreateMachineTooLSerialForMeasuringDevice(measuringDeviceObj.MachineToolSerial, transaction, connection);

							machineToolSerialDA.UpdateMachineToolSerial(measuringDeviceObj.MachineToolSerial, transaction, connection);
							command.Parameters.AddWithValue("@MachineToolSerialId", measuringDeviceObj.MachineToolSerial.Id);
						}
						else
							command.Parameters.AddWithValue("@MachineToolSerialId", DBNull.Value);

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

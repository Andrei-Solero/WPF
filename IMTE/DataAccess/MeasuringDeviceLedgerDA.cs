using IMTE.IMTEEntity.Models;
using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.IMTEEntity;
using IMTE.Models.Inventory;
using IMTE.Models.Production;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.DataAccess
{
    public class MeasuringDeviceLedgerDA : DataAccessFunctions<MeasuringDeviceLedger>
    {
        private readonly EmployeeDA employeeDA;
        private readonly PlantDA plantDA;
        private readonly DepartmentDA departmentDA;

        public MeasuringDeviceLedgerDA()
        {
            employeeDA = new EmployeeDA();
            plantDA = new PlantDA();
            departmentDA = new DepartmentDA();
        }


        /// <summary>
        /// Will get the distinct values from the MeasuringDeviceLedger and determine which measuring device has ledger
        /// </summary>
        /// <returns>All the measuring devices including if the measuring device has ledger</returns>
        public async Task<IEnumerable<MeasuringDeviceLedger>> GetMeasuringDevicesWithLedgerDetails()
        {
            var output = new List<MeasuringDeviceLedger>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();

                string query = @"
                                    SELECT
                                        DISTINCT mdLedger.""MeasuringDeviceId"" AS ""MeasuringDevice_Ledger"",

                                        -- Measuring Device
                                        md.""Id"" AS ""MDId"", 
                                        md.""Version"" AS ""MDVersion"", 

                                        -- Plant
                                        pl.""Id"" AS ""PlId"", 
                                        pl.""PlantName"",

                                        -- Department
                                        dept.""Id"" AS ""DepartmentId"", 
                                        dept.""DepartmentName"",

                                        -- Location
                                        loc.""Id"" AS ""LocationId"", 
                                        loc.""Name"" AS ""LocationName"",

                                        -- Instrument Serial
                                        instSerial.""Id"" AS ""InstrumentSerialId"",
                                        instSerial.""SerialNo"" AS ""InstrumentSerialSerialNo"",

                                        -- Instrument Serial's Instrument
                                        instSerialInstrument.""Id"" AS ""InstrumentSerialInstrumentId"", 
                                        instType.""Id"" AS ""InstrumentTypeId"", 
                                        instType.""Name"" AS ""InstrumentTypeName"",
                                        instIt.""Id"" AS ""InstrumentItemId"", 
                                        instIt.""ItemCode"" AS ""InstrumentItemCode"", 
                                        instIt.""ShortDescription"" AS ""InstrumentItemShortDescription"",
                                        instDes.""Id"" AS ""InstrumentItemDescriptionId"", 
                                        instDes.""Text"" AS ""InstrumentItemDescriptionText"",
                                        instDept.""Id"" AS ""InstrumentDepartmentId"", 
                                        instDept.""DepartmentName"" AS ""InstrumentDepartmentName"", 
                                        instDept.""Description"" AS ""InstrumentDepartmentDescription"",
                                        instSerialInstrument.""Manufacturer"" AS ""InstrumentManufacturer"", 
                                        instSerialInstrument.""Model"" AS ""InstrumentModel"", 
                                        instSerialInstrument.""HasAccessory"" AS ""InstrumentHasAccessory"", 
                                        instSerialInstrument.""ApprovalCode"" AS ""InstrumentApprovalCode"",
                                        instSerialInstrument.""IsPrinted"" AS ""InstrumentIsPrinted"", 
                                        instSerialInstrument.""IsSent"" AS ""InstrumentIsSent"", 
                                        instSerialInstrument.""IsForeignCurrency"" AS ""InstrumentIsForeignCurrency"",

                                        -- Unit
                                        unit.""Id"" AS ""UnitId"", 
                                        unit.""UnitCategory"", 
                                        unit.""UnitVal"", 
                                        unit.""Description"" AS ""UnitDescription"",

                                        -- Frequency of Calibration
                                        md.""FrequencyOfCalibration"",

                                        -- Next Calibration Date
                                        md.""NextCalibrationDate"",

                                        -- Measuring Device Status
                                        md.""Status"" AS ""MeasuringDeviceStatus"",

                                        -- Remarks
                                        md.""Remarks"" AS ""MeasuringDeviceRemarks"",

                                        -- Maker
                                        md.""Maker"" AS ""MeasuringDeviceMaker"",

                                        -- Resolution
                                        md.""Resolution"" AS ""MeasuringDeviceResolution"",

                                        -- Device Range
                                        md.""DeviceRange"" AS ""MeasuringDeviceRange"",

                                        -- Accuracy
                                        md.""Accuracy"" AS ""MeasuringDeviceAccuracy"",

                                        -- Barcode
                                        md.""Barcode"" AS ""MeasuringDeviceBarcode"",

                                        -- Calibration Method
                                        md.""CalibrationMethod"" AS ""MeasuringDeviceCalibrationMethod"",

                                        -- Acceptance Criteria
                                        md.""AcceptanceCriteria"" AS ""MeasuringDeviceAcceptanceCriteria"",

                                        -- Description
                                        md.""Description"" AS ""MeasuringDeviceDescription"",

                                        -- Serial Number
                                        md.""SerialNo"" AS ""MeasuringDeviceSerialNo"",

                                        -- Device Type
                                        devType.""Id"" AS ""DeviceTypeId"",
                                        devType.""DeviceTypeName"",

                                        -- Equipment Serial
                                        equipSerial.""Id"" AS ""EquipmentSerialId"",
                                        equipSerial.""SerialNo"" AS ""EquipmentSerialSerialNo"",

                                        -- Equipment Serial's Equipment
                                        eq.""Id"" AS ""EquipmentSerialEquipmentId"",
                                        eq.""Manufacturer"" AS ""EquipmentManufacturer"",
                                        eq.""Model"" AS ""EquipmentModel"", 
                                        eq.""HasAccessory"" AS ""EquipmentHasAccessory"", 
                                        eq.""ApprovalCode"" AS ""EquipmentApprovalCode"", 
                                        eq.""IsPrinted"" AS ""EquipmentIsPrinted"", 
                                        eq.""IsSent"" AS ""EquipmentIsSent"", 
                                        eq.""IsForeignCurrency"" AS ""EquipmentIsForeignCurrency"",

                                        -- Equipment Type
                                        eqType.""Id"" AS ""EquipmentTypeId"",
                                        eqType.""Name"" AS ""EquipmentTypeName"",

                                        -- Equipment Item
                                        eqIt.""Id"" AS ""EquipmentItemId"",
                                        eqIt.""ItemCode"" AS ""EquipmentItemCode"",
                                        eqIt.""ShortDescription"" AS ""EquipmentItemShortDescription"", 

                                        -- Equipment Item Description
                                        eqDes.""Id"" AS ""EquipmentItemDescriptionId"",
                                        eqDes.""Text"" AS ""EquipmentItemDescriptionText"",

                                        -- Machine Tool Serial
                                        mtSerial.""Id"" AS ""MachineToolSerialId"",
                                        mtSerial.""SerialNo"" AS ""MachineToolSerialNo"",
                                        mtSerial.""ToolUsageLifePcs"" AS ""MachineToolToolUsageLifePcs"", 
                                        mtSerial.""Quantity"" AS ""MachineToolSerialQuantity"",

                                        -- Machine Tool Serial Status
                                        mtSerialStatus.""Id"" AS ""MachineTooLSerialStatusId"", 
                                        mtSerialStatus.""Status"" AS ""MachineToolSerialStatusStatus"",

                                        -- Machine Tool Serial's Machine Tool
                                        mt.""Id"" AS ""MachineToolId"", 
                                        mtIt.""Id"" AS ""MachineToolItemId"", 
                                        mtIt.""ItemCode"" AS ""MachineToolItemCode"", 
                                        mtIt.""ShortDescription"" AS ""MachineToolItemShortDescription"",
                                        mtDes.""Id"" AS ""MachineToolItemDescriptionId"", 
                                        mtDes.""Text"" AS ""MachineToolItemDescriptionText"",
                                        mtType.""Id"" AS ""MachineToolTypeId"", 
                                        mtType.""Description"" AS ""MachineToolTypeDescription"", 
                                        mtType.""ToolTypeName"" AS ""MachineToolTypeName"",
                                        mt.""Description"" AS ""MachineToolDescription"", 
                                        mt.""Note"" AS ""MachineToolNote"",
                                        mt.""ToolName"" AS ""MachineToolName"", 
                                        mt.""UnitCost"" AS ""MachineToolUnitCost"", 
                                        mt.""ToolLifeUsagePcs"" AS ""MachineToolLifeUsagePcs"",

                                        -- End of Life
                                        md.""EndOfLife"",

                                        -- Acceptance Criteria
                                        acc.""Id"" AS ""AcceptanceCriteriaId"", 
                                        acc.""Title"" AS ""AcceptanceCriteriaTitle"", 
                                        acc.""Description"" AS ""AcceptanceCriteriaDescription"",

                                        -- Maker
                                        mak.""Id"" AS ""MakerId"", 
                                        mak.""Name"" AS ""MakerName"",

                                        -- Resolution
                                        res.""Id"" AS ""ResolutionId"", 
                                        res.""Title"" AS ""ResolutionTitle"", 
                                        res.""Description"" AS ""ResolutionDescription"", 
                                        res.""ResolutionValue"" AS ""ResolutionValue"",

                                        -- Frequency OF Calibration
                                        freq.""Id"" AS ""FrequencyOfCalibrationId"", 
                                        freq.""Title"" AS ""FrequencyOfCalibrationTitle"", 
                                        freq.""Frequency"" AS ""FrequencyOfCalibrationFrequency"", 
                                        freq.""Description"" AS ""FrequencyOfCalibrationDescription""

                                    FROM ""IMTE"".""MeasuringDeviceLedger"" mdLedger

                                    RIGHT OUTER JOIN ""IMTE"".""MeasuringDevice"" md ON mdLedger.""MeasuringDeviceId"" = md.""Id""
                                    -- FROM ""IMTE"".""MeasuringDevice"" md

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

                command.Connection = connection;
                command.CommandText = query;
                object objectCheckerValue = 0;

                var data = await command.ExecuteReaderAsync();

                while (data.Read())
                {
                    output.Add(new MeasuringDeviceLedger
                    {
                        // if id is not equal to null or 0 = the measuring device ledger data is already been issued
                        Id = CheckDbNullInt(data, "MeasuringDevice_Ledger"),
                        MeasuringDevice = CheckDbNullInt(data, "MDId").Equals(objectCheckerValue) ? null : new MeasuringDevice
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
                            _FrequencyOfCalibration = CheckDbNullInt(data, "FrequencyOfCalibrationId").Equals(objectCheckerValue) ? null : new FrequencyOfCalibration
                            {
                                Id = CheckDbNullInt(data, "FrequencyOfCalibrationId"),
                                Title = CheckDbNullString(data, "FrequencyOfCalibrationTitle"),
                                Frequency = CheckDbNullString(data, "FrequencyOfCalibrationFrequency"),
                                Description = CheckDbNullString(data, "FrequencyOfCalibrationDescription"),
                            },
                        }
                    });
                }

            }

            return output;
        }


        /// <summary>
        /// Get all the measuring device ledgers
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MeasuringDeviceLedger>> GetMeasuringDeviceLedgers()
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Get all the measuring device ledgers by Measuring Device
        /// </summary>
        /// <param name="measuringDeviceObj">The measuring device that will to filter</param>
        /// <returns></returns>
        public async Task<IEnumerable<MeasuringDeviceLedger>> GetMeasuringDeviceLedgers(MeasuringDevice measuringDeviceObj)
        {
            var output = new List<MeasuringDeviceLedger>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();

                string query = @"SELECT * 
                                    FROM ""IMTE"".""MeasuringDeviceLedger""
                                    WHERE ""IsDeleted"" = false AND ""MeasuringDeviceId"" = @MeasuringDeviceId";

                command.Connection = connection;
                command.CommandText = query;
                object objectCheckerValue = 0;

                command.Parameters.AddWithValue("@MeasuringDeviceId", measuringDeviceObj.Id);

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(new MeasuringDeviceLedger
                    {
                        Id = CheckDbNullInt(data, "Id"),
                        Remarks = CheckDbNullString(data, "Remarks"),
                        RefNo = CheckDbNullString(data, "RefNo"),
                        DeviceUsage = CheckDbNullInt(data, "DeviceUsage"),
                        DeviceRemainUsage = CheckDbNullInt(data, "DeviceRemainUsage"),
                        TransactionDate = CheckDbNullDateTime(data, "TransactionDate"),
                        IssuedToEmployee = await employeeDA.GetEmployees(new Employee { Id = CheckDbNullInt(data, "IssuedToEmployeeId") }),
                        IssuedByEmployee = await employeeDA.GetEmployees(new Employee { Id = CheckDbNullInt(data, "IssuedByEmployeeId") }),
                        Plant = await plantDA.GetPlant(new Plant { Id = CheckDbNullInt(data, "PlantId") }),
                        TransferToDepartment = await departmentDA.GetDepartments(new Department { Id = CheckDbNullInt(data, "TransferToDepartmentId") })
                    });
                }
            }

            return output;
        }

        public void CreateMeasuringDeviceLedger(MeasuringDeviceLedger measuringDeviceLedger)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                if (measuringDeviceLedger != null)
                {
                    connection.Open();
                    using (NpgsqlTransaction transaction = connection.BeginTransaction())
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        try
                        {
                            string query = @"INSERT INTO ""IMTE"".""MeasuringDeviceLedger"" 
                                                (""IssuedToEmployeeId"", ""IssuedByEmployeeId"",
                                                ""PlantId"", ""TransferToDepartmentId"",
                                                ""MeasuringDeviceId"", ""DeviceStatusId"",
                                                ""TransactionDate"", ""Remarks"",
                                                ""CreatedOn"", ""RefNo"",
                                                ""DeviceUsage"", ""DeviceRemainUsage"") 
                                                VALUES
                                                (@IssuedToEmployeeId, @IssuedByEmployeeId,
                                                @PlantId, @TransferToDepartmentId,
                                                @MeasuringDeviceId, @DeviceStatusId,
                                                @TransactionDate, @Remarks,
                                                @CreatedOn, @RefNo,
                                                @DeviceUsage, @DeviceRemainUsage);";

                            command.Connection = connection;
                            command.CommandText = query;

                            command.Parameters.AddWithValue("@IssuedToEmployeeId", measuringDeviceLedger.IssuedToEmployee.Id);
                            command.Parameters.AddWithValue("@IssuedByEmployeeId", measuringDeviceLedger.IssuedByEmployee.Id);
                            command.Parameters.AddWithValue("@PlantId", measuringDeviceLedger.Plant.Id);
                            command.Parameters.AddWithValue("@TransferToDepartmentId", measuringDeviceLedger.TransferToDepartment.Id);
                            command.Parameters.AddWithValue("@MeasuringDeviceId", measuringDeviceLedger.MeasuringDevice.Id);
                            command.Parameters.AddWithValue("@DeviceStatusId", measuringDeviceLedger.DeviceStatus.Id);
                            command.Parameters.AddWithValue("@TransactionDate", measuringDeviceLedger.TransactionDate);
                            command.Parameters.AddWithValue("@Remarks", measuringDeviceLedger.Remarks);
                            command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);
                            command.Parameters.AddWithValue("@RefNo", measuringDeviceLedger.RefNo);
                            command.Parameters.AddWithValue("@DeviceUsage", measuringDeviceLedger.DeviceUsage);
                            command.Parameters.AddWithValue("@DeviceRemainUsage", measuringDeviceLedger.DeviceRemainUsage);

                            command.ExecuteNonQuery();
                            transaction.Commit();

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occured {ex.Message} - {ex.StackTrace}");
                        }
                    }
                }
            }
        }

    }
}

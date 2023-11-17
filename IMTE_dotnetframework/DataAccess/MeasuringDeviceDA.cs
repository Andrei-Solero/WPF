using IMTE_dotnetframework.Models;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE_dotnetframework.DataAccess
{
    public class MeasuringDeviceDA : DataAccessConnection<MeasuringDevices>
    {
        public void AddNewMeasuringDevice(MeasuringDevices measuringDeviceObj)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO ""IMTE"".""MeasuringDevices"" 
                                        (""Version"", ""Type"", ""FrequencyOfCalibration"", ""LastCalibrationDate"",
                                        ""ResultOfCalibration"", ""NextCalibrationDate"", ""Status"",
                                        ""CalibrationRemarks"", ""TrgTpgAndSettingRemarks"", ""Remarks"",
                                        ""Date"", ""Maker"", ""Resolution"", ""DeviceRange"", ""Accuracy"",
                                        ""UnitOfMeasurement"", ""Barcode"", ""CalibrationMethod"") 
                                        VALUES (@Version, @Type, @FrequencyOfCalibration, @LastCalibrationDate, 
                                        @ResultOfCalibration, @NextCalibrationDate, @Status, @CalibrationRemarks,
                                        @TrgTpgAndSettingsRemark, @Remarks, @Date, @Maker, @Resolution, @DeviceRange,
                                        @Accuracy, @UnitOfMeasurement, @Barcode, @CalibrationMethod)";

                command.Parameters.AddWithValue("@Version", measuringDeviceObj.Version);
                command.Parameters.AddWithValue("@Type", measuringDeviceObj.Type);
                command.Parameters.AddWithValue("@FrequencyOfCalibration", measuringDeviceObj.FrequencyOfCalibration);
                command.Parameters.AddWithValue("@LastCalibrationDate", measuringDeviceObj.LastCalibrationDate);
                command.Parameters.AddWithValue("@NextCalibrationDate", measuringDeviceObj.NextCalibrationDate);
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
                command.Parameters.AddWithValue("@UnitOfMeasurement", measuringDeviceObj.UnitOfMeasurement);
                command.Parameters.AddWithValue("@Barcode", measuringDeviceObj.Barcode);
                command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceObj.CalibrationMethod);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateSelectedMeasuringDevice(MeasuringDevices measuringDeviceObj)
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

                command.Parameters.AddWithValue("@Version", measuringDeviceObj.Version);
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
                command.Parameters.AddWithValue("@UnitOfMeasurement", measuringDeviceObj.UnitOfMeasurement);
                command.Parameters.AddWithValue("@Barcode", measuringDeviceObj.Barcode);
                command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceObj.CalibrationMethod);
                command.Parameters.AddWithValue("@Id", measuringDeviceObj.Id);

                command.ExecuteNonQuery();
            }
        }

        public ObservableCollection<MeasuringDevices> GetAllMeasuringDevices()
        {
            var output = new ObservableCollection<MeasuringDevices>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM ""IMTE"".""MeasuringDevices""
                                        WHERE ""IsDeleted"" = false";

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    string outputJson = SerializeDataToJSON(data);
                    MeasuringDevices device = DeserializeJSONToObj(outputJson);

                    output.Add(device);
                }
            }

            return output;
        }

        public void DeleteMeasuringDevice(MeasuringDevices measuringDevicesObj)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE ""IMTE"".""MeasuringDevices"" SET 
                                        ""IsDeleted"" = True
                                        WHERE ""Id"" = @Id";

                command.Parameters.AddWithValue("@Id", measuringDevicesObj.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}


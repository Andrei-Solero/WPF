using Dapper;
using IMTE.General.Models;
using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.IMTEEntity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class MeasuringDeviceCertificateDA : DataAccessFunctions<MeasuringDeviceCertificates>
    {
        public async Task CreateMeasuringDeviceCertificateAsync(MeasuringDeviceCertificates measuringDeviceCertificatesObj)
        {
            string query = @"
                                INSERT INTO ""IMTE"".""MeasuringDeviceCertificates""
                                (
                                    ""MeasuringDeviceId"",
                                    ""CompanyId"",
                                    ""CalibrationCertificate"",
                                    ""CreatedOn"",
                                    ""CalibratedByEmployeeId"",
                                    ""CalibrationMethod"",
                                    ""CalibratedOn"",
                                    ""Remarks"",
                                    ""AcceptanceCriteria"",
                                    ""CalibrationResult""
                                )
                                VALUES
                                (
                                    @MeasuringDeviceId,
                                    2,
                                    @CalibrationCertificate,
                                    @CreatedOn,
                                    @CalibratedByEmployeeId,
                                    @CalibrationMethod,
                                    @CalibratedOn,
                                    @Remarks,
                                    @AcceptanceCriteria,
                                    @CalibrationResult
                                );
                            ";

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                var parameters = new
                {
                    MeasuringDeviceId = measuringDeviceCertificatesObj.MeasuringDevice.Id,
                    CalibrationCertificate = measuringDeviceCertificatesObj.CalibrationCerticate,
                    CreatedOn = DateTime.Now,
                    CalibratedByEmployeeId = measuringDeviceCertificatesObj.CalibratedByEmployee.Id,
                    CalibrationMethod = measuringDeviceCertificatesObj.CalibrationMethod,
                    CalibratedOn = measuringDeviceCertificatesObj.CalibratedOn,
                    Remarks = measuringDeviceCertificatesObj.Remarks,
                    AcceptanceCriteria = measuringDeviceCertificatesObj.AcceptanceCriteria,
                    CalibrationResult = measuringDeviceCertificatesObj.CalibrationResult
                };

                await connection.ExecuteAsync(query, parameters);
            }

            #region AdoNET
            //using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            //using (NpgsqlCommand command = new NpgsqlCommand())
            //{
            //    await connection.OpenAsync();



            //    command.Parameters.AddWithValue("@MeasuringDeviceId", measuringDeviceCertificatesObj.MeasuringDevice.Id);
            //    command.Parameters.AddWithValue("@CalibrationCertificate", measuringDeviceCertificatesObj.CalibrationCerticate);
            //    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
            //    command.Parameters.AddWithValue("@CalibratedByEmployeeId", measuringDeviceCertificatesObj.CalibratedByEmployee.Id);
            //    command.Parameters.AddWithValue("@CalibrationMethod", measuringDeviceCertificatesObj.CalibrationMethod);
            //    command.Parameters.AddWithValue("@CalibratedOn", measuringDeviceCertificatesObj.CalibratedOn);
            //    command.Parameters.AddWithValue("@Remarks", measuringDeviceCertificatesObj.Remarks);
            //    command.Parameters.AddWithValue("@AcceptanceCriteria", measuringDeviceCertificatesObj.AcceptanceCriteria);
            //    command.Parameters.AddWithValue("@CalibrationResult", measuringDeviceCertificatesObj.CalibrationResult);

            //    command.Connection = connection;
            //    command.CommandText = query;

            //    await command.ExecuteNonQueryAsync();
            //}
            #endregion

        }

        /// <summary>
        /// Return all the measuring device certificates by measuring device ID
        /// </summary>
        /// <param name="measuringDevice"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MeasuringDeviceCertificates>> GetMeasuringDeviceCertificatesAsync(MeasuringDevice measuringDevice)
        {
            var output = new List<MeasuringDeviceCertificates>();
            string query = @"
                            SELECT 
                                cert.""MeasuringDeviceId"",
                                cert.""Id"" AS ""MeasuringDeviceCertificateId"",
                                cert.""CalibrationCertificate"", 
                                emp.""Id"" AS ""EmployeeId"", 
                                dept.""Id"" AS ""DepartmentId"", 
                                dept.""DepartmentName"", 
                                et.""Id"" AS ""EmployeeTypeId"", et.""TypeName"" AS ""EmployeeType"", 
                                person.""Id"" AS ""PersonId"", person.""First"", person.""Last"", person.""Middle"", 
                                emp.""EmployeeNo"",
                                pos.""Id"" AS ""PositionId"", pos.""PositionName"" AS ""JobPosition"",
                                cert.""CalibrationMethod"",
                                cert.""CalibratedOn"",
                                cert.""UsageNoOnCalibration"",
                                cert.""NextCalibrationDate"",
                                cert.""Remarks"",
                                cert.""AcceptanceCriteria"",
                                cert.""CalibrationResult""
                            FROM ""IMTE"".""MeasuringDeviceCertificates"" cert
                            LEFT OUTER JOIN ""HumanResources"".""Employee"" emp ON cert.""CalibratedByEmployeeId"" = emp.""Id""
                            LEFT OUTER JOIN ""HumanResources"".""EmployeeType"" et ON emp.""EmployeeTypeId"" = et.""Id""
                            LEFT OUTER JOIN ""General"".""Department"" dept ON emp.""PrimaryDepartmentId"" = dept.""Id""
                            LEFT OUTER JOIN ""General"".""Person"" person ON emp.""PersonId"" = person.""Id""
                            LEFT OUTER JOIN ""HumanResources"".""Position"" pos ON emp.""PositionId"" = pos.""Id""
                            WHERE cert.""MeasuringDeviceId"" = @MeasuringDeviceID;";

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = query;

                command.Parameters.AddWithValue("@MeasuringDeviceID", measuringDevice.Id);

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    output.Add(MeasuringDeviceCertificatesMapping(data));
                }
            }

            return output;
        }

        public MeasuringDeviceCertificates MeasuringDeviceCertificatesMapping(NpgsqlDataReader data)
        {
            object checkerValue = 0;

            return new MeasuringDeviceCertificates
            {
                Id = CheckDbNullInt(data, "MeasuringDeviceCertificateId"),
                CalibrationCerticate = (byte[])CheckDbNullObject(data, "CalibrationCertificate"),
                CalibratedByEmployee = CheckDbNullInt(data, "EmployeeId").Equals(checkerValue) ? null : new Employee
                {
                    Id = CheckDbNullInt(data, "EmployeeId"),
                    PrimaryDepartment = CheckDbNullInt(data, "DepartmentId").Equals(checkerValue) ? null : new Department
                    {
                        Id = CheckDbNullInt(data, "DepartmentId"),
                        DepartmentName = CheckDbNullString(data, "DepartmentName")
                    },
                    EmployeeType = CheckDbNullInt(data, "EmployeeTypeId").Equals(checkerValue) ? null : new EmployeeType
                    {
                        Id = CheckDbNullInt(data, "EmployeeTypeId"),
                        TypeName = CheckDbNullString(data, "EmployeeType")
                    },
                    Person = CheckDbNullInt(data, "PersonId").Equals(checkerValue) ? null : new Person
                    {
                        Id = CheckDbNullInt(data, "PersonId"),
                        First = CheckDbNullString(data, "First"),
                        Last = CheckDbNullString(data, "Last"),
                        Middle = CheckDbNullString(data, "Middle"),
                    },
                    EmployeeNo = CheckDbNullString(data, "EmployeeNo")
                },
                CalibrationMethod = CheckDbNullString(data, "CalibrationMethod"),
                CalibratedOn = CheckDbNullDateTime(data, "CalibratedOn"),
                UsageNoOnCalibration = CheckDbNullInt(data, "UsageNoOnCalibration"),
                NextCalibrationDate = CheckDbNullDateTime(data, "NextCalibrationDate"),
                Remarks = CheckDbNullString(data, "Remarks"),
                AcceptanceCriteria = CheckDbNullString(data, "AcceptanceCriteria"),
                CalibrationResult = CheckDbNullString(data, "CalibrationResult"),
            };
        }

    }
}

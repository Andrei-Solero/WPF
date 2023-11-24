using IMTE.General.Models;
using IMTE.Models.HumanResources;
using Npgsql;
using System;
using System.Collections.Generic;

namespace IMTE.DataAccess
{
    public class EmployeeDA : DataAccessFunctions<Employee>
    {
        public IEnumerable<Employee> GetAllEmployees()
        {
            var output = new List<Employee>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                                        SELECT 
                                            emp.""Id"", 
                                            dept.""DepartmentName"", 
                                            et.""Id"" AS ""EmployeeTypeId"", et.""TypeName"" AS ""EmployeeType"", 
                                            person.""Id"" AS ""PersonId"", person.""First"", person.""Last"", person.""Middle"", 
                                            emp.""EmployeeNo"",
                                            pos.""Id"" AS ""PositionId"", pos.""PositionName"" AS ""JobPosition""
                                        FROM ""HumanResources"".""Employee"" emp
                                        LEFT OUTER JOIN ""HumanResources"".""EmployeeType"" et ON emp.""EmployeeTypeId"" = et.""Id""
                                        LEFT OUTER JOIN ""General"".""Department"" dept ON emp.""PrimaryDepartmentId"" = dept.""Id""
                                        LEFT OUTER JOIN ""General"".""Person"" person ON emp.""PersonId"" = person.""Id""
                                        LEFT OUTER JOIN ""HumanResources"".""Position"" pos ON emp.""PositionId"" = pos.""Id""";

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    Employee employee = new Employee
                    {
                        Id = CheckDbNullInt(data, "Id"),
                        EmployeeType = new EmployeeType
                        {
                            Id = CheckDbNullInt(data, "EmployeeTypeId"),
                            TypeName = CheckDbNullString(data, "EmployeeType"),
                        },
                        Position = new Position
                        {
                            Id = CheckDbNullInt(data, "PositionId"),
                            PositionName = CheckDbNullString(data, "JobPosition"),
                        },
                        Person = new Person
                        {
                            Id = CheckDbNullInt(data, "PersonId"),
                            First = CheckDbNullString(data, "First"),
                            Last = CheckDbNullString(data, "Last"),
                            Middle = CheckDbNullString(data, "Middle")
                        },
                        EmployeeNo = data.GetString(data.GetOrdinal("EmployeeNo")),

                        #region Other Fields (Null)
                        //EmergencyContactPersonId = !data.IsDBNull(data.GetOrdinal("EmergencyContactPersonId")) ? data.GetInt32(data.GetOrdinal("EmergencyContactPersonId")) : 0,
                        //EducationId = !data.IsDBNull(data.GetOrdinal("EducationId")) ? data.GetInt32(data.GetOrdinal("EducationId")) : 0,

                        //ModifiedByEmployeeId = data.GetInt32(data.GetOrdinal("ModifiedByEmployeeId")),
                        //PrimaryDepartmentId = data.GetInt32(data.GetOrdinal("PrimaryDepartmentId")),

                        //SpecificationId = !data.IsDBNull(data.GetOrdinal("SpecificationId")) ? data.GetInt32(data.GetOrdinal("SpecificationId")) : 0,

                        //HiredDate = data.GetDateTime(data.GetOrdinal("HiredDate")),
                        //StartedDate = data.GetDateTime(data.GetOrdinal("StartedDate")),
                        //ProbationEndDate = data.GetDateTime(data.GetOrdinal("ProbationEndDate")),
                        //TerminatedDate = data.GetDateTime(data.GetOrdinal("TerminatedDate")),
                        ////LastEvaluation = data.GetDateTime(data.GetOrdinal("LastEvaluation")),
                        ////NextEvaluation = data.GetDateTime(data.GetOrdinal("NextEvaluation")),
                        //TaxIdentificationNo = data.GetString(data.GetOrdinal("TaxIdentificationNo")),
                        ////CreatedDate = data.GetDateTime(data.GetOrdinal("CreatedDate")),
                        //IsDeleted = data.GetBoolean(data.GetOrdinal("IsDeleted")),
                        //CreatedOn = data.GetDateTime(data.GetOrdinal("CreatedOn")),
                        //ModifiedOn = data.GetDateTime(data.GetOrdinal("ModifiedOn")),
                        ////LastRaise = data.GetDateTime(data.GetOrdinal("LastRaise")),
                        //IsTerminated = data.GetBoolean(data.GetOrdinal("IsTerminated")),
                        //IsInactive = data.GetBoolean(data.GetOrdinal("IsInactive")),
                        //AssignedLocationId = data.GetInt32(data.GetOrdinal("AssignedLocationId")),
                        //SectyClId = data.GetInt32(data.GetOrdinal("SectyClId")),
                        ////ResignedDate = data.GetDateTime(data.GetOrdinal("ResignedDate")),
                        //Guid = data.GetGuid(data.GetOrdinal("Guid")),
                        //LaborTypeId = data.GetInt32(data.GetOrdinal("LaborTypeId")),
                        //PlantId = data.GetInt32(data.GetOrdinal("PlantId")),
                        ////ClearanceDate = data.GetDateTime(data.GetOrdinal("ClearanceDate")),
                        //AgencyId = data.GetInt32(data.GetOrdinal("AgencyId")),
                        //AccountGroupId = data.GetInt32(data.GetOrdinal("AccountGroupId"))
                        #endregion
                    };

                    output.Add(employee);

                    #region Mapped Data
                    //EmployeeType employeeType = new EmployeeType
                    //{
                    //    Id = data.GetInt32(data.GetOrdinal("EmployeeType_Id")),
                    //    Version = data.GetInt32(data.GetOrdinal("EmployeeType_Version")),
                    //    TypeName = data.GetString(data.GetOrdinal("EmployeeType")),
                    //    IsEmployee = data.GetBoolean(data.GetOrdinal("EmployeeType_IsEmployee")),
                    //    HasBenefits = data.GetBoolean(data.GetOrdinal("EmployeeType_HasBenefits")),
                    //    CreatedDate = data.GetDateTime(data.GetOrdinal("EmployeeType_CreatedDate")),
                    //    IsDeleted = data.GetBoolean(data.GetOrdinal("EmployeeType_IsDeleted")),
                    //    CreatedOn = data.GetDateTime(data.GetOrdinal("EmployeeType_CreatedOn")),
                    //    ModifiedOn = data.GetDateTime(data.GetOrdinal("EmployeeType_ModifiedOn"))
                    //};

                    //Person person = new Person
                    //{
                    //    Id = data.GetInt32(data.GetOrdinal("Person_Id")),
                    //    First = data.GetString(data.GetOrdinal("First")),
                    //    Last = data.GetString(data.GetOrdinal("Last")),
                    //    Middle = data.GetString(data.GetOrdinal("Middle"))
                    //};

                    //Position position = new Position
                    //{
                    //    Id = data.GetInt32(data.GetOrdinal("Position_Id")),
                    //    Version = data.GetInt32(data.GetOrdinal("Position_Version")),
                    //    CompanyId = data.GetInt32(data.GetOrdinal("Position_CompanyId")),
                    //    PositionName = data.GetString(data.GetOrdinal("JobPosition")),
                    //    DutiesDescription = data.GetString(data.GetOrdinal("Position_DutiesDescription")),
                    //    CreatedDate = data.GetDateTime(data.GetOrdinal("Position_CreatedDate")),
                    //    IsDeleted = data.GetBoolean(data.GetOrdinal("Position_IsDeleted")),
                    //    CreatedOn = data.GetDateTime(data.GetOrdinal("Position_CreatedOn")),
                    //    ModifiedOn = data.GetDateTime(data.GetOrdinal("Position_ModifiedOn")),
                    //    SuggestMinLvlSectyClId = data.GetInt32(data.GetOrdinal("Position_SuggestMinLvlSectyClId"))
                    //};
                    #endregion

                }

            }

            return output;
        }
        
        public Person CreatePersonForEmployee(Person personObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            Person output = personObj;
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                try
                {
                    string query = @"INSERT INTO ""General"".""Person""(""First"", ""Last"", ""Middle"",
                                    ""Birthdate"") VALUES (@First, @Last, @Middle, @Birthdate)
                                    RETURNING ""Id""";

                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@First", personObj.First);
                    command.Parameters.AddWithValue("@Last", personObj.Last);
                    command.Parameters.AddWithValue("@Middle", personObj.Middle);
                    command.Parameters.AddWithValue("@Birthdate", personObj.Birthdate);

                    personObj.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return output;
        }


        public Employee CreateEmployee(Employee empObj)
        {
            Employee output = empObj;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    try
                    {
                        string query = @"INSERT INTO ""HumanResources"".""Employee"" (""CompanyId"", ""EmployeeNo"", ""EmployeeTypeId"", ""PositionId"",
                                    ""PrimaryDepartmentId"", ""PersonId"") VALUES (2, @EmployeeNo, @EmployeeTypeId, @PositionId,
                                    @PrimaryDepartmentId, @PersonId)";

                        command.Connection = connection;
                        command.CommandText = query;

                        Person person = CreatePersonForEmployee(empObj.Person, transaction, connection);

                        command.Parameters.AddWithValue("@EmployeeNo", empObj.EmployeeNo);
                        command.Parameters.AddWithValue("@EmployeeTypeId", empObj.EmployeeType.Id);
                        command.Parameters.AddWithValue("@PositionId", empObj.Position.Id);
                        command.Parameters.AddWithValue("@PrimaryDepartmentId", empObj.PrimaryDepartment.Id);
                        command.Parameters.AddWithValue("@PersonId", empObj.Person.Id);

                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return output;
        }
    }
}


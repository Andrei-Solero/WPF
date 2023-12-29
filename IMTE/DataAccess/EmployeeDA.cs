using Dapper;
using IMTE.General.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class EmployeeDA : DataAccessFunctions<Employee>
    {
        /// <summary>
        /// Will return all employees
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Employee>> GetEmployees()
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

                var data = await command.ExecuteReaderAsync();

                while (await data.ReadAsync())
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
                        PrimaryDepartment = new Department
                        {

                        }
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

        /// <summary>
        /// Dapper: Get all the employees
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            // TODO: LEARN DAPPER
            var output = new List<Employee>();
            string query = @"SELECT 
                            emp.""Id"", emp.""EmployeeNo"", 
                            dep.""Id"", dep.""DepartmentName"",
                            et.""Id"", et.""TypeName"",
                            pos.""Id"", pos.""PositionName"",
                            p.""Id"", p.""First"", p.""Last"", p.""Middle""
                            FROM ""HumanResources"".""Employee"" emp
                            LEFT OUTER JOIN ""General"".""Department"" dep ON emp.""PrimaryDepartmentId"" = dep.""Id""
                            LEFT OUTER JOIN ""General"".""Person"" p ON emp.""PersonId"" = p.""Id""
                            LEFT OUTER JOIN ""HumanResources"".""EmployeeType"" et ON emp.""EmployeeTypeId"" = et.""Id""
                            LEFT OUTER JOIN ""HumanResources"".""Position"" pos ON emp.""PositionId"" = pos.""Id""";

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var a = await connection.QueryAsync<Employee, Department, Person, EmployeeType, Position, Employee>(query,
                    (employee, department, person, employeeType, position) =>
                    {
                        employee.PrimaryDepartment = department;
                        employee.Person = person;
                        employee.EmployeeType = employeeType;
                        employee.Position = position;

                        return employee;
                    }, splitOn: "Id");

                output = a.ToList();
            }

            return output;
        }

        public async Task<Person> CreatePersonForEmployeeAsync(Person personObj, IDbConnection connection, IDbTransaction transaction)
        {
            var output = personObj;

            string query = @"INSERT INTO ""General"".""Person""
                                (""First"", 
                                ""Last"", 
                                ""Middle"",
                                ""Birthdate"", 
                                ""CreatedDate"", 
                                ""CreatedOn"") VALUES 
                                (@First, 
                                @Last,
                                @Middle, 
                                @Birthdate, 
                                @CreatedDate, 
                                @CreatedOn)
                                RETURNING ""Id""";

            var parameter = new
            {
                First = output.First,
                Last = output.Last,
                Middle = output.Middle,
                Birthdate = output.Birthdate,
                CreatedDate = DateTime.Now,
                CreatedOn = DateTime.Now,
            };

            var executeOutput = await connection.ExecuteScalarAsync(query, parameter, transaction);

            output.Id = int.Parse(executeOutput.ToString());

            return output;
        }


        /// <summary>
        /// Will save an employee to the database (CompanyId and SectyClId value on the query is manually set)
        /// </summary>
        /// <param name="employeeObj"></param>
        /// <returns></returns>
        public async Task CreateEmployeeAsync(Employee employeeObj)
        {
            var output = employeeObj;

            string query = @"
                            INSERT INTO ""HumanResources"".""Employee"" 
                            (
                                ""CompanyId"", 
                                ""EmployeeNo"", 
                                ""EmployeeTypeId"", 
                                ""PositionId"",
                                ""PrimaryDepartmentId"", 
                                ""PersonId"", 
                                ""CreatedOn"",
                                ""SectyClId""
                            ) 
                            VALUES 
                            (
                                2, 
                                @EmployeeNo, 
                                @EmployeeTypeId, 
                                @PositionId,
                                @PrimaryDepartmentId, 
                                @PersonId, 
                                @CreatedOn,
                                @SectyClId
                            )";

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        output.Person = await CreatePersonForEmployeeAsync(output.Person, connection, transaction);

                        object[] parameters = { new {
                                            EmployeeNo = output.EmployeeNo,
                                            EmployeeTypeId = output.EmployeeType.Id,
                                            PositionId = output.Position.Id,
                                            PrimaryDepartmentId = output.PrimaryDepartment.Id,
                                            PersonId = output.Person.Id,
                                            CreatedOn = DateTime.Now,
                                            SectyClId = 1,
                                        }};

                        await connection.ExecuteAsync(query, parameters);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Will return employee by id
        /// </summary>
        /// <param name="employee">Employee that will be use to filter</param>
        /// <returns></returns>
        public async Task<Employee> GetEmployees(Employee employeeObj)
        {
            var output = new Employee();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"
                                        SELECT 
                                            emp.""Id"" AS ""EmployeeId"", 
                                            dept.""DepartmentName"", 
                                            et.""Id"" AS ""EmployeeTypeId"", et.""TypeName"" AS ""EmployeeType"", 
                                            person.""Id"" AS ""PersonId"", person.""First"", person.""Last"", person.""Middle"", 
                                            emp.""EmployeeNo"",
                                            pos.""Id"" AS ""PositionId"", pos.""PositionName"" AS ""JobPosition""
                                        FROM ""HumanResources"".""Employee"" emp
                                        LEFT OUTER JOIN ""HumanResources"".""EmployeeType"" et ON emp.""EmployeeTypeId"" = et.""Id""
                                        LEFT OUTER JOIN ""General"".""Department"" dept ON emp.""PrimaryDepartmentId"" = dept.""Id""
                                        LEFT OUTER JOIN ""General"".""Person"" person ON emp.""PersonId"" = person.""Id""
                                        LEFT OUTER JOIN ""HumanResources"".""Position"" pos ON emp.""PositionId"" = pos.""Id""
                                        WHERE emp.""IsDeleted"" = false AND emp.""Id"" = @EmpId";

                command.Parameters.AddWithValue("@EmpId", employeeObj.Id);

                var data = await command.ExecuteReaderAsync();

                if (await data.ReadAsync())
                {
                    output.Id = CheckDbNullInt(data, "EmployeeId");
                    output.EmployeeNo = CheckDbNullString(data, "EmployeeNo");
                    output.PrimaryDepartment = new Department
                    {
                        DepartmentName = CheckDbNullString(data, "DepartmentName")
                    };
                    output.EmployeeType = new EmployeeType
                    {
                        Id = CheckDbNullInt(data, "EmployeeTypeId"),
                        TypeName = CheckDbNullString(data, "EmployeeType")
                    };
                    output.Person = new Person
                    {
                        Id = CheckDbNullInt(data, "PersonId"),
                        First = CheckDbNullString(data, "First"),
                        Last = CheckDbNullString(data, "Last"),
                        Middle = CheckDbNullString(data, "Middle"),
                    };
                    output.Position = new Position
                    {
                        Id = CheckDbNullInt(data, "PositionId"),
                        PositionName = CheckDbNullString(data, "JobPosition"),

                    };
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
                                    ""Birthdate"", ""CreatedDate"", ""CreatedOn"") VALUES (@First, @Last, @Middle, @Birthdate, @CreatedDate, @CreatedOn)
                                    RETURNING ""Id""";

                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@First", personObj.First);
                    command.Parameters.AddWithValue("@Last", personObj.Last);
                    command.Parameters.AddWithValue("@Middle", personObj.Middle);
                    command.Parameters.AddWithValue("@Birthdate", personObj.Birthdate);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

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
                                    ""PrimaryDepartmentId"", ""PersonId"", ""CreatedDate"", ""CreatedOn"") VALUES (2, @EmployeeNo, @EmployeeTypeId, @PositionId,
                                    @PrimaryDepartmentId, @PersonId, @CreatedDate, @CreatedOn)";

                        command.Connection = connection;
                        command.CommandText = query;

                        Person person = CreatePersonForEmployee(empObj.Person, transaction, connection);

                        command.Parameters.AddWithValue("@EmployeeNo", empObj.EmployeeNo);
                        command.Parameters.AddWithValue("@EmployeeTypeId", empObj.EmployeeType.Id);
                        command.Parameters.AddWithValue("@PositionId", empObj.Position.Id);
                        command.Parameters.AddWithValue("@PrimaryDepartmentId", empObj.PrimaryDepartment.Id);
                        command.Parameters.AddWithValue("@PersonId", empObj.Person.Id);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);

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


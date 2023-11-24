using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public class DepartmentDA : DataAccessFunctions<Department>
    {
        public IEnumerable<Department> GetAllDepartments()
        {
            var output = new List<Department>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT ""dep"".*, ""com"".""CompanyName"" 
                                        FROM ""General"".""Department"" dep
                                        INNER JOIN ""General"".""Company"" com ON ""dep"".""CompanyId"" = ""com"".""Id""";

                var data = command.ExecuteReader();

                while (data.Read())
                {
                    output.Add(new Department 
                    {
                        Id = CheckDbNullInt(data, "Id"),
                        DepartmentName = CheckDbNullString(data, "DepartmentName"),
                        Company = new Company
                        {
                            CompanyName = CheckDbNullString(data, "CompanyName")
                        },
                        Description = CheckDbNullString(data, "Description")
                    });
                }
            }

            return output;
        }

        public Department CreateDepartment(Department departmentObj)
        {
            Department output = departmentObj;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    try
                    {
                        string query = @"INSERT INTO ""General"".""Department""(""CompanyId"", ""DepartmentName"", ""Description"")
                                    VALUES(2, @DepartmentName, @Description)
                                    RETURNING ""Id""";


                        command.Connection = connection;
                        command.CommandText = query;

                        command.Parameters.AddWithValue("@DepartmentName", departmentObj.DepartmentName);
                        command.Parameters.AddWithValue("@Description", departmentObj.Description);

                        output.Id = Convert.ToInt32(command.ExecuteScalar());

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

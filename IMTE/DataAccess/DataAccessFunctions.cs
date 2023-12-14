using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
    public abstract class DataAccessFunctions<T> where T : class
    {
        protected string ConnectionString
        {
            get
            {
                return "Host=localhost;Username=postgres;Password=admin;Database=VisionTestDB-12/07";
            }
        }

        protected int CheckDbNullInt(NpgsqlDataReader data, string columnName)
        {
            int columnIndex = data.GetOrdinal(columnName);
            if (data.IsDBNull(columnIndex))
            {
                return 0;
            }
            else
            {
                return data.GetInt32(columnIndex);
            }
        }

        protected DateTime? CheckDbNullDateTime(NpgsqlDataReader data, string columnName)
        {
			int columnIndex = data.GetOrdinal(columnName);
			if (data.IsDBNull(columnIndex))
			{
				return null;
			}
			else
			{
				return data.GetDateTime(columnIndex);
			}
		}

        protected string CheckDbNullString(NpgsqlDataReader data, string columnName)
        {
            int columnIndex = data.GetOrdinal(columnName);
            if (data.IsDBNull(columnIndex))
            {
                return string.Empty;
            }
            else
            {
                return data.GetString(columnIndex);
            }
        }

        protected bool? CheckDbNullBoolean(NpgsqlDataReader data, string columnName)
        {
            int columnIndex = data.GetOrdinal(columnName);
            if (data.IsDBNull(columnIndex))
            {
                return null;
            }
            else
            {
                return data.GetBoolean(columnIndex);
            }
        }

        protected decimal CheckDbNullDecimal(NpgsqlDataReader data, string columnName)
        {
            int columnIndex = data.GetOrdinal(columnName);
            if (data.IsDBNull(columnIndex))
            {
                return 0;
            }
            else
            {
                return data.GetDecimal(columnIndex);
            }
        }

        #region JSON

        protected string SerializeDataToJSON(NpgsqlDataReader data)
        {
            var dictionary = Enumerable.Range(0, data.FieldCount).ToDictionary(data.GetName, data.GetValue);

            return JsonConvert.SerializeObject(dictionary, Formatting.Indented);
        }

        protected T DeserializeJSONToObj(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        protected T MapDataToObject<T>(NpgsqlDataReader data) where T : new()
        {
            T obj = new T();

            for (int i = 0; i < data.FieldCount; i++)
            {
                string propName = data.GetName(i);
                object value = data[i];

                PropertyInfo propInfo = typeof(T).GetProperty(propName);
                if (propInfo != null && value != DBNull.Value)
                {
                    if (propInfo.PropertyType == typeof(int) && value is long longValue)
                    {
                        propInfo.SetValue(obj, (int)longValue);
                    }
                    else
                    {
                        propInfo.SetValue(obj, value);
                    }
                }
            }

            return obj;
        }

        #endregion

    }
}

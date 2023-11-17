using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE_dotnetframework.DataAccess
{
    public class DataAccessConnection<T> where T : class
    {
        public string ConnectionString 
        { 
            get
            {
                return "Host = localhost; Username = postgres; Password = AMCSentinel333!; Database = VisionMain";
            }
        }

        #region JSON

        public string SerializeDataToJSON(NpgsqlDataReader data)
        {
            var dictionary = Enumerable.Range(0, data.FieldCount).ToDictionary(data.GetName, data.GetValue);

            return JsonConvert.SerializeObject(dictionary);
        }

        public T DeserializeJSONToObj(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        #endregion

    }
}

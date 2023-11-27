using IMTE.Models.General;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.DataAccess
{
	public class ItemDA : DataAccessFunctions<Item>
	{
		public IEnumerable<Item> GetAllItems()
		{
			var output = new List<Item>();

			using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"
								SELECT 
									item.""Id"" as ""ItemId"", item.""ItemCode"", item.""ShortDescription"",
									des.""Id"" as ""DescriptionId"", des.""Text"" as ""DescriptionText""
								FROM ""General"".""Item"" item
								LEFT OUTER JOIN ""General"".""Description"" des ON item.""DescriptionId"" = des.""Id""";


				connection.Open();
				command.Connection = connection;
				command.CommandText = query;

				var data = command.ExecuteReader();

				while (data.Read())
				{
					output.Add(new Item
					{
						Id = CheckDbNullInt(data, "ItemId"),
						ItemCode = CheckDbNullString(data, "ItemCode"),
						ShortDescription = CheckDbNullString(data, "ShortDescription"),
						Description = new Description
						{
							Id = CheckDbNullInt(data, "DescriptionId"),
							Text = CheckDbNullString(data, "DescriptionText"),
						}
					});
				}
			}

			return output;
		}
	}
}

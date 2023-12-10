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
									item.""Id"" as ""ItemId"", item.""Version"", item.""ItemCode"", item.""ShortDescription"",
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
						Version = CheckDbNullInt(data, "Version"),
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

		// Update the item that is related to a measuring device
		public void UpdateItem(Item itemObj, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
			using (NpgsqlCommand command = new NpgsqlCommand())
			{
				string query = @"UPDATE ""General"".""Item"" SET
									""Version"" = ""Version"" + 1,
                                    ""DescriptionId"" = @DescriptionId,
                                    ""ItemCode"" = @ItemCode,
                                    ""ShortDescription"" = @ShortDescription,
                                    ""ModifiedOn"" = @ModifiedOn
                                    WHERE ""Id"" = @Id";

				command.Connection = connection;
				command.Transaction = transaction;
				command.CommandText = query;

				command.Parameters.AddWithValue("@DescriptionId", itemObj.Description.Id);
				command.Parameters.AddWithValue("@ItemCode", itemObj.ItemCode);
				command.Parameters.AddWithValue("@ShortDescription", itemObj.ShortDescription);
				command.Parameters.AddWithValue("@ModifiedOn", DateTime.UtcNow);
				command.Parameters.AddWithValue("@Id", itemObj.Id);

				command.ExecuteNonQuery();
			}
		}

	}
}

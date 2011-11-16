/*
	This code was generated by SQL Compact Code Generator version 1.2.1.5

	SQL Compact Code Generator was written by Christian Resma Helle (http://sqlcecodegen.codeplex.com)
	and is under the GNU General Public License version 2 (GPLv2)

	Generated: 07/19/2011 17:29:26
*/



namespace SmartDeviceApp.TestDatabaseMultiple
{
	/// <summary>
	/// Default IStockRepository implementation 
	/// </summary>
	public partial class StockRepository : IStockRepository
	{
		public System.Data.SqlServerCe.SqlCeTransaction Transaction { get; set; }

		#region SELECT *

		/// <summary>
		/// Retrieves all items as a generic collection
		/// </summary>
		public System.Collections.Generic.List<Stock> ToList()
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "SELECT * FROM Stock";
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		public Stock[] ToArray()
		{
			var list = ToList();
			return list != null ? list.ToArray() : null;
		}

		#endregion

		#region SELECT TOP()

		/// <summary>
		/// Retrieves the first set of items specified by count as a generic collection
		/// </summary>
		/// <param name="count">Number of records to be retrieved</param>
		public System.Collections.Generic.List<Stock> ToList(int count)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = string.Format("SELECT TOP({0}) * FROM Stock", count);
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		public Stock[] ToArray(int count)
		{
			var list = ToList(count);
			return list != null ? list.ToArray() : null;
		}

		#endregion

		#region SELECT .... WHERE Id=?

		/// <summary>
		/// Retrieves a collection of items by Id
		/// </summary>
		/// <param name="Id">Id value</param>
		public System.Collections.Generic.List<Stock> SelectById(System.Int32? Id)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				if (Id != null)
				{
					command.CommandText = "SELECT * FROM Stock WHERE Id=@Id";
					command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
					command.Parameters["@Id"].Value = Id != null ? (object)Id : System.DBNull.Value;
				}
				else
					command.CommandText = "SELECT * FROM Stock WHERE Id IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region SELECT .... WHERE ProductId=?

		/// <summary>
		/// Retrieves a collection of items by ProductId
		/// </summary>
		/// <param name="ProductId">ProductId value</param>
		public System.Collections.Generic.List<Stock> SelectByProductId(System.Int32? ProductId)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				if (ProductId != null)
				{
					command.CommandText = "SELECT * FROM Stock WHERE ProductId=@ProductId";
					command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int);
					command.Parameters["@ProductId"].Value = ProductId != null ? (object)ProductId : System.DBNull.Value;
				}
				else
					command.CommandText = "SELECT * FROM Stock WHERE ProductId IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region SELECT .... WHERE Quantity=?

		/// <summary>
		/// Retrieves a collection of items by Quantity
		/// </summary>
		/// <param name="Quantity">Quantity value</param>
		public System.Collections.Generic.List<Stock> SelectByQuantity(System.Int32? Quantity)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				if (Quantity != null)
				{
					command.CommandText = "SELECT * FROM Stock WHERE Quantity=@Quantity";
					command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int);
					command.Parameters["@Quantity"].Value = Quantity != null ? (object)Quantity : System.DBNull.Value;
				}
				else
					command.CommandText = "SELECT * FROM Stock WHERE Quantity IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region SELECT .... WHERE LastUpdated=?

		/// <summary>
		/// Retrieves a collection of items by LastUpdated
		/// </summary>
		/// <param name="LastUpdated">LastUpdated value</param>
		public System.Collections.Generic.List<Stock> SelectByLastUpdated(System.DateTime? LastUpdated)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				if (LastUpdated != null)
				{
					command.CommandText = "SELECT * FROM Stock WHERE LastUpdated=@LastUpdated";
					command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
					command.Parameters["@LastUpdated"].Value = LastUpdated != null ? (object)LastUpdated : System.DBNull.Value;
				}
				else
					command.CommandText = "SELECT * FROM Stock WHERE LastUpdated IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region SELECT TOP(?).... WHERE Id=?

		/// <summary>
		/// Retrieves the first set of items specified by count by Id
		/// </summary>
		/// <param name="Id">Id value</param>
		/// <param name="count">Number of records to be retrieved</param>
		public System.Collections.Generic.List<Stock> SelectById(System.Int32? Id, int count)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
			if (Id != null)
			{
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE Id=@Id";					command.CommandText = "SELECT * FROM Stock WHERE Id=@Id";
				command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
				command.Parameters["@Id"].Value = Id != null ? (object)Id : System.DBNull.Value;
			}
			else
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE Id IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region SELECT TOP(?).... WHERE ProductId=?

		/// <summary>
		/// Retrieves the first set of items specified by count by ProductId
		/// </summary>
		/// <param name="ProductId">ProductId value</param>
		/// <param name="count">Number of records to be retrieved</param>
		public System.Collections.Generic.List<Stock> SelectByProductId(System.Int32? ProductId, int count)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
			if (ProductId != null)
			{
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE ProductId=@ProductId";					command.CommandText = "SELECT * FROM Stock WHERE ProductId=@ProductId";
				command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int);
				command.Parameters["@ProductId"].Value = ProductId != null ? (object)ProductId : System.DBNull.Value;
			}
			else
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE ProductId IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region SELECT TOP(?).... WHERE Quantity=?

		/// <summary>
		/// Retrieves the first set of items specified by count by Quantity
		/// </summary>
		/// <param name="Quantity">Quantity value</param>
		/// <param name="count">Number of records to be retrieved</param>
		public System.Collections.Generic.List<Stock> SelectByQuantity(System.Int32? Quantity, int count)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
			if (Quantity != null)
			{
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE Quantity=@Quantity";					command.CommandText = "SELECT * FROM Stock WHERE Quantity=@Quantity";
				command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int);
				command.Parameters["@Quantity"].Value = Quantity != null ? (object)Quantity : System.DBNull.Value;
			}
			else
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE Quantity IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region SELECT TOP(?).... WHERE LastUpdated=?

		/// <summary>
		/// Retrieves the first set of items specified by count by LastUpdated
		/// </summary>
		/// <param name="LastUpdated">LastUpdated value</param>
		/// <param name="count">Number of records to be retrieved</param>
		public System.Collections.Generic.List<Stock> SelectByLastUpdated(System.DateTime? LastUpdated, int count)
		{
			var list = new System.Collections.Generic.List<Stock>();
			using (var command = EntityBase.CreateCommand(Transaction))
			{
			if (LastUpdated != null)
			{
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE LastUpdated=@LastUpdated";					command.CommandText = "SELECT * FROM Stock WHERE LastUpdated=@LastUpdated";
				command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
				command.Parameters["@LastUpdated"].Value = LastUpdated != null ? (object)LastUpdated : System.DBNull.Value;
			}
			else
				command.CommandText = "SELECT TOP(" + count + ") * FROM Stock WHERE LastUpdated IS NULL";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Stock();
						item.Id = (System.Int32?) (reader.IsDBNull(0) ? null : reader["Id"]);
						item.ProductId = (System.Int32?) (reader.IsDBNull(1) ? null : reader["ProductId"]);
						item.Quantity = (System.Int32?) (reader.IsDBNull(2) ? null : reader["Quantity"]);
						item.LastUpdated = (System.DateTime?) (reader.IsDBNull(3) ? null : reader["LastUpdated"]);
						list.Add(item);
					}
				}
			}
			return list.Count > 0 ? list : null;
		}

		#endregion

		#region INSERT [Stock]

		/// <summary>
		/// Inserts the item to the table
		/// </summary>
		/// <param name="item">Item to insert to the database</param>
		public void Create(Stock item)
		{
			Create(item.ProductId, item.Quantity, item.LastUpdated);
		}

		#endregion

		#region INSERT Ignoring Primary Key

		/// <summary>
		/// Inserts a new record to the table without specifying the primary key
		/// </summary>
		/// <param name="ProductId">ProductId value</param>
		/// <param name="Quantity">Quantity value</param>
		/// <param name="LastUpdated">LastUpdated value</param>
		public void Create(System.Int32? ProductId, System.Int32? Quantity, System.DateTime? LastUpdated)
		{

			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "INSERT INTO [Stock] (ProductId, Quantity, LastUpdated)  VALUES (@ProductId, @Quantity, @LastUpdated)";

				command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int);
				command.Parameters["@ProductId"].Value = ProductId != null ? (object)ProductId : System.DBNull.Value;
				command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int);
				command.Parameters["@Quantity"].Value = Quantity != null ? (object)Quantity : System.DBNull.Value;
				command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
				command.Parameters["@LastUpdated"].Value = LastUpdated != null ? (object)LastUpdated : System.DBNull.Value;
				command.ExecuteNonQuery();
			}
		}

		#endregion

		#region INSERT [Stock] by fields

		/// <summary>
		/// Inserts a new record to the table specifying all fields
		/// </summary>
		/// <param name="Id">Id value</param>
		/// <param name="ProductId">ProductId value</param>
		/// <param name="Quantity">Quantity value</param>
		/// <param name="LastUpdated">LastUpdated value</param>
		public void Create(System.Int32? Id, System.Int32? ProductId, System.Int32? Quantity, System.DateTime? LastUpdated)
		{

			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "INSERT INTO [Stock] (Id, ProductId, Quantity, LastUpdated)  VALUES (@Id, @ProductId, @Quantity, @LastUpdated)";

				command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
				command.Parameters["@Id"].Value = Id != null ? (object)Id : System.DBNull.Value;
				command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int);
				command.Parameters["@ProductId"].Value = ProductId != null ? (object)ProductId : System.DBNull.Value;
				command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int);
				command.Parameters["@Quantity"].Value = Quantity != null ? (object)Quantity : System.DBNull.Value;
				command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
				command.Parameters["@LastUpdated"].Value = LastUpdated != null ? (object)LastUpdated : System.DBNull.Value;
				command.ExecuteNonQuery();
			}
		}

		#endregion

		#region INSERT MANY

		/// <summary>
		/// Populates the table with a collection of items
		/// </summary>
		public void Create(System.Collections.Generic.IEnumerable<Stock> items)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandType = System.Data.CommandType.TableDirect;
				command.CommandText = "Stock";

				using (var resultSet = command.ExecuteResultSet(System.Data.SqlServerCe.ResultSetOptions.Updatable))
				{
					var record = resultSet.CreateRecord();
					foreach (var item in items)
					{
						record.SetValue(1, item.ProductId);
						record.SetValue(2, item.Quantity);
						record.SetValue(3, item.LastUpdated);
						resultSet.Insert(record);
					}
				}
			}
		}

		#endregion

		#region DELETE

		/// <summary>
		/// Deletes the item
		/// </summary>
		/// <param name="item">Item to delete</param>
		public void Delete(Stock item)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "DELETE FROM [Stock] WHERE Id = @Id";

				command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
				command.Parameters["@Id"].Value = item.Id != null ? (object)item.Id : System.DBNull.Value;
				command.ExecuteNonQuery();
			}
		}

		#endregion

		#region DELETE MANY

		/// <summary>
		/// Deletes a collection of item
		/// </summary>
		/// <param name="items">Items to delete</param>
		public void Delete(System.Collections.Generic.IEnumerable<Stock> items)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "DELETE FROM [Stock] WHERE Id = @Id";
				command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
				command.Prepare();

				foreach (var item in items)
				{
					command.Parameters["@Id"].Value = item.Id != null ? (object)item.Id : System.DBNull.Value;

					command.ExecuteNonQuery();
				}
			}
		}

		#endregion

		#region DELETE BY Id

		/// <summary>
		/// Delete records by Id
		/// </summary>
		/// <param name="Id">Id value</param>
		public int DeleteById(System.Int32? Id)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "DELETE FROM [Stock] WHERE Id=@Id";
				command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
				command.Parameters["@Id"].Value = Id != null ? (object)Id : System.DBNull.Value;

				return command.ExecuteNonQuery();
			}
		}

		#endregion

		#region DELETE BY ProductId

		/// <summary>
		/// Delete records by ProductId
		/// </summary>
		/// <param name="ProductId">ProductId value</param>
		public int DeleteByProductId(System.Int32? ProductId)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "DELETE FROM [Stock] WHERE ProductId=@ProductId";
				command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int);
				command.Parameters["@ProductId"].Value = ProductId != null ? (object)ProductId : System.DBNull.Value;

				return command.ExecuteNonQuery();
			}
		}

		#endregion

		#region DELETE BY Quantity

		/// <summary>
		/// Delete records by Quantity
		/// </summary>
		/// <param name="Quantity">Quantity value</param>
		public int DeleteByQuantity(System.Int32? Quantity)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "DELETE FROM [Stock] WHERE Quantity=@Quantity";
				command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int);
				command.Parameters["@Quantity"].Value = Quantity != null ? (object)Quantity : System.DBNull.Value;

				return command.ExecuteNonQuery();
			}
		}

		#endregion

		#region DELETE BY LastUpdated

		/// <summary>
		/// Delete records by LastUpdated
		/// </summary>
		/// <param name="LastUpdated">LastUpdated value</param>
		public int DeleteByLastUpdated(System.DateTime? LastUpdated)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "DELETE FROM [Stock] WHERE LastUpdated=@LastUpdated";
				command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
				command.Parameters["@LastUpdated"].Value = LastUpdated != null ? (object)LastUpdated : System.DBNull.Value;

				return command.ExecuteNonQuery();
			}
		}

		#endregion

		#region Purge

		/// <summary>
		/// Purges the contents of the table
		/// </summary>
		public int Purge()
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "DELETE FROM [Stock]";
				return command.ExecuteNonQuery();
			}
		}

		#endregion

		#region UPDATE

		/// <summary>
		/// Updates the item
		/// </summary>
		/// <param name="item">Item to update</param>
		public void Update(Stock item)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "UPDATE [Stock] SET ProductId = @ProductId, Quantity = @Quantity, LastUpdated = @LastUpdated WHERE Id = @Id";

				command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
				command.Parameters["@Id"].Value = item.Id != null ? (object)item.Id : System.DBNull.Value;
				command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int);
				command.Parameters["@ProductId"].Value = item.ProductId != null ? (object)item.ProductId : System.DBNull.Value;
				command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int);
				command.Parameters["@Quantity"].Value = item.Quantity != null ? (object)item.Quantity : System.DBNull.Value;
				command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
				command.Parameters["@LastUpdated"].Value = item.LastUpdated != null ? (object)item.LastUpdated : System.DBNull.Value;
				command.ExecuteNonQuery();
			}
		}

		#endregion

		#region UPDATE MANY

		/// <summary>
		/// Updates a collection of items
		/// </summary>
		/// <param name="items">Items to update</param>
		public void Update(System.Collections.Generic.IEnumerable<Stock> items)
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "UPDATE [Stock] SET ProductId = @ProductId, Quantity = @Quantity, LastUpdated = @LastUpdated WHERE Id = @Id";
				command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
				command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int);
				command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int);
				command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
				command.Prepare();

				foreach (var item in items)
				{
					command.Parameters["@Id"].Value = item.Id != null ? (object)item.Id : System.DBNull.Value;
					command.Parameters["@ProductId"].Value = item.ProductId != null ? (object)item.ProductId : System.DBNull.Value;
					command.Parameters["@Quantity"].Value = item.Quantity != null ? (object)item.Quantity : System.DBNull.Value;
					command.Parameters["@LastUpdated"].Value = item.LastUpdated != null ? (object)item.LastUpdated : System.DBNull.Value;
					command.ExecuteNonQuery();
				}
			}
		}

		#endregion

		#region COUNT [Stock]

		/// <summary>
		/// Gets the number of records in the table
		/// </summary>
		public int Count()
		{
			using (var command = EntityBase.CreateCommand(Transaction))
			{
				command.CommandText = "SELECT COUNT(*) FROM Stock";
				return (int)command.ExecuteScalar();
			}
		}

		#endregion

	}
}


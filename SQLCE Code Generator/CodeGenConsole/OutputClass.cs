namespace CodeGenConsole
{
    #region EntityBase
    public abstract class EntityBase
    {
        public static System.String ConnectionString { get; set; }

        private static System.Data.SqlServerCe.SqlCeConnection connectionInstance = null;
        public static System.Data.SqlServerCe.SqlCeConnection Connection
        {
            get
            {
                if (connectionInstance == null)
                    connectionInstance = new System.Data.SqlServerCe.SqlCeConnection(ConnectionString);
                if (connectionInstance.State != System.Data.ConnectionState.Open)
                    connectionInstance.Open();
                return connectionInstance;
            }
        }
    }
    #endregion

    #region Contact
    public partial class Contact
    {
        public System.Int32? id { get; set; }
        public System.String name { get; set; }
        public System.String address { get; set; }
        public System.String postalcode { get; set; }
        public System.String country { get; set; }
        public System.String homephone { get; set; }
        public System.String mobilephone { get; set; }
        public System.String workphone { get; set; }
    }
    #endregion

    #region Customer
    public partial class Customer
    {
        public System.Int32? id { get; set; }
        public System.Int32? contactid { get; set; }
        public System.String name { get; set; }
        public System.String address { get; set; }
        public System.String postalcode { get; set; }
        public System.String country { get; set; }
    }
    #endregion

    #region Manufacturer
    public partial class Manufacturer
    {
        public System.Int32? id { get; set; }
        public System.String name { get; set; }
        public System.String address { get; set; }
        public System.String postalcode { get; set; }
        public System.String country { get; set; }
    }
    #endregion

    #region Product
    public partial class Product
    {
        public System.Int32? id { get; set; }
        public System.String name { get; set; }
        public System.String description { get; set; }
        public System.Double? price { get; set; }
        public System.Int32? manufacturerid { get; set; }
        public System.DateTime? productiondate { get; set; }
        public System.DateTime? lastmodifieddate { get; set; }
    }
    #endregion

}
namespace CodeGenConsole
{
    #region Contact
    public partial class Contact
    {
        #region SELECT *
        public static System.Collections.Generic.List<Contact> ToList()
        {
            var list = new System.Collections.Generic.List<Contact>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contact";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Contact();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.name = reader["name"] as System.String;
                        item.address = reader["address"] as System.String;
                        item.postalcode = reader["postalcode"] as System.String;
                        item.country = reader["country"] as System.String;
                        item.homephone = reader["homephone"] as System.String;
                        item.mobilephone = reader["mobilephone"] as System.String;
                        item.workphone = reader["workphone"] as System.String;
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Contact[] ToArray()
        {
            var list = ToList();
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region SELECT TOP()
        public static System.Collections.Generic.List<Contact> ToList(int count)
        {
            var list = new System.Collections.Generic.List<Contact>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT TOP({0}) * FROM Contact", count);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Contact();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.name = reader["name"] as System.String;
                        item.address = reader["address"] as System.String;
                        item.postalcode = reader["postalcode"] as System.String;
                        item.country = reader["country"] as System.String;
                        item.homephone = reader["homephone"] as System.String;
                        item.mobilephone = reader["mobilephone"] as System.String;
                        item.workphone = reader["workphone"] as System.String;
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Contact[] ToArray(int count)
        {
            var list = ToList(count);
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region INSERT
        public static Contact Create(System.String name, System.String address, System.String postalcode, System.String country, System.String homephone, System.String mobilephone, System.String workphone)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Contact (name, address, postalcode, country, homephone, mobilephone, workphone)  VALUES (@name, @address, @postalcode, @country, @homephone, @mobilephone, @workphone)";
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.Parameters.AddWithValue("@homephone", homephone != null ? (object)homephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@mobilephone", mobilephone != null ? (object)mobilephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@workphone", workphone != null ? (object)workphone : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Contact();

                command.CommandText = "SELECT TOP(1) id FROM Contact ORDER BY id DESC";
                command.Parameters.Clear();
                var value = command.ExecuteScalar();

                item.id = value as System.Int32?;
                item.name = name;
                item.address = address;
                item.postalcode = postalcode;
                item.country = country;
                item.homephone = homephone;
                item.mobilephone = mobilephone;
                item.workphone = workphone;
                return item;
            }
        }
        #endregion

        #region INSERT
        public static Contact Create(System.Int32? id, System.String name, System.String address, System.String postalcode, System.String country, System.String homephone, System.String mobilephone, System.String workphone)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Contact (id, name, address, postalcode, country, homephone, mobilephone, workphone)  VALUES (@id, @name, @address, @postalcode, @country, @homephone, @mobilephone, @workphone)";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.Parameters.AddWithValue("@homephone", homephone != null ? (object)homephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@mobilephone", mobilephone != null ? (object)mobilephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@workphone", workphone != null ? (object)workphone : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Contact();
                item.id = id;
                item.name = name;
                item.address = address;
                item.postalcode = postalcode;
                item.country = country;
                item.homephone = homephone;
                item.mobilephone = mobilephone;
                item.workphone = workphone;
                return item;
            }
        }
        #endregion

        #region DELETE
        public void Delete()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Contact WHERE id = @id AND name = @name AND address = @address AND postalcode = @postalcode AND country = @country AND homephone = @homephone AND mobilephone = @mobilephone AND workphone = @workphone";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.Parameters.AddWithValue("@homephone", homephone != null ? (object)homephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@mobilephone", mobilephone != null ? (object)mobilephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@workphone", workphone != null ? (object)workphone : System.DBNull.Value);
                command.ExecuteNonQuery();
            }

            this.id = null;
            this.name = null;
            this.address = null;
            this.postalcode = null;
            this.country = null;
            this.homephone = null;
            this.mobilephone = null;
            this.workphone = null;
        }
        #endregion

        #region Purge
        public static void Purge()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Contact";
                command.ExecuteNonQuery();
            }

        }
        #endregion

        #region UPDATE
        public void SaveChanges()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "UPDATE Contact SET name = @name, address = @address, postalcode = @postalcode, country = @country, homephone = @homephone, mobilephone = @mobilephone, workphone = @workphone WHERE id = @id";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.Parameters.AddWithValue("@homephone", homephone != null ? (object)homephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@mobilephone", mobilephone != null ? (object)mobilephone : System.DBNull.Value);
                command.Parameters.AddWithValue("@workphone", workphone != null ? (object)workphone : System.DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
    #endregion

    #region Customer
    public partial class Customer
    {
        #region SELECT *
        public static System.Collections.Generic.List<Customer> ToList()
        {
            var list = new System.Collections.Generic.List<Customer>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Customer";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Customer();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.contactid = (System.Int32?)(reader["contactid"] is System.DBNull ? null : reader["contactid"]);
                        item.name = reader["name"] as System.String;
                        item.address = reader["address"] as System.String;
                        item.postalcode = reader["postalcode"] as System.String;
                        item.country = reader["country"] as System.String;
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Customer[] ToArray()
        {
            var list = ToList();
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region SELECT TOP()
        public static System.Collections.Generic.List<Customer> ToList(int count)
        {
            var list = new System.Collections.Generic.List<Customer>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT TOP({0}) * FROM Customer", count);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Customer();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.contactid = (System.Int32?)(reader["contactid"] is System.DBNull ? null : reader["contactid"]);
                        item.name = reader["name"] as System.String;
                        item.address = reader["address"] as System.String;
                        item.postalcode = reader["postalcode"] as System.String;
                        item.country = reader["country"] as System.String;
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Customer[] ToArray(int count)
        {
            var list = ToList(count);
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region INSERT
        public static Customer Create(System.Int32? contactid, System.String name, System.String address, System.String postalcode, System.String country)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Customer (contactid, name, address, postalcode, country)  VALUES (@contactid, @name, @address, @postalcode, @country)";
                command.Parameters.AddWithValue("@contactid", contactid != null ? (object)contactid : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Customer();

                command.CommandText = "SELECT TOP(1) id FROM Customer ORDER BY id DESC";
                command.Parameters.Clear();
                var value = command.ExecuteScalar();

                item.id = value as System.Int32?;
                item.contactid = contactid;
                item.name = name;
                item.address = address;
                item.postalcode = postalcode;
                item.country = country;
                return item;
            }
        }
        #endregion

        #region INSERT
        public static Customer Create(System.Int32? id, System.Int32? contactid, System.String name, System.String address, System.String postalcode, System.String country)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Customer (id, contactid, name, address, postalcode, country)  VALUES (@id, @contactid, @name, @address, @postalcode, @country)";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@contactid", contactid != null ? (object)contactid : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Customer();
                item.id = id;
                item.contactid = contactid;
                item.name = name;
                item.address = address;
                item.postalcode = postalcode;
                item.country = country;
                return item;
            }
        }
        #endregion

        #region DELETE
        public void Delete()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Customer WHERE id = @id AND contactid = @contactid AND name = @name AND address = @address AND postalcode = @postalcode AND country = @country";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@contactid", contactid != null ? (object)contactid : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();
            }

            this.id = null;
            this.contactid = null;
            this.name = null;
            this.address = null;
            this.postalcode = null;
            this.country = null;
        }
        #endregion

        #region Purge
        public static void Purge()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Customer";
                command.ExecuteNonQuery();
            }

        }
        #endregion

        #region UPDATE
        public void SaveChanges()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "UPDATE Customer SET contactid = @contactid, name = @name, address = @address, postalcode = @postalcode, country = @country WHERE id = @id";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@contactid", contactid != null ? (object)contactid : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
    #endregion

    #region Manufacturer
    public partial class Manufacturer
    {
        #region SELECT *
        public static System.Collections.Generic.List<Manufacturer> ToList()
        {
            var list = new System.Collections.Generic.List<Manufacturer>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Manufacturer";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Manufacturer();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.name = reader["name"] as System.String;
                        item.address = reader["address"] as System.String;
                        item.postalcode = reader["postalcode"] as System.String;
                        item.country = reader["country"] as System.String;
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Manufacturer[] ToArray()
        {
            var list = ToList();
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region SELECT TOP()
        public static System.Collections.Generic.List<Manufacturer> ToList(int count)
        {
            var list = new System.Collections.Generic.List<Manufacturer>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT TOP({0}) * FROM Manufacturer", count);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Manufacturer();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.name = reader["name"] as System.String;
                        item.address = reader["address"] as System.String;
                        item.postalcode = reader["postalcode"] as System.String;
                        item.country = reader["country"] as System.String;
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Manufacturer[] ToArray(int count)
        {
            var list = ToList(count);
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region INSERT
        public static Manufacturer Create(System.String name, System.String address, System.String postalcode, System.String country)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Manufacturer (name, address, postalcode, country)  VALUES (@name, @address, @postalcode, @country)";
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Manufacturer();

                command.CommandText = "SELECT TOP(1) id FROM Manufacturer ORDER BY id DESC";
                command.Parameters.Clear();
                var value = command.ExecuteScalar();

                item.id = value as System.Int32?;
                item.name = name;
                item.address = address;
                item.postalcode = postalcode;
                item.country = country;
                return item;
            }
        }
        #endregion

        #region INSERT
        public static Manufacturer Create(System.Int32? id, System.String name, System.String address, System.String postalcode, System.String country)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Manufacturer (id, name, address, postalcode, country)  VALUES (@id, @name, @address, @postalcode, @country)";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Manufacturer();
                item.id = id;
                item.name = name;
                item.address = address;
                item.postalcode = postalcode;
                item.country = country;
                return item;
            }
        }
        #endregion

        #region DELETE
        public void Delete()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Manufacturer WHERE id = @id AND name = @name AND address = @address AND postalcode = @postalcode AND country = @country";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();
            }

            this.id = null;
            this.name = null;
            this.address = null;
            this.postalcode = null;
            this.country = null;
        }
        #endregion

        #region Purge
        public static void Purge()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Manufacturer";
                command.ExecuteNonQuery();
            }

        }
        #endregion

        #region UPDATE
        public void SaveChanges()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "UPDATE Manufacturer SET name = @name, address = @address, postalcode = @postalcode, country = @country WHERE id = @id";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@address", address != null ? (object)address : System.DBNull.Value);
                command.Parameters.AddWithValue("@postalcode", postalcode != null ? (object)postalcode : System.DBNull.Value);
                command.Parameters.AddWithValue("@country", country != null ? (object)country : System.DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
    #endregion

    #region Product
    public partial class Product
    {
        #region SELECT *
        public static System.Collections.Generic.List<Product> ToList()
        {
            var list = new System.Collections.Generic.List<Product>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Product";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Product();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.name = reader["name"] as System.String;
                        item.description = reader["description"] as System.String;
                        item.price = (System.Double?)(reader["price"] is System.DBNull ? null : reader["price"]);
                        item.manufacturerid = (System.Int32?)(reader["manufacturerid"] is System.DBNull ? null : reader["manufacturerid"]);
                        item.productiondate = (System.DateTime?)(reader["productiondate"] is System.DBNull ? null : reader["productiondate"]);
                        item.lastmodifieddate = (System.DateTime?)(reader["lastmodifieddate"] is System.DBNull ? null : reader["lastmodifieddate"]);
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Product[] ToArray()
        {
            var list = ToList();
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region SELECT TOP()
        public static System.Collections.Generic.List<Product> ToList(int count)
        {
            var list = new System.Collections.Generic.List<Product>();
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT TOP({0}) * FROM Product", count);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Product();
                        item.id = (System.Int32?)(reader["id"] is System.DBNull ? null : reader["id"]);
                        item.name = reader["name"] as System.String;
                        item.description = reader["description"] as System.String;
                        item.price = (System.Double?)(reader["price"] is System.DBNull ? null : reader["price"]);
                        item.manufacturerid = (System.Int32?)(reader["manufacturerid"] is System.DBNull ? null : reader["manufacturerid"]);
                        item.productiondate = (System.DateTime?)(reader["productiondate"] is System.DBNull ? null : reader["productiondate"]);
                        item.lastmodifieddate = (System.DateTime?)(reader["lastmodifieddate"] is System.DBNull ? null : reader["lastmodifieddate"]);
                        list.Add(item);
                    }
                }
            }
            return list.Count > 0 ? list : null;
        }

        public static Product[] ToArray(int count)
        {
            var list = ToList(count);
            return list != null ? list.ToArray() : null;
        }
        #endregion

        #region INSERT
        public static Product Create(System.String name, System.String description, System.Double? price, System.Int32? manufacturerid, System.DateTime? productiondate, System.DateTime? lastmodifieddate)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Product (name, description, price, manufacturerid, productiondate, lastmodifieddate)  VALUES (@name, @description, @price, @manufacturerid, @productiondate, @lastmodifieddate)";
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@description", description != null ? (object)description : System.DBNull.Value);
                command.Parameters.AddWithValue("@price", price != null ? (object)price : System.DBNull.Value);
                command.Parameters.AddWithValue("@manufacturerid", manufacturerid != null ? (object)manufacturerid : System.DBNull.Value);
                command.Parameters.AddWithValue("@productiondate", productiondate != null ? (object)productiondate : System.DBNull.Value);
                command.Parameters.AddWithValue("@lastmodifieddate", lastmodifieddate != null ? (object)lastmodifieddate : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Product();

                command.CommandText = "SELECT TOP(1) id FROM Product ORDER BY id DESC";
                command.Parameters.Clear();
                var value = command.ExecuteScalar();

                item.id = value as System.Int32?;
                item.name = name;
                item.description = description;
                item.price = price;
                item.manufacturerid = manufacturerid;
                item.productiondate = productiondate;
                item.lastmodifieddate = lastmodifieddate;
                return item;
            }
        }
        #endregion

        #region INSERT
        public static Product Create(System.Int32? id, System.String name, System.String description, System.Double? price, System.Int32? manufacturerid, System.DateTime? productiondate, System.DateTime? lastmodifieddate)
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Product (id, name, description, price, manufacturerid, productiondate, lastmodifieddate)  VALUES (@id, @name, @description, @price, @manufacturerid, @productiondate, @lastmodifieddate)";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@description", description != null ? (object)description : System.DBNull.Value);
                command.Parameters.AddWithValue("@price", price != null ? (object)price : System.DBNull.Value);
                command.Parameters.AddWithValue("@manufacturerid", manufacturerid != null ? (object)manufacturerid : System.DBNull.Value);
                command.Parameters.AddWithValue("@productiondate", productiondate != null ? (object)productiondate : System.DBNull.Value);
                command.Parameters.AddWithValue("@lastmodifieddate", lastmodifieddate != null ? (object)lastmodifieddate : System.DBNull.Value);
                command.ExecuteNonQuery();

                var item = new Product();
                item.id = id;
                item.name = name;
                item.description = description;
                item.price = price;
                item.manufacturerid = manufacturerid;
                item.productiondate = productiondate;
                item.lastmodifieddate = lastmodifieddate;
                return item;
            }
        }
        #endregion

        #region DELETE
        public void Delete()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Product WHERE id = @id AND name = @name AND description = @description AND price = @price AND manufacturerid = @manufacturerid AND productiondate = @productiondate AND lastmodifieddate = @lastmodifieddate";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@description", description != null ? (object)description : System.DBNull.Value);
                command.Parameters.AddWithValue("@price", price != null ? (object)price : System.DBNull.Value);
                command.Parameters.AddWithValue("@manufacturerid", manufacturerid != null ? (object)manufacturerid : System.DBNull.Value);
                command.Parameters.AddWithValue("@productiondate", productiondate != null ? (object)productiondate : System.DBNull.Value);
                command.Parameters.AddWithValue("@lastmodifieddate", lastmodifieddate != null ? (object)lastmodifieddate : System.DBNull.Value);
                command.ExecuteNonQuery();
            }

            this.id = null;
            this.name = null;
            this.description = null;
            this.price = null;
            this.manufacturerid = null;
            this.productiondate = null;
            this.lastmodifieddate = null;
        }
        #endregion

        #region Purge
        public static void Purge()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Product";
                command.ExecuteNonQuery();
            }

        }
        #endregion

        #region UPDATE
        public void SaveChanges()
        {
            using (var command = EntityBase.Connection.CreateCommand())
            {
                command.CommandText = "UPDATE Product SET name = @name, description = @description, price = @price, manufacturerid = @manufacturerid, productiondate = @productiondate, lastmodifieddate = @lastmodifieddate WHERE id = @id";
                command.Parameters.AddWithValue("@id", id != null ? (object)id : System.DBNull.Value);
                command.Parameters.AddWithValue("@name", name != null ? (object)name : System.DBNull.Value);
                command.Parameters.AddWithValue("@description", description != null ? (object)description : System.DBNull.Value);
                command.Parameters.AddWithValue("@price", price != null ? (object)price : System.DBNull.Value);
                command.Parameters.AddWithValue("@manufacturerid", manufacturerid != null ? (object)manufacturerid : System.DBNull.Value);
                command.Parameters.AddWithValue("@productiondate", productiondate != null ? (object)productiondate : System.DBNull.Value);
                command.Parameters.AddWithValue("@lastmodifieddate", lastmodifieddate != null ? (object)lastmodifieddate : System.DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
    #endregion

}

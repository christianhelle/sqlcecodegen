using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpDataAccessLayerGenerator : DataAccessLayerGenerator
    {
        public CSharpDataAccessLayerGenerator(StringBuilder code, Table table)
            : base(code, table)
        {
        }

        private void GetReaderValues(Table table)
        {
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.AppendLine("\t\t\t\t\t\titem." + column.Key + " = (" + column.Value + "?) (reader[\"" + column.Key + "\"] == System.DBNull.Value ? null : reader[\"" + column.Key + "\"]);");
                else
                    code.AppendLine("\t\t\t\t\t\titem." + column.Key + " = reader[\"" + column.Key + "\"] as " + column.Value + ";");
            }
        }

        public override void GenerateSelectAll()
        {
            code.AppendLine("\t\t#region SELECT *");
            code.AppendLine("\t\tpublic System.Collections.Generic.List<" + table.TableName + "> ToList()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = \"SELECT * FROM " + table.TableName + "\";");
            code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            code.AppendLine("\t\t\t\t\t{");
            code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
            GetReaderValues(table);
            code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
            code.AppendLine("\t\t\t\t\t}");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\tpublic " + table.TableName + "[] ToArray()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = ToList();");
            code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSelectWithTop()
        {
            code.AppendLine("\t\t#region SELECT TOP()");
            code.AppendLine("\t\tpublic System.Collections.Generic.List<" + table.TableName + "> ToList(int count)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = string.Format(\"SELECT TOP({0}) * FROM " + table.TableName + "\", count);");
            code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            code.AppendLine("\t\t\t\t\t{");
            code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
            GetReaderValues(table);
            code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
            code.AppendLine("\t\t\t\t\t}");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\tpublic " + table.TableName + "[] ToArray(int count)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = ToList(count);");
            code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSelectBy()
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                code.AppendLine("\t\t#region SELECT .... WHERE " + column.Value.Name + "=?");
                if (column.Value.ManagedType.IsValueType)
                    code.AppendFormat("\n\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2})", table.TableName, column.Value.ManagedType, column.Value.Name);
                else
                    code.AppendFormat("\n\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2})", table.TableName, column.Value.ManagedType, column.Value.Name);
                code.AppendLine();
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
                code.AppendLine("\t\t\t{");
                code.AppendLine("\t\t\tif (" + column.Value.Name + " != null)");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT * FROM {0} WHERE {1}=@{1}\";", table.TableName, column.Value.Name);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.Name);
                code.AppendLine();
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\telse");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT * FROM {0} WHERE {1} IS NULL\";", table.TableName, column.Value.Name);
                code.AppendLine();
                code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                code.AppendLine("\t\t\t\t{");
                code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                code.AppendLine("\t\t\t\t\t{");
                code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
                GetReaderValues(table);
                code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
                code.AppendLine("\t\t\t\t\t}");
                code.AppendLine("\t\t\t\t}");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                code.AppendLine("\t\t}");
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateSelectByWithTop()
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                code.AppendLine("\t\t#region SELECT TOP(?).... WHERE " + column.Value.Name + "=?");
                if (column.Value.ManagedType.IsValueType)
                    code.AppendFormat("\n\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count)", table.TableName, column.Value.ManagedType, column.Value.Name);
                else
                    code.AppendFormat("\n\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count)", table.TableName, column.Value.ManagedType, column.Value.Name);
                code.AppendLine();
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
                code.AppendLine("\t\t\t{");
                code.AppendLine("\t\t\tif (" + column.Value.Name + " != null)");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT TOP(\" + count + \") * FROM {0} WHERE {1}=@{1}\";", table.TableName, column.Value.Name);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.Name);
                code.AppendLine();
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\telse");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT TOP(\" + count + \") * FROM {0} WHERE {1} IS NULL\";", table.TableName, column.Value.Name);
                code.AppendLine();
                code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                code.AppendLine("\t\t\t\t{");
                code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                code.AppendLine("\t\t\t\t\t{");
                code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
                GetReaderValues(table);
                code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
                code.AppendLine("\t\t\t\t\t}");
                code.AppendLine("\t\t\t\t}");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                code.AppendLine("\t\t}");
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateCreate()
        {
            code.AppendLine("\t\t#region INSERT " + table.TableName);
            code.AppendLine("\t\tpublic void Create(" + table.TableName + " item)");
            code.AppendLine("\t\t{");
            code.Append("\t\t\tCreate(");
            foreach (var column in table.Columns)
            {
                if (column.Value.IsPrimaryKey && column.Value.AutoIncrement)
                    continue;
                code.Append("item." + column.Value.Name + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(");");
            code.AppendLine();
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateCreateIgnoringPrimaryKey()
        {
            code.AppendLine("\t\t#region INSERT Ignoring Primary Key");
            code.Append("\t\tpublic void Create(");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Key + ", ");
                else
                    code.Append(column.Value + " " + column.Key + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(")");
            code.AppendLine();
            code.AppendLine("\t\t{");

            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                if (!column.Value.ManagedType.Equals(typeof(string)))
                    continue;
                code.AppendLine("\t\t\tif (" + column.Value.Name + " != null && " + column.Value.Name + ".Length > " + column.Value.MaxLength + ")");
                code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.Name + " is " + column.Value.MaxLength + "\");");
            }
            code.AppendLine();

            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + table.TableName + " (");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Key + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                query.Append("@" + column.Key + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", " + column.Key + " != null ? (object)" + column.Key + " : System.DBNull.Value);");
            }
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateCreateUsingAllColumns()
        {
            code.AppendLine("\t\t#region INSERT " + table.TableName + " by fields");
            code.Append("\t\tpublic void Create(");
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Key + ", ");
                else
                    code.Append(column.Value + " " + column.Key + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(")\n");
            code.AppendLine("\t\t{");

            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                if (!column.Value.ManagedType.Equals(typeof(string)))
                    continue;
                code.AppendLine("\t\t\tif (" + column.Value.Name + " != null && " + column.Value.Name + ".Length > " + column.Value.MaxLength + ")");
                code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.Name + " is " + column.Value.MaxLength + "\");");
            }
            code.AppendLine();

            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + table.TableName + " (");
            foreach (var column in table.Columns)
                query.Append(column.Key + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in table.Columns)
                query.Append("@" + column.Key + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", " + column.Key + " != null ? (object)" + column.Key + " : System.DBNull.Value);");
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GeneratePopulate()
        {
            code.AppendLine("\t\t#region INSERT MANY");
            code.AppendLine("\t\tpublic void Create(System.Collections.Generic.IEnumerable<" + table.TableName + "> items)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tforeach (var item in items)");
            code.Append("\t\t\t\tCreate(");
            foreach (var column in table.Columns)
            {
                if (column.Value.IsPrimaryKey && column.Value.AutoIncrement)
                    continue;
                code.Append("item." + column.Value.Name + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(");");
            code.AppendLine();
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateDelete()
        {
            code.AppendLine("\t\t#region DELETE");
            code.AppendLine("\t\tpublic void Delete(" + table.TableName + " item)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"DELETE FROM " + table.TableName + " WHERE ");

            var hasPrimaryKey = false;
            foreach (var column in table.Columns)
            {
                if (!column.Value.IsPrimaryKey) continue;
                hasPrimaryKey = true;
                query.Append(column.Key + " = @" + column.Key);
                break;
            }
            if (!hasPrimaryKey)
            {
                foreach (var column in table.Columns)
                    query.Append(column.Key + " = @" + column.Key + " AND ");
                query.Remove(query.Length - 5, 5);
            }
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
            {
                if (!hasPrimaryKey)
                    code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", item." + column.Key + " != null ? (object)item." + column.Key + " : System.DBNull.Value);");
                else if (column.Value.IsPrimaryKey)
                {
                    code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", item." + column.Key + " != null ? (object)item." + column.Key + " : System.DBNull.Value);");
                    break;
                }
            }

            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateDeleteBy()
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                code.AppendLine("\t\t#region DELETE BY " + column.Value.Name);
                code.AppendFormat("\n\t\tpublic int DeleteBy{1}({0}{2} {1})", column.Value.ManagedType, column.Value.Name, column.Value.ManagedType.IsValueType ? "?" : string.Empty);
                code.AppendLine("\n\t\t{");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\n\t\t\t\tcommand.CommandText = \"DELETE FROM {0} WHERE {1}=@{1}\";", table.TableName, column.Value.Name);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0} != null ? (object){0} : System.DBNull.Value);", column.Value.Name);
                code.AppendLine("\n\t\t\t\treturn command.ExecuteNonQuery();");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t}");
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateDeleteAll()
        {
            code.AppendLine("\t\t#region Purge");
            code.AppendLine("\t\tpublic void Purge()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = \"DELETE FROM " + table.TableName + "\";");
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSaveChanges()
        {
            code.AppendLine("\t\t#region UPDATE");
            code.AppendLine("\t\tpublic void Update(" + table.TableName + " item)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"UPDATE " + table.TableName + " SET ");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Key + " = @" + column.Key + ", ");
            }
            query.Remove(query.Length - 2, 2);
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                {
                    query.Append(" WHERE " + column.Key + " = @" + column.Key);
                    break;
                }
            }
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", item." + column.Key + " != null ? (object)item." + column.Key + " : System.DBNull.Value);");

            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }
    }
}

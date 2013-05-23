using System;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;

namespace SqlCeLargeDatabaseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());

            for (int i = 100; i <= 1000; i += 100)
            {
                if (File.Exists(i + "Tables.sdf"))
                    File.Delete(i + "Tables.sdf");
            }

            for (int i = 100; i <= 1000; i += 100)
            {
                var connstr = "Data Source=" + i + "Tables.sdf";
                Debug.WriteLine("Connection string: " + connstr);

                using (var engine = new SqlCeEngine(connstr))
                    engine.CreateDatabase();

                using (var conn = new SqlCeConnection(connstr))
                {
                    conn.Open();
                    for (int j = 0; j < i; j++)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "CREATE TABLE tbl" + GetRandomString() + " ( Id INT IDENTITY(1,1) PRIMARY KEY";
                            for (int ii = 0; ii < 10; ii++)
                                cmd.CommandText += ",col" + GetRandomString() + " NVARCHAR(50)";
                            cmd.CommandText += ")";
                            cmd.ExecuteNonQuery();

                            Debug.WriteLine("Executing: " + cmd.CommandText);
                        }
                    }
                }
            }
        }

        private static string GetRandomString()
        {
            return Guid.NewGuid()
                       .ToString()
                       .Replace("{", null)
                       .Replace("}", null)
                       .Replace("-", null);
        }

        const string PWD_CHARSET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVXYZ1234567890";

        static string GenerateString(int len = 10)
        {
            if (len > 4000) len = 4000;
            var buffer = new byte[len * 2];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(buffer);

            using (var stream = new System.IO.MemoryStream(buffer, 0, buffer.Length, false, false))
            using (var reader = new System.IO.BinaryReader(stream))
            {
                var builder = new System.Text.StringBuilder(buffer.Length, buffer.Length);
                while (len-- > 0)
                {
                    var i = (reader.ReadUInt16() & 8) % PWD_CHARSET.Length;
                    builder.Append(PWD_CHARSET[i]);
                }
                return builder.ToString();
            }
        }
    }
}

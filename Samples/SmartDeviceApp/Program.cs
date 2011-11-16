using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using SmartDeviceApp.TestDatabaseSingle;

namespace SmartDeviceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityBase.ConnectionString = string.Format("Data Source={0}\\TestDatabase.sdf", 
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));

            using (IDataRepository repository = new DataRepository())
            {
                var contact = new Contact
                {
                    Name = "Christian Helle",
                    Address = "Somewhere",
                    City = "Over",
                    PostalCode = "The",
                    Country = "Rainbow",
                    Email = "christian.helle@yahoo.com",
                    Phone = "1234567"
                };

                // Create
                repository.Contact.Create(contact);
                repository.Contact.Create(contact);

                // Select
                var contacts = repository.Contact.ToList();
                contacts = repository.Contact.SelectByName("Christian Resma Helle");
                contacts = repository.Contact.SelectByEmail("christian.helle@yahoo.com");

                // Count
                var count = repository.Contact.Count();

                // Update
                contact.Name = "Christian Resma Helle";
                repository.Contact.Update(contact);

                // Delete
                repository.Contact.Delete(contact);
                repository.Contact.Purge();
            }
        }
    }
}

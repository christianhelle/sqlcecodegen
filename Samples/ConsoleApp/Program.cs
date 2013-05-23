using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp.TestDatabaseSingle;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityBase.ConnectionString = "Data Source=TestDatabase.sdf";

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

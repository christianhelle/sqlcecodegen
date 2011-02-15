using System;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenConsole;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class OutputClassTest : CodeGenBaseTest
    {
        [TestMethod]
        public void GenerateContactTest()
        {
            EntityBase.ConnectionString = Settings.Default.TestDatabaseConnectionString;

            var guid = Guid.NewGuid().ToString();
            var contact = Contact.Create(guid, guid, guid, guid, guid, guid, guid);
            Assert.IsNotNull(contact);

            var list = Contact.ToList();
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof (Contact));

            var array = Contact.ToArray();
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof (Contact));

            list = Contact.ToList(10);
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof (Contact));
            Assert.IsTrue(list.Count < 10);

            array = Contact.ToArray(10);
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof (Contact));
            Assert.IsTrue(array.Length < 10);

            contact.SaveChanges();
            contact.Delete();

            var propertyInfoList = typeof(Contact).GetProperties();
            foreach (var property in propertyInfoList)
                Assert.IsNull(property.GetValue(contact, null));

            Product.Purge();
            Manufacturer.Purge();
            Customer.Purge();
            Contact.Purge();

            list = Contact.ToList();
            Assert.IsNull(list);
        }

        [TestMethod]
        public void GenerateCustomerTest()
        {
            EntityBase.ConnectionString = Settings.Default.TestDatabaseConnectionString;

            var guid = Guid.NewGuid().ToString();
            var customer = Customer.Create(null, guid, guid, guid, guid);
            Assert.IsNotNull(customer);

            var list = Customer.ToList();
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof(Customer));

            var array = Customer.ToArray();
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof(Customer));

            list = Customer.ToList(10);
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof(Customer));
            Assert.IsTrue(list.Count < 10);

            array = Customer.ToArray(10);
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof(Customer));
            Assert.IsTrue(array.Length < 10);

            customer.SaveChanges();
            customer.Delete();

            var propertyInfoList = typeof(Customer).GetProperties();
            foreach (var property in propertyInfoList)
                Assert.IsNull(property.GetValue(customer, null));

            Product.Purge();
            Manufacturer.Purge();
            Customer.Purge();

            list = Customer.ToList();
            Assert.IsNull(list);
        }

        [TestMethod]
        public void GenerateManufacturerTest()
        {
            EntityBase.ConnectionString = Settings.Default.TestDatabaseConnectionString;

            var guid = Guid.NewGuid().ToString();
            var manufacturer = Manufacturer.Create(guid, guid, guid, guid);
            Assert.IsNotNull(manufacturer);

            var list = Manufacturer.ToList();
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof(Manufacturer));

            var array = Manufacturer.ToArray();
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof(Manufacturer));

            list = Manufacturer.ToList(10);
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof(Manufacturer));
            Assert.IsTrue(list.Count < 10);

            array = Manufacturer.ToArray(10);
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof(Manufacturer));
            Assert.IsTrue(array.Length < 10);

            manufacturer.SaveChanges();
            manufacturer.Delete();

            var propertyInfoList = typeof(Manufacturer).GetProperties();
            foreach (var property in propertyInfoList)
                Assert.IsNull(property.GetValue(manufacturer, null));

            Product.Purge();
            Manufacturer.Purge();

            list = Manufacturer.ToList();
            Assert.IsNull(list);
        }

        [TestMethod]
        public void GenerateProductTest()
        {
            EntityBase.ConnectionString = Settings.Default.TestDatabaseConnectionString;

            var guid = Guid.NewGuid().ToString();
            var product = Product.Create(guid, guid, 0, null, DateTime.Now, DateTime.Now);
            Assert.IsNotNull(product);

            var list = Product.ToList();
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof(Product));

            var array = Product.ToArray();
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof(Product));

            list = Product.ToList(10);
            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
            CollectionAssert.AllItemsAreInstancesOfType(list, typeof(Product));
            Assert.IsTrue(list.Count < 10);

            array = Product.ToArray(10);
            Assert.IsNotNull(array);
            CollectionAssert.AllItemsAreNotNull(array);
            CollectionAssert.AllItemsAreInstancesOfType(array, typeof(Product));
            Assert.IsTrue(array.Length < 10);

            product.SaveChanges();
            product.Delete();

            var propertyInfoList = typeof(Product).GetProperties();
            foreach (var property in propertyInfoList)
                Assert.IsNull(property.GetValue(product, null));

            Product.Purge();

            list = Product.ToList();
            Assert.IsNull(list);
        }
    }
}
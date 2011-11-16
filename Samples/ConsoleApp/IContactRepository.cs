/*
	This code was generated by SQL Compact Code Generator version 1.2.1.5

	SQL Compact Code Generator was written by Christian Resma Helle (http://sqlcecodegen.codeplex.com)
	and is under the GNU General Public License version 2 (GPLv2)

	Generated: 07/19/2011 18:06:46
*/



namespace ConsoleApp.TestDatabaseMultiple
{
	/// <summary>
	/// Represents the Contact repository
	/// </summary>
	public partial interface IContactRepository : IRepository<Contact>
	{
		/// <summary>
		/// Transaction instance created from <see cref="IDataRepository" />
		/// </summary>
		System.Data.SqlServerCe.SqlCeTransaction Transaction { get; set; }

		/// <summary>
		/// Retrieves a collection of items by Id
		/// </summary>
		/// <param name="Id">Id value</param>
		System.Collections.Generic.List<Contact> SelectById(System.Int32? Id);

		/// <summary>
		/// Retrieves the first set of items specified by count by Id
		/// </summary>
		/// <param name="Id">Id value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectById(System.Int32? Id, int count);

		/// <summary>
		/// Retrieves a collection of items by Name
		/// </summary>
		/// <param name="Name">Name value</param>
		System.Collections.Generic.List<Contact> SelectByName(System.String Name);

		/// <summary>
		/// Retrieves the first set of items specified by count by Name
		/// </summary>
		/// <param name="Name">Name value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectByName(System.String Name, int count);

		/// <summary>
		/// Retrieves a collection of items by Address
		/// </summary>
		/// <param name="Address">Address value</param>
		System.Collections.Generic.List<Contact> SelectByAddress(System.String Address);

		/// <summary>
		/// Retrieves the first set of items specified by count by Address
		/// </summary>
		/// <param name="Address">Address value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectByAddress(System.String Address, int count);

		/// <summary>
		/// Retrieves a collection of items by City
		/// </summary>
		/// <param name="City">City value</param>
		System.Collections.Generic.List<Contact> SelectByCity(System.String City);

		/// <summary>
		/// Retrieves the first set of items specified by count by City
		/// </summary>
		/// <param name="City">City value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectByCity(System.String City, int count);

		/// <summary>
		/// Retrieves a collection of items by PostalCode
		/// </summary>
		/// <param name="PostalCode">PostalCode value</param>
		System.Collections.Generic.List<Contact> SelectByPostalCode(System.String PostalCode);

		/// <summary>
		/// Retrieves the first set of items specified by count by PostalCode
		/// </summary>
		/// <param name="PostalCode">PostalCode value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectByPostalCode(System.String PostalCode, int count);

		/// <summary>
		/// Retrieves a collection of items by Country
		/// </summary>
		/// <param name="Country">Country value</param>
		System.Collections.Generic.List<Contact> SelectByCountry(System.String Country);

		/// <summary>
		/// Retrieves the first set of items specified by count by Country
		/// </summary>
		/// <param name="Country">Country value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectByCountry(System.String Country, int count);

		/// <summary>
		/// Retrieves a collection of items by Email
		/// </summary>
		/// <param name="Email">Email value</param>
		System.Collections.Generic.List<Contact> SelectByEmail(System.String Email);

		/// <summary>
		/// Retrieves the first set of items specified by count by Email
		/// </summary>
		/// <param name="Email">Email value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectByEmail(System.String Email, int count);

		/// <summary>
		/// Retrieves a collection of items by Phone
		/// </summary>
		/// <param name="Phone">Phone value</param>
		System.Collections.Generic.List<Contact> SelectByPhone(System.String Phone);

		/// <summary>
		/// Retrieves the first set of items specified by count by Phone
		/// </summary>
		/// <param name="Phone">Phone value</param>
		/// <param name="count">the number of records to be retrieved</param>
		System.Collections.Generic.List<Contact> SelectByPhone(System.String Phone, int count);

		/// <summary>
		/// Delete records by Id
		/// </summary>
		/// <param name="Id">Id value</param>
		int DeleteById(System.Int32? Id);

		/// <summary>
		/// Delete records by Name
		/// </summary>
		/// <param name="Name">Name value</param>
		int DeleteByName(System.String Name);

		/// <summary>
		/// Delete records by Address
		/// </summary>
		/// <param name="Address">Address value</param>
		int DeleteByAddress(System.String Address);

		/// <summary>
		/// Delete records by City
		/// </summary>
		/// <param name="City">City value</param>
		int DeleteByCity(System.String City);

		/// <summary>
		/// Delete records by PostalCode
		/// </summary>
		/// <param name="PostalCode">PostalCode value</param>
		int DeleteByPostalCode(System.String PostalCode);

		/// <summary>
		/// Delete records by Country
		/// </summary>
		/// <param name="Country">Country value</param>
		int DeleteByCountry(System.String Country);

		/// <summary>
		/// Delete records by Email
		/// </summary>
		/// <param name="Email">Email value</param>
		int DeleteByEmail(System.String Email);

		/// <summary>
		/// Delete records by Phone
		/// </summary>
		/// <param name="Phone">Phone value</param>
		int DeleteByPhone(System.String Phone);

		/// <summary>
		/// Create new record without specifying a primary key
		/// </summary>
		void Create(System.String Name, System.String Address, System.String City, System.String PostalCode, System.String Country, System.String Email, System.String Phone);

		/// <summary>
		/// Create new record specifying all fields
		/// </summary>
		void Create(System.Int32? Id, System.String Name, System.String Address, System.String City, System.String PostalCode, System.String Country, System.String Email, System.String Phone);
	}
}


/*
	This code was generated by SQL Compact Code Generator version 1.2.1.5

	SQL Compact Code Generator was written by Christian Resma Helle (http://sqlcecodegen.codeplex.com)
	and is under the GNU General Public License version 2 (GPLv2)

	Generated: 07/19/2011 17:29:26
*/



namespace SmartDeviceApp.TestDatabaseMultiple
{
	/// <summary>
	/// Main Data Repository interface containing all table repositories
	/// </summary>
	public partial interface IDataRepository : System.IDisposable
	{
		/// <summary>
		/// Gets an instance of the IContactRepository
		/// </summary>
		IContactRepository Contact { get; }

		/// <summary>
		/// Gets an instance of the ICustomerRepository
		/// </summary>
		ICustomerRepository Customer { get; }

		/// <summary>
		/// Gets an instance of the IProductRepository
		/// </summary>
		IProductRepository Product { get; }

		/// <summary>
		/// Gets an instance of the IStockRepository
		/// </summary>
		IStockRepository Stock { get; }

		/// <summary>
		/// Starts a SqlCeTransaction using the global SQL CE Conection instance
		/// </summary>
		System.Data.SqlServerCe.SqlCeTransaction BeginTransaction();

		/// <summary>
		/// Commits the transaction
		/// </summary>
		void Commit();

		/// <summary>
		/// Rollbacks the transaction
		/// </summary>
		void Rollback();
	}
}


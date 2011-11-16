/*
	This code was generated by SQL Compact Code Generator version 1.2.1.5

	SQL Compact Code Generator was written by Christian Resma Helle (http://sqlcecodegen.codeplex.com)
	and is under the GNU General Public License version 2 (GPLv2)

	Generated: 07/19/2011 17:29:26
*/



namespace SmartDeviceApp.TestDatabaseMultiple
{
	/// <summary>
	/// Represents the Stock table
	/// </summary>
	public partial class Stock
	{
		private System.Int32? _Id;
		/// <summary>
		/// Gets or sets the value of Id
		/// </summary>
		public System.Int32? Id
		{
			get { return _Id; }
			set
			{
				_Id = value;
			}
		}
		private System.Int32? _ProductId;
		/// <summary>
		/// Gets or sets the value of ProductId
		/// </summary>
		public System.Int32? ProductId
		{
			get { return _ProductId; }
			set
			{
				_ProductId = value;
			}
		}
		private System.Int32? _Quantity;
		/// <summary>
		/// Gets or sets the value of Quantity
		/// </summary>
		public System.Int32? Quantity
		{
			get { return _Quantity; }
			set
			{
				_Quantity = value;
			}
		}
		private System.DateTime? _LastUpdated;
		/// <summary>
		/// Gets or sets the value of LastUpdated
		/// </summary>
		public System.DateTime? LastUpdated
		{
			get { return _LastUpdated; }
			set
			{
				_LastUpdated = value;
			}
		}
	}
}


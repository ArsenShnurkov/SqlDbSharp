namespace System.Data.SqlDbSharp
{
    using System;
    using System.Data;
    using System.Data.Common;

    using org.rufwork.mooresDb;

	/// <summary>
	/// Transaction class for Hsql ADO.NET data provider.
	/// <seealso cref="SqlDbSharpConnection"/>
	/// <seealso cref="SqlDbSharpReader"/>
	/// <seealso cref="SqlDbSharpParameter"/>
	/// <seealso cref="SqlDbSharpCommand"/>
	/// <seealso cref="SqlDbSharpDataAdapter"/>
	/// </summary>
	public sealed class SqlDbSharpTransaction : DbTransaction, IDbTransaction
	{
		#region Constructors

		/// <summary>
		/// Transaction class constructor.
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="isoLevel"></param>
		internal SqlDbSharpTransaction(SqlDbSharpConnection connection, IsolationLevel isoLevel)
		{
			this._isolationLevel = IsolationLevel.ReadCommitted;
			this._sqlConnection = connection;
			this._sqlConnection.LocalTransaction = this;
			this._isolationLevel = isoLevel;
		}

		#endregion

		#region IDbTransaction Members

		/// <summary>
		/// Aborts the current active transaction.
		/// </summary>
		public override void Rollback()
		{
			if (this._sqlConnection == null)
			{
				throw new InvalidOperationException("Connection is not longer valid.");
			}
			//IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
			this._sqlConnection.Channel.Execute("ROLLBACK WORK");
			this._sqlConnection.Channel.Execute("SET AUTOCOMMIT TRUE");
			this._sqlConnection.LocalTransaction = null;
			this._sqlConnection = null;
		}

		/// <summary>
		/// Closes the current transaction applying all changes to the database.
		/// </summary>
		public override void Commit()
		{
			if (this._sqlConnection.Channel == null)
			{
				throw new InvalidOperationException("Connection is not longer valid.");
			}
			this._sqlConnection.Channel.Execute("COMMIT WORK");
			this._sqlConnection.Channel.Execute("SET AUTOCOMMIT TRUE");
			this._sqlConnection.LocalTransaction = null;
			this._sqlConnection = null;
		}

		/// <summary>
		/// Gets the connection instance used in the transaction.
		/// </summary>
		protected override DbConnection DbConnection
		{
			get
			{
				return _sqlConnection;
			}
		}
		
		/// <summary>
		/// Gets the connection instance used in the transaction.
		/// </summary>
		public new IDbConnection Connection
		{
			get
			{
				return _sqlConnection;
			}
		}

		/// <summary>
		/// Gets the transaction isolation level.
		/// </summary>
		public override System.Data.IsolationLevel IsolationLevel
		{
			get
			{
				return _isolationLevel;
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Dispose this transaction doing a rollback if needed.
		/// </summary>
		public new void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private new void Dispose(bool disposing)
		{
			if (disposing && (this._sqlConnection != null))
			{
				this.Rollback();
			}
		}

		#endregion

		#region Private & Internal Vars

		private IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;
		internal SqlDbSharpConnection _sqlConnection = null;

		#endregion

	}
}

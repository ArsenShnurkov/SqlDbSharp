namespace System.Data.SqlDbSharp
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;

    #region Public Delegates

	/// <summary>
	/// InfoMessage event for Hsql ADO.NET data provider.
	/// </summary>
	public delegate void SqlDbSharpInfoMessageEventHandler(object sender, SqlDbSharpInfoMessageEventArgs e);
	/// <summary>
	/// Row Updated event for Hsql ADO.NET data provider.
	/// </summary>
	public delegate void SqlDbSharpRowUpdatedEventHandler(object sender, SqlDbSharpRowUpdatedEventArgs e);
	/// <summary>
	/// Row Updating event for Hsql ADO.NET data provider.
	/// </summary>
	public delegate void SqlDbSharpRowUpdatingEventHandler(object sender, SqlDbSharpRowUpdatingEventArgs e);
	
	#endregion

	#region SqlDbSharpInfoMessageEventArgs

	/// <summary>
	/// InfoMessage argument class for Hsql ADO.NET data provider.
	/// </summary>
	public sealed class SqlDbSharpInfoMessageEventArgs : EventArgs
	{
		/// <summary>
		/// Internal constructor.
		/// </summary>
		/// <param name="exception"></param>
		internal SqlDbSharpInfoMessageEventArgs(SqlDbSharpException exception)
		{
			this.exception = exception;
		}

		/// <summary>
		/// True if exists eny errors that should be serialized.
		/// </summary>
		/// <returns></returns>
		private bool ShouldSerializeErrors()
		{
			if (this.exception != null)
			{
				return (0 < this.exception.Errors.Count);
			}
			return false;
		}

		/// <summary>
		/// Returns a string representation of the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Message;
		}

		/// <summary>
		/// Error collection
		/// </summary>
		public SqlDbSharpErrorCollection Errors
		{
			get
			{
				return this.exception.Errors;
			}
		}

		/// <summary>
		/// Message
		/// </summary>
		public string Message
		{
			get
			{
				return this.exception.Message;
			}
		}

		#if !POCKETPC
		/// <summary>
		/// Exception Source.
		/// </summary>
		/// <remarks>Not supported on Compact Framwork 1.0.</remarks>
		public string Source
		{
			get
			{
				return this.exception.Source;
			}
		}
		#endif

		// Fields
		private SqlDbSharpException exception;
	}

	#endregion

	#region SqlDbSharpRowUpdatedEventArgs

	/// <summary>
	/// RowUpdated argument class for Hsql ADO.NET data provider.
	/// </summary>
	public sealed class SqlDbSharpRowUpdatedEventArgs : RowUpdatedEventArgs
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="row"></param>
		/// <param name="command"></param>
		/// <param name="statementType"></param>
		/// <param name="tableMapping"></param>
		public SqlDbSharpRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) : base(row, command, statementType, tableMapping)
		{
		}

		/// <summary>
		/// Command beign executed.
		/// </summary>
		public new SqlDbSharpCommand Command
		{
			get
			{
				return (SqlDbSharpCommand) base.Command;
			}
		}
	}

	#endregion

	#region SqlDbSharpRowUpdatingEventArgs

	/// <summary>
	/// RowUpdating argument class for Hsql ADO.NET data provider.
	/// </summary>
	public sealed class SqlDbSharpRowUpdatingEventArgs : RowUpdatingEventArgs
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="command"></param>
		/// <param name="statementType"></param>
		/// <param name="tableMapping"></param>
		public SqlDbSharpRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) : base(row, command, statementType, tableMapping)
		{
		}

		/// <summary>
		/// Command beign executed.
		/// </summary>
		public new SqlDbSharpCommand Command
		{
			get
			{
				return (SqlDbSharpCommand) base.Command;
			}
		}	
	}

	#endregion
}


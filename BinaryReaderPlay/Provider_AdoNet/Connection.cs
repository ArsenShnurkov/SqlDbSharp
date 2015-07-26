namespace System.Data.SqlDbSharp
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Xml;
    using System.Data.Common;
    using System.Collections.Generic;

    using org.rufwork.mooresDb;

    /// <summary>
	/// Class representing a database connection. 
	/// <seealso cref="SqlDbSharpCommand"/>
	/// <seealso cref="SqlDbSharpReader"/>
	/// <seealso cref="SqlDbSharpParameter"/>
	/// <seealso cref="SqlDbSharpTransaction"/>
	/// <seealso cref="SqlDbSharpDataAdapter"/>
	/// </summary>
	public sealed class SqlDbSharpConnection : DbConnection, IDbConnection, ICloneable
	{
		#region Constructors

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public SqlDbSharpConnection()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Constructor using a connection string.
		/// </summary>
		/// <param name="connectionString"></param>
		public SqlDbSharpConnection(string connectionString)
		{
			GC.SuppressFinalize(this);
			ConnectionString = connectionString;
		}
		
		public override string DataSource {
			get { return String.Empty; }
		}
		[Browsable (false)]
		public override string ServerVersion {
			get { return String.Empty; }
		}
		
		/// <summary>
		/// Private constructor used internally.
		/// </summary>
		/// <param name="connection"></param>
		private SqlDbSharpConnection(SqlDbSharpConnection connection)
		{
			GC.SuppressFinalize(this);
			this._hidePasswordPwd = connection._hidePasswordPwd;
			this._constr = connection._constr;
		}

		#endregion

		#region Public Properties & Methods

		/// <summary>
		/// Get or set the connection string.
		/// </summary>
		public override string ConnectionString
		{
			get { return _connString;  }
			set { 
				_connString = value; 

				_constr = new SqlDbSharpConnectionStringBuilder( _connString );
				_database = _constr.Database;
				_user = _constr.UserName;
				_pwd = _constr.UserPassword;
			}
		}
    
		/// <summary>
		/// Get the current connection timeout.
		/// </summary>
		public int ConnectionTimeout
		{
			get 
			{
				throw new InvalidOperationException("SqlDbSharp Provider does not support this function");
			}
		}

		/// <summary>
		/// Get the current database name.
		/// </summary>
		public override string Database 
		{
			get 
			{
				return _database;
			}
		}

		/// <summary>
		/// Get the current connection state.
		/// </summary>
		public override ConnectionState State 
		{
			get
			{ 
				string tranID = this.LocalTransaction == null ? "null" : this.LocalTransaction.GetHashCode ().ToString ();
				//TracingHelper.Write (string.Format ("Connection {0}, state={1}, Transaction={2}", this.GetHashCode (), _connState.ToString (), tranID));
				return _connState; 
			}
		}

		StateChangeEventHandler _StateChange;
		/// <summary>
		/// StateChange event.
		/// </summary>
		public override event StateChangeEventHandler StateChange
		{
			add
			{
				_StateChange += value;
			}
			remove
			{
				_StateChange -= value;
			}
		}

		private void FireObjectState(ConnectionState original, ConnectionState current)
		{
			if( _StateChange != null )
				_StateChange(this, new StateChangeEventArgs(original, current));
		}


		/// <summary>
		/// Starts a new transaction using the default isolation level (ReadCommitted).
		/// <seealso cref="IsolationLevel"/>
		/// </summary>
		/// <returns>The new <see cref="SqlDbSharpTransaction"/> object.</returns>
		public SqlDbSharpTransaction BeginTransaction()
		{
			return BeginTransaction(IsolationLevel.ReadCommitted);
		}

		protected override DbTransaction BeginDbTransaction (IsolationLevel isolationLevel)
		{
			return this.BeginTransaction(isolationLevel);
		}
		
		/// <summary>
		/// Starts a new transaction using the default isolation level (ReadCommitted).
		/// <seealso cref="IsolationLevel"/>
		/// </summary>
		/// <returns>The new <see cref="SqlDbSharpTransaction"/> object.</returns>
		IDbTransaction IDbConnection.BeginTransaction()
		{
			return this.BeginTransaction(IsolationLevel.ReadCommitted);
		}

		/// <summary>
		/// Starts a new transaction.
		/// <seealso cref="IsolationLevel"/>
		/// </summary>
		/// <param name="level"></param>
		/// <returns>The new <see cref="SqlDbSharpTransaction"/> object.</returns>
		IDbTransaction IDbConnection.BeginTransaction(IsolationLevel level)
		{
			return this.BeginTransaction(level);
		}

		/// <summary>
		/// Starts a new transaction.
		/// <seealso cref="IsolationLevel"/>
		/// </summary>
		/// <param name="level"></param>
		/// <returns>The new <see cref="SqlDbSharpTransaction"/> object.</returns>
		public new SqlDbSharpTransaction BeginTransaction(IsolationLevel level)
		{
			if (this._connState == ConnectionState.Closed)
			{
				throw new InvalidOperationException("Connection is closed.");
			}
			this.CloseDeadReader();
			this.RollbackDeadTransaction();
			if (this.LocalTransaction != null)
			{
				throw new InvalidOperationException("Parallel Transactions Not Supported");
			}
			this.Execute("SET AUTOCOMMIT FALSE");

			return new SqlDbSharpTransaction( this, level );
		}

		/// <summary>
		/// Changes the current database for this connection.
		/// </summary>
		/// <remarks>Not currently supported.</remarks>
		/// <param name="databaseName"></param>
		public override void ChangeDatabase(string databaseName)
		{
			throw new InvalidOperationException("SqlDbSharp Provider does not support this function");
		}

		/// <summary>
		/// Closes the current connection.
		/// </summary>
		public override void Close()
		{
			switch (this._connState)
			{
				case ConnectionState.Closed:
					return;
				case ConnectionState.Open:
					if( _channel != null )
					{
						CloseReader();

						if (this._connState != ConnectionState.Open)
							return;

						if (this.LocalTransaction != null)
						{
							this.LocalTransaction.Rollback();
						}
						else
						{
							this.RollbackDeadTransaction();
						}
						_channel.Disconnect();
						_channel = null;
						_connState = ConnectionState.Closed;
						this.FireObjectState(ConnectionState.Open, ConnectionState.Closed);
					}
					break;
				default:
					return;
			}
		}

		/// <summary>
		/// Creates a new SqlDbSharpCommand object.
		/// </summary>
		/// <returns>A new SqlDbSharpCommand object.</returns>
		public SqlDbSharpCommand CreateCommand()
		{
			return new SqlDbSharpCommand(String.Empty, this);
		}
		protected override DbCommand CreateDbCommand ()
		{
			return CreateCommand ();
		}
		/// <summary>
		/// Creates a new SqlDbSharpCommand object.
		/// </summary>
		/// <returns>A new SqlDbSharpCommand object.</returns>
		IDbCommand IDbConnection.CreateCommand()
		{
			return CreateCommand();
		}

		/// <summary>
		/// Open the current connection.
		/// </summary>
		public override void Open()
		{
			switch (this._connState)
			{
				case ConnectionState.Closed:
					Database db = DatabaseController.GetDatabase( _database );
					_channel = db.Connect(_user,_pwd);
					_connState = ConnectionState.Open;
					this.FireObjectState(ConnectionState.Closed, ConnectionState.Open);
					break;
				case ConnectionState.Open:
					throw new InvalidOperationException("Connection Already Open");
			}
		}

		/// <summary>
		/// InfoMessage event.
		/// </summary>
		public event SqlDbSharpInfoMessageEventHandler InfoMessage;

		/// <summary>
		/// Get a clone of the current instance.
		/// </summary>
		/// <returns></returns>
		public SqlDbSharpConnection Clone()
		{
			return new SqlDbSharpConnection(this);
		}

		/// <summary>
		/// Get a clone of the current instance.
		/// </summary>
		/// <returns></returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion

		#region Dispose Methods

		/// <summary>
		/// Clean up used resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				switch (this._connState)
				{
					case ConnectionState.Open:
					{
						this.Close();
						break;
					}
				}
				this._constr = null;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Rollbacks any dead transaction before doing something else.
		/// </summary>
		internal void RollbackDeadTransaction()
		{
			if ((this._localTransaction != null) && !this._localTransaction.IsAlive)
			{
				this.InternalRollback();
			}
		}

		/// <summary>
		/// Executes a rollback command on the database.
		/// </summary>
		internal void InternalRollback()
		{
			this.Execute("ROLLBACK TRANSACTION");
			this.LocalTransaction = null;
		}

		/// <summary>
		/// Closes any active reader before doing something else.
		/// </summary>
		internal void CloseDeadReader()
		{
			if ((this._reader == null) || this._reader.IsAlive)
			{
				return;
			}
			this._reader = null;
		}

		/// <summary>
		/// Connection state
		/// </summary>
		internal ConnectionState	_connState = ConnectionState.Closed;
		/// <summary>
		/// Connection string
		/// </summary>
		internal string				_connString = String.Empty;
		/// <summary>
		/// Database name
		/// </summary>
		internal string				_database =  String.Empty;

		/// <summary>
		/// Active reader using this connection.
		/// </summary>
		internal SqlDbSharpReader Reader
		{
			get
			{
				if (this._reader != null)
				{
					SqlDbSharpReader reader = (SqlDbSharpReader) this._reader.Target;
					if ((reader != null) && this._reader.IsAlive)
					{
						return reader;
					}
				}
				return null;
			}
			set
			{
				this._reader = null;
				if (value != null)
				{
					this._reader = new WeakReference(value);
				}
			}
		}
 
		/// <summary>
		/// Database instance associated with this connection.
		/// </summary>
		internal Database InternalDatabase
		{
			get
			{
				if (_channel != null)
				{
					return _channel.Database;
				}
				else
					return null;
			}
		}

		/// <summary>
		/// Internal SqlDbSharp channel associated with this connection.
		/// </summary>
		internal Channel Channel
		{
			get
			{
				return _channel;
			}
		}

		/// <summary>
		/// Executes the sql query and return the results.
		/// </summary>
		/// <param name="sqlBatch"></param>
		/// <returns></returns>
		internal Result Execute(string sqlBatch)
		{
			if (this._connState == ConnectionState.Closed)
			{
				throw new InvalidOperationException("Connection is closed");
			}
			this.CloseDeadReader();
			this.RollbackDeadTransaction();

			Result _result = this._channel.Execute(sqlBatch);
			CheckForError( _result );
			return _result;
		}

		#endregion

		#region Private Methods

		private void CheckForError( Result _result )
		{
			if( _result != null && _result.Error != null && _result.Error != string.Empty )
			{
				throw new SqlDbSharpException( _result.Error );
			}
		}

		private void CloseReader()
		{
			if (this._reader == null)
			{
				return;
			}
			SqlDbSharpReader reader = (SqlDbSharpReader) this._reader.Target;
			if ((reader != null) && this._reader.IsAlive)
			{
				if (!reader.IsClosed)
				{
					reader.Close();
				}
			}
			this._reader = null;
		}

		private void FireInfoMessage( SqlDbSharpException ex )
		{
			if( InfoMessage!= null )
				InfoMessage(this, new SqlDbSharpInfoMessageEventArgs( ex ) );
		}

		#endregion

		#region Internal Vars

		/// <summary>
		/// Local transaction object used internally.
		/// </summary>
		internal SqlDbSharpTransaction LocalTransaction = null;

		#endregion

		#region Private Vars

		private string				_user = String.Empty;
		private string				_pwd = String.Empty;
		private SqlDbSharpConnectionStringBuilder _constr;
		private bool _hidePasswordPwd;
		private WeakReference _localTransaction = null;
		private WeakReference _reader;
		private Channel _channel;

		#endregion
		
		/// <summary>
		/// Gets the db provider factory.
		/// </summary>
		/// <value>The db provider factory.</value>
		protected /*internal*/ override DbProviderFactory DbProviderFactory
		{
			get
			{
				return SqlDbSharpDbProviderFactory.Instance;
			}
		}
		public bool ParseViaFramework
		{
			get { return true; }
		}
		static public SortedList<string, string> ParseConnectionString(string connStr)
		{
			var list = new SortedList<string, string> ();
			return list;
		}
		static public SortedList<string, string> ParseConnectionStringViaFramework(string connStr, bool f)
		{
			var list = new SortedList<string, string> ();
			return list;
		}
		static public string FindKey(SortedList<string, string> opts, string keyName, string specification)
		{
			return specification;
		}
	}
}
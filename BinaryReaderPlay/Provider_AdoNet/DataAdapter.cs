namespace System.Data.SqlDbSharp
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Collections;

    /// <summary>
	/// Data adapter class for SqlDbSharp.
	/// <seealso cref="SqlDbSharpConnection"/>
	/// <seealso cref="SqlDbSharpReader"/>
	/// <seealso cref="SqlDbSharpParameter"/>
	/// <seealso cref="SqlDbSharpTransaction"/>
	/// <seealso cref="SqlDbSharpCommand"/>
	/// </summary>
	public sealed class SqlDbSharpDataAdapter : DbDataAdapter, IDbDataAdapter, ICloneable, IDataAdapter
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public SqlDbSharpDataAdapter() : base()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Constructor using a <see cref="SqlDbSharpCommand"/> object.
		/// </summary>
		/// <param name="selectCommand"></param>
		public SqlDbSharpDataAdapter(SqlDbSharpCommand selectCommand) : this()
		{
			_selectCommand = selectCommand;
		}

		/// <summary>
		/// Internal constructor used for cloning.
		/// </summary>
		/// <param name="adapter"></param>
		internal SqlDbSharpDataAdapter(DbDataAdapter adapter) : base(adapter)
		{
		}
 
		/// <summary>
		/// Constructor using a command text string.
		/// </summary>
		/// <param name="selectCommandText"></param>
		public SqlDbSharpDataAdapter(string selectCommandText) : this()
		{
			_selectCommand = new SqlDbSharpCommand(selectCommandText, new SqlDbSharpConnection());
		}

		/// <summary>
		/// Constructor using a command text string and a select connection string.
		/// </summary>
		/// <param name="selectCommandText"></param>
		/// <param name="selectConnectionString"></param>
		public SqlDbSharpDataAdapter(string selectCommandText, string selectConnectionString)
		{
			_selectCommand = new SqlDbSharpCommand(selectCommandText, new SqlDbSharpConnection(selectConnectionString));
		}
 
		/// <summary>
		/// Constructor using a command text string and a select connection object.
		/// </summary>
		/// <param name="selectCommandText"></param>
		/// <param name="selectConnection"></param>
		public SqlDbSharpDataAdapter(string selectCommandText, SqlDbSharpConnection selectConnection) : this()
		{
			_selectCommand = new SqlDbSharpCommand(selectCommandText, selectConnection);
		}

		#endregion

		#region DbDataAdapter Overrides

		/// <summary>
		/// Creates a new <see cref="RowUpdatedEventArgs"/> to fire the RowUpdated event.
		/// </summary>
		/// <param name="dataRow"></param>
		/// <param name="command"></param>
		/// <param name="statementType"></param>
		/// <param name="tableMapping"></param>
		/// <returns></returns>
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new SqlDbSharpRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		/// <summary>
		/// Creates a new <see cref="RowUpdatingEventArgs"/> to fire the RowUpdating event.
		/// </summary>
		/// <param name="dataRow"></param>
		/// <param name="command"></param>
		/// <param name="statementType"></param>
		/// <param name="tableMapping"></param>
		/// <returns></returns>
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new SqlDbSharpRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		/// <summary>
		/// Fires the RowUpdated event.
		/// </summary>
		/// <param name="value"></param>
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			#if !POCKETPC
			SqlDbSharpRowUpdatedEventHandler handler = (SqlDbSharpRowUpdatedEventHandler) base.Events[EventRowUpdated];
			if ((handler != null) && (value is SqlDbSharpRowUpdatedEventArgs))
			{
				handler(this, (SqlDbSharpRowUpdatedEventArgs) value);
			}
			#endif
		}

		/// <summary>
		/// Fires the RowUpdating event.
		/// </summary>
		/// <param name="value"></param>
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			#if !POCKETPC
			SqlDbSharpRowUpdatingEventHandler handler = (SqlDbSharpRowUpdatingEventHandler) base.Events[EventRowUpdating];
			if ((handler != null) && (value is SqlDbSharpRowUpdatingEventArgs))
			{
				handler(this, (SqlDbSharpRowUpdatingEventArgs) value);
			}
			#endif
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Get or set the select command used.
		/// </summary>
		public SqlDbSharpCommand SelectCmd
		{
			get { return _selectCommand;  }
			set { _selectCommand = value; }
		}

		/// <summary>
		/// Get or set the update command used.
		/// </summary>
		public SqlDbSharpCommand UpdateCmd
		{
			get { return _updateCommand;  }
			set { _updateCommand = value; }
		}

		/// <summary>
		/// Get or set the insert command used.
		/// </summary>
		public SqlDbSharpCommand InsertCmd
		{
			get { return _insertCommand;  }
			set { _insertCommand = value; }
		}

		/// <summary>
		/// Get or set the delete command used.
		/// </summary>
		public SqlDbSharpCommand DeleteCmd
		{
			get { return _deleteCommand;  }
			set { _deleteCommand = value; }
		}

		#endregion

        #region IDbDataAdapter
        IDbCommand IDbDataAdapter.DeleteCommand {
            get { return DeleteCmd; }
            set { throw new NotImplementedException (); }
        }

        IDbCommand IDbDataAdapter.InsertCommand {
            get { return InsertCmd; }
            set { throw new NotImplementedException (); }
        }

        IDbCommand IDbDataAdapter.SelectCommand {
            get { return SelectCmd; }
            set { throw new NotImplementedException (); }
        }

        IDbCommand IDbDataAdapter.UpdateCommand {
            get { return UpdateCmd; }
            set { throw new NotImplementedException (); }
        }
        #endregion

		#region Dispose Methods

		/// <summary>
		/// Clean up any used resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.tableMappings = null;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Internal Methods

		internal int IndexOfDataSetTable(string dataSetTable)
		{
			if (this.tableMappings != null)
			{
				return ((DataTableMappingCollection)this.TableMappings).IndexOfDataSetTable(dataSetTable);
			}
			return -1;
		}

		internal DataTableMapping GetTableMappingBySchemaAction(string sourceTableName, string dataSetTableName, MissingMappingAction mappingAction)
		{
			return DataTableMappingCollection.GetTableMappingBySchemaAction(this.tableMappings, sourceTableName, dataSetTableName, mappingAction);
		}

		internal void ClearDataSet(DataSet dataSet)
		{
			dataSet.Reset();
		}

		#endregion

		#region Public Events

		#if !POCKETPC
		/// <summary>
		/// RowUpdated event.
		/// </summary>
		/// <remarks>Not supportes on Compact Framework 1.0</remarks>
		public event SqlDbSharpRowUpdatedEventHandler RowUpdated
		{
			add
			{
				base.Events.AddHandler(EventRowUpdated, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventRowUpdated, value);
			}
		}

		/// <summary>
		/// Row updating event.
		/// </summary>
		/// <remarks>Not supportes on Compact Framework 1.0</remarks>
		public event SqlDbSharpRowUpdatingEventHandler RowUpdating
		{
			add
			{
				SqlDbSharpRowUpdatingEventHandler handler = (SqlDbSharpRowUpdatingEventHandler) base.Events[EventRowUpdating];
				if ((handler != null) && (value.Target is CommandBuilder))
				{
					SqlDbSharpRowUpdatingEventHandler builder = (SqlDbSharpRowUpdatingEventHandler) CommandBuilder.FindBuilder(handler);
					if (builder != null)
					{
						base.Events.RemoveHandler(EventRowUpdating, builder);
					}
				}
				base.Events.AddHandler(EventRowUpdating, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventRowUpdating, value);
			}
		}
		#endif

		#endregion

		#region Private Fields

		internal static readonly object EventRowUpdated = null;
		internal static readonly object EventRowUpdating = null;

		private SqlDbSharpCommand _selectCommand = null;
		private SqlDbSharpCommand _deleteCommand = null;
		private SqlDbSharpCommand _insertCommand = null;
		private SqlDbSharpCommand _updateCommand = null;

		////////////////////
		// Private Data Members
		////////////////////
		//private bool acceptChangesDuringFill;
		//private bool continueUpdateOnError;
		//private MissingMappingAction missingMappingAction;
		//private MissingSchemaAction missingSchemaAction;
		private DataTableMappingCollection tableMappings;

		#endregion

		#region ICloneable Members

		/// <summary>
		/// Returns a clone of the current instance.
		/// </summary>
		/// <returns>A new <see cref="SqlDbSharpDataAdapter"/> object clone of the current.</returns>
		public SqlDbSharpDataAdapter Clone()
		{
			#if !POCKETPC
			return new SqlDbSharpDataAdapter(this);
			#else
			return new SqlDbSharpDataAdapter(this);
			#endif
		}

		/// <summary>
		/// Returns a clone of the current instance.
		/// </summary>
		/// <returns>A new <see cref="SqlDbSharpDataAdapter"/> object clone of the current.</returns>
		object ICloneable.Clone()
		{	
			return Clone();
		}

		#endregion

		#region IDataAdapter Members

		/// <summary>
		/// Fills a <see cref="DataSet"/> object.
		/// </summary>
		/// <param name="dataSet"></param>
		/// <returns></returns>
		public override int Fill(DataSet dataSet)
		{
			return base.Fill( dataSet );
		}

		/// <summary>
		/// Get the fill parameters.
		/// </summary>
		/// <returns></returns>
		public override IDataParameter[] GetFillParameters()
		{
			return base.GetFillParameters();
		}

		/// <summary>
		/// Fills the schema.
		/// </summary>
		/// <param name="dataSet"></param>
		/// <param name="schemaType"></param>
		/// <returns>The schema <see cref="DataTable"/>.</returns>
		public override DataTable[] FillSchema(DataSet dataSet, System.Data.SchemaType schemaType)
		{
			return base.FillSchema( dataSet, schemaType );
		}

		/// <summary>
		/// Update the database using the passed <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet"></param>
		/// <returns></returns>
		public override int Update(DataSet dataSet)
		{
			return base.Update( dataSet );
		}

		#endregion
	}
}

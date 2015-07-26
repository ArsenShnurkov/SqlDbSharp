namespace System.Data.SqlDbSharp
{
    using System;
    using System.ComponentModel;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using System.Data.Common;

    using org.rufwork.mooresDb;
    using org.rufwork.mooresDb.infrastructure.tableParts;

	/// <summary>
	/// Reader class for Hsql ADO.NET data provider.
	/// <seealso cref="SqlDbSharpConnection"/>
	/// <seealso cref="SqlDbSharpParameter"/>
	/// <seealso cref="SqlDbSharpTransaction"/>
	/// <seealso cref="SqlDbSharpCommand"/>
	/// <seealso cref="SqlDbSharpDataAdapter"/>
	/// </summary>
	public sealed class SqlDbSharpReader : DbDataReader, IDataReader, IDisposable
	{
		#region Class Constructors

		/// <summary>
		/// Internal constructor
		/// </summary>
		/// <param name="command"></param>
		internal SqlDbSharpReader( SqlDbSharpCommand command ) 
		{
			_first = true;
			_command = command;
			_behavior = CommandBehavior.Default;
			_rs = command.Result;
			_columns = new Hashtable();
			int count = _rs.ColumnCount;
            for (int i = 0; i < count; i++)
            {
                throw new NotImplementedException ();// MERGE_AS _columns.Add (_rs.Label [i], i);
            }
		}

		#endregion

		#region IDataReader Members

		/// <summary>
		/// Returns the count of recods affected by the last execution.
		/// </summary>
		public override int RecordsAffected
		{
			get
			{
				if (this._command != null)
				{
					return this._rs.UpdateCount;
				}
				return this._recordsAffected;
			}
		}

		/// <summary>
		/// Returns True is this reader is in closed state.
		/// </summary>
		public override bool IsClosed
		{
			get
			{
				return this._isClosed;
			}
		}

		/// <summary>
		/// Fetches the next result set.
		/// </summary>
		/// <remarks>Not currently supported.</remarks>
		/// <returns>True if the operation was performed sucessfuly.</returns>
		public override bool NextResult()
		{
			return false;
		}

		/// <summary>
		/// Closed the active open reader and frees any used resources.
		/// </summary>
		public override void Close()
		{
			if (this.IsClosed)
			{
				return;
			}
			this.InternalClose(true);
		}

		/// <summary>
		/// Read the next row from the results.
		/// </summary>
		/// <returns>True if the read operation was sucessful.</returns>
		public override bool Read()
		{
			if( _first )
			{
                throw new NotImplementedException(); // MERGE_AS_current = _rs.Root;
				_first = false;
			}
			else
			{
                throw new NotImplementedException(); // MERGE_AS_current = _current.Next;
			}

			if( _current == null )
				return false;
			else
				return true;
		}

		/// <summary>
		/// Returns the depth of the current results.
		/// </summary>
		/// <remarks>Not currently supported.</remarks>
		public override int Depth
		{
			get
			{
				if (this.IsClosed)
				{
					throw new InvalidOperationException("Data Reader is Closed");
				}
				return 0;
			}
		}

		/// <summary>
		/// Gets the schema table from the reader metadata.
		/// </summary>
		/// <returns></returns>
		public override DataTable GetSchemaTable()
		{
			if (this._schemaTable == null)
			{
				this._schemaTable = this.BuildSchemaTable();
			}
			return this._schemaTable;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Cleans any used resources.
		/// </summary>
		void System.IDisposable.Dispose()
		{
			this.Close();
			GC.SuppressFinalize(this);
		}

		#endregion
		
		#region IDataRecord Members

		/// <summary>
		/// Get the Int32 value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override int GetInt32(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (Int32)_current.Data[i];
		}
		
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public override IEnumerator GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
		
		/// <summary>
		/// Get the value of the specified column name.
		/// </summary>
		public override object this[string name]
		{
			get
			{
                throw new NotImplementedException(); // MERGE_AS
                /*
				int index = GetColumnIndex(name);
				if( _current.Data[ index ] == null )
					return DBNull.Value;
				else
					return _current.Data[ index ];
     */               
			}
		}
		
		/// <summary>
		/// Gets the <see cref="System.Data.SqlDbSharp.SqlDbSharpReader"/> with the specified ordinal.
		/// </summary>
		/// <param name="ordinal">Ordinal.</param>
		public override object this [int ordinal]
		{
			get
			{
				return GetValue(ordinal);
			}
		}

		/// <summary>
		/// Get the value from the specified column index.
		/// </summary>
		object System.Data.IDataRecord.this[int i]
		{
			get
			{
                throw new NotImplementedException(); // MERGE_AS
                /*
				if( _current.Data[i] == null )
					return DBNull.Value;
				else
                    return _current.Data[i];
                    */
			}
		}

		/// <summary>
		/// Get the value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override object GetValue(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return _current.Data[i];
		}

		/// <summary>
		/// Check for null value of a column.
		/// </summary>
		/// <param name="i"></param>
		/// <returns>True if the passed column index contains a null value.</returns>
		public override bool IsDBNull(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (_current.Data[i] == null);
		}

		/// <summary>
		/// Read bytes from a binary column.
		/// </summary>
		/// <param name="i">Column index.</param>
		/// <param name="fieldOffset">Offset of the field where start reading.</param>
		/// <param name="buffer">Buffer where read data will be left.</param>
		/// <param name="bufferOffset">Offset of the buffer where start writing.</param>
		/// <param name="length">Number of bytes to read.</param>
		/// <returns>The number of bytes read.</returns>
		/// <remarks>
		/// If passed a null value as buffer, the method will return the total length
		/// of the data contained in the database field.
		/// </remarks>
		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
            throw new NotImplementedException(); // MERGE_AS byte[] data = (byte[])_current.Data[i];

			if( buffer != null )
			{
                throw new NotImplementedException (); // MERGE_AS				Array.Copy(data, fieldOffset, buffer, bufferOffset, length);
				return length;
			}
			else
			{
                throw new NotImplementedException (); // MERGE_AS return data.Length;
			}

		}

		/// <summary>
		/// Get the Byte value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override byte GetByte(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (byte)_current.Data[i];
		}

		/// <summary>
		/// Get the Type of the data stored on the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override Type GetFieldType(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return GetDataType( _rs.Type[i] );
		}

		/// <summary>
		/// Get the Decimal value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override decimal GetDecimal(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (Decimal)_current.Data[i];
		}

		/// <summary>
		/// Get the column values from the row in an array.
		/// </summary>
		/// <param name="values">An object array containing the column values.</param>
		/// <returns>The count of values read.</returns>
		public override int GetValues(object[] values)
		{
			if (this._current == null)
			{
				throw new InvalidOperationException("Invalid Read.");
			}
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
            throw new NotImplementedException(); // MERGE_AS 
/*
            int count = (values.Length < this._current.Data.Length) ? values.Length : this._current.Data.Length;
			for (int i = 0; i < count; i++)
			{
                throw new NotImplementedException(); // MERGE_AS values[i] = _current.Data[i];
			}
			return count;
   */         
		}

		/// <summary>
		/// Gets the name of the specified column by index.
		/// </summary>
		/// <param name="i">Column index.</param>
		/// <returns>Column name.</returns>
		public override string GetName(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return _rs.Label[i];
		}

		/// <summary>
		/// Returns the current field count.
		/// </summary>
		public override int FieldCount
		{
			get
			{
				return _rs.ColumnCount;
			}
		}

		/// <summary>
		/// Get the Int64 value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override long GetInt64(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (Int64)_current.Data[i];
		}

		/// <summary>
		/// Get the Double value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override double GetDouble(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (Double)_current.Data[i];
		}

		/// <summary>
		/// Get the Boolean value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override bool GetBoolean(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (Boolean)_current.Data[i];
		}

		/// <summary>
		/// Get the Guid value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override Guid GetGuid(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (Guid)_current.Data[i];
		}

		/// <summary>
		/// Get the DateTime value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override DateTime GetDateTime(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (DateTime)_current.Data[i];
		}

		/// <summary>
		/// Obtinains the column index using the name.
		/// </summary>
		/// <param name="name">The column name.</param>
		/// <returns>The column index, or an exception if not found.</returns>
		public override int GetOrdinal(string name)
		{
			return GetColumnIndex(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override string GetDataTypeName(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return _current.Data[i].GetType().Name;
		}

		/// <summary>
		/// Get the Float value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override float GetFloat(int i)
		{
            throw new NotImplementedException(); // MERGE_AS return (float)_current.Data[i];
		}

		/// <summary>
		/// GetData method not supported.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public new IDataReader GetData(int i)
		{
			throw new NotSupportedException("GetData method not supported.");
		}

		/// <summary>
		/// Read characters from a string column.
		/// </summary>
		/// <param name="i">Column index.</param>
		/// <param name="fieldOffset">Offset of the field where start reading.</param>
		/// <param name="buffer">Buffer where read data will be left.</param>
		/// <param name="bufferOffset">Offset of the buffer where start writing.</param>
		/// <param name="length">Number of bytes to read.</param>
		/// <returns>The number of characters read.</returns>
		/// <remarks>
		/// If passed a null value as buffer, the method will return the total length
		/// of the data contained in the database field.
		/// </remarks>
		public override long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			char[] data = null;
            throw new NotImplementedException(); // MERGE_AS
            /*
			if( _current.Data[i].GetType() == typeof(string) )
				data = _current.Data[i].ToString().ToCharArray();
			else
				data = (char[])_current.Data[i];
*/
			if( buffer != null )
			{
				#if !POCKETPC
				Array.Copy(data, fieldOffset, buffer, bufferOffset, length);
				#else
				Array.Copy(data, (int)fieldOffset, buffer, (int)bufferOffset, length);
				#endif
				return length;
			}
			else
			{
				return data.Length;
			}			
		}

		/// <summary>
		/// Get the String value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override string GetString(int i)
		{
            throw new NotImplementedException(); // MERGE_AS  return _current.Data[i] as string;
		}

		/// <summary>
		/// Get the Char value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override char GetChar(int i)
		{
            throw new NotImplementedException(); // MERGE_AS    return Convert.ToChar(_current.Data[i]);
		}

		/// <summary>
		/// Get the Int16 value from the specified column index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override short GetInt16(int i)
		{
            throw new NotImplementedException(); // MERGE_AS    return (Int16)_current.Data[i];
		}

		#endregion

		#region Private & Internal Methods

		internal DataTable BuildSchemaTable()
		{
			int columns = _rs.ColumnCount;
			DataTable schema = CreateSchemaTable(null, columns );
			for (int i = 0; i < columns; i++)
			{
                throw new NotImplementedException(); // MERGE_AS  
                /*
				string column = _rs.Label[i];
				Database db = _command._connection.InternalDatabase;				
				Table table = db.GetTable(_rs.Table[i], _command._connection.Channel );
				int index = table.GetColumnNumber(column);
				Column col = table.GetColumn(index);

				DataRow row = schema.NewRow();
				row["ColumnName"] = column;
				row["ColumnOrdinal"] = i;

				row["ColumnSize"] = 0;
				row["ProviderType"] = _rs.Type[i];
				row["DataType"] = GetDataType(_rs.Type[i]);
				row["AllowDBNull"] = col.IsNullable;
				row["IsIdentity"] = col.IsIdentity;
				row["IsAutoIncrement"] = col.IsIdentity;

				if (this._browseModeInfoConsumed)
				{
					row["IsAliased"] = (_rs.Label[i] != _rs.Name[i]);
					Index ix = table.PrimaryIndex;
					if( ix != null )
					{
						ArrayList a = new ArrayList( ix.Columns );
						row["IsKey"] = (a.IndexOf(i) != -1);						
					}
					else
					{
						row["IsKey"] = false;
					}
					row["IsHidden"] = "";
					row["IsExpression"] = (_rs.Table[i] == null);
				}

				row["IsLong"] = false;//data1.metaType.IsLong;
				if (col.ColumnType == ColumnType.Timestamp)
				{
					row["IsUnique"] = true;
					row["IsRowVersion"] = true;
				}
				else
				{
					row["IsUnique"] = false;
					row["IsRowVersion"] = false;
				}
				row["IsReadOnly"] = false;

				row["NumericPrecision"] = 0;
				row["NumericScale"] = 0;

				row["BaseServerName"] = null;
				row["BaseCatalogName"] = db.Name;
				row["BaseSchemaName"] = null;
				row["BaseTableName"] = table.Name;
				row["BaseColumnName"] = column;
				schema.Rows.Add(row);
   */			}
			DataColumnCollection c = schema.Columns;
			for (int i = 0; i < c.Count; i++)
				c[i].ReadOnly = true;

			return schema;
         
		}

		private static Type GetDataType( COLUMN_TYPES type )
		{
            throw new NotImplementedException (); // MERGE_AS

			switch( type )
			{
//            case COLUMN_TYPES.BigInt:
//					return typeof(long);
//            case COLUMN_TYPES.Binary:
//					return typeof(byte[]);
            case COLUMN_TYPES.BIT:
					return typeof(bool);
//            case COLUMN_TYPES.Char:
//					return typeof(string);
            case COLUMN_TYPES.DATETIME:
                return typeof(DateTime);
            case COLUMN_TYPES.DECIMAL:
					return typeof(Decimal);
//            case COLUMN_TYPES.DbDouble:
//					return typeof(Double);
            case COLUMN_TYPES.FLOAT:
					return typeof(float);
            case COLUMN_TYPES.INT:
					return typeof(int);
//            case COLUMN_TYPES.LongVarBinary:
//					return typeof(byte[]);
//            case COLUMN_TYPES.LongVarChar:
//					return typeof(string);
//            case COLUMN_TYPES.Null:
//					return typeof(DBNull);
//            case COLUMN_TYPES.Numeric:
//					return typeof(Decimal);
//            case COLUMN_TYPES.Other:
//					return typeof(Object);
//            case COLUMN_TYPES.Real:
//					return typeof(float);
//            case COLUMN_TYPES.SmallInt:
//					return typeof(Int16);
//            case COLUMN_TYPES.Time:
//					return typeof(DateTime);
//            case COLUMN_TYPES.Timestamp:
//					return typeof(byte[]);
//            case COLUMN_TYPES.TINYINT:
//					return typeof(byte);
//            case COLUMN_TYPES.VarBinary:
//					return typeof(byte[]);
            case COLUMN_TYPES.CHAR:
					return typeof(string);
//            case COLUMN_TYPES.VarCharIgnoreCase:
//					return typeof(string);
				default:
					return typeof(Object);
			}
		}

		internal static bool IsEmpty(string str)
		{
			if (str != null)
			{
				return (0 == str.Length);
			}
			return true;
		}

		internal static DataTable CreateSchemaTable(DataTable schemaTable, int capacity)
		{
			if (schemaTable == null)
			{
				schemaTable = new DataTable("SchemaTable");
				if (0 < capacity)
				{
					schemaTable.MinimumCapacity = capacity;
				}
			}
			DataColumnCollection collection = schemaTable.Columns;
			AddColumn(collection, null, "ColumnName", typeof(string));
			AddColumn(collection, 0, "ColumnOrdinal", typeof(int));
			AddColumn(collection, null, "ColumnSize", typeof(int));
			AddColumn(collection, null, "NumericPrecision", typeof(short));
			AddColumn(collection, null, "NumericScale", typeof(short));
			AddColumn(collection, null, "IsUnique", typeof(bool));
			AddColumn(collection, null, "IsKey", typeof(bool));
			AddColumn(collection, null, "BaseServerName", typeof(string));
			AddColumn(collection, null, "BaseCatalogName", typeof(string));
			AddColumn(collection, null, "BaseColumnName", typeof(string));
			AddColumn(collection, null, "BaseSchemaName", typeof(string));
			AddColumn(collection, null, "BaseTableName", typeof(string));
			AddColumn(collection, null, "DataType", typeof(object));
			AddColumn(collection, null, "AllowDBNull", typeof(bool));
			AddColumn(collection, null, "ProviderType", typeof(int));
			AddColumn(collection, null, "IsAliased", typeof(bool));
			AddColumn(collection, null, "IsExpression", typeof(bool));
			AddColumn(collection, null, "IsIdentity", typeof(bool));
			AddColumn(collection, null, "IsAutoIncrement", typeof(bool));
			AddColumn(collection, null, "IsRowVersion", typeof(bool));
			AddColumn(collection, null, "IsHidden", typeof(bool));
			AddColumn(collection, false, "IsLong", typeof(bool));
			AddColumn(collection, null, "IsReadOnly", typeof(bool));
			return schemaTable;
		}

		private static void AddColumn(DataColumnCollection columns, object defaultValue, string name, Type type)
		{
			if (columns.Contains(name))
			{
				return;
			}
			DataColumn column = new DataColumn(name, type);
			if (defaultValue != null)
			{
				column.DefaultValue = defaultValue;
			}
			columns.Add(column);
		}

		internal bool BrowseModeInfoConsumed
		{
			set
			{
				this._browseModeInfoConsumed = value;
			}
		}

		private void InternalClose(bool closeReader)
		{
			bool closeConn = CommandBehavior.CloseConnection == (this._behavior & CommandBehavior.CloseConnection);
			Exception exception = null;
			if (closeReader)
			{
				if ((this._command != null) && (this._command.Connection != null))
				{
					((SqlDbSharpConnection)this._command.Connection).Reader = null;
				}
				this._isClosed = true;
				if ((closeConn && (this._command != null)) && (this._command.Connection != null))
				{
					try
					{
						this._command.Connection.Close();
					}
					catch (Exception e)
					{
						exception = e;
					}
				}
				if (this._command != null)
				{
					this._recordsAffected = this._rs.UpdateCount;
				}
				this._command = null;
				this._current = null;
				this._rs = null;
				this._columns = null;
				this._schemaTable = null;
			}
			if (exception != null)
				throw exception;
		}

		private int GetColumnIndex( string name )
		{
			if( !_columns.ContainsKey(name) )
				throw new ArgumentException("The supplied column name is not found.", name);

			return (int)_columns[name];
		}
		
		#endregion

		#region Private Fields

		// Fields
		private Result _rs = null;
		private Record _current = null;
		private CommandBehavior _behavior;
		private SqlDbSharpCommand _command;
		private int _recordsAffected;
		private DataTable _schemaTable;
		private bool _isClosed;
		private bool _browseModeInfoConsumed;
		private Hashtable _columns = null;
		private bool _first;

		#endregion
		public override bool HasRows
		{
			get { throw new NotImplementedException(); }
		}
	}
}
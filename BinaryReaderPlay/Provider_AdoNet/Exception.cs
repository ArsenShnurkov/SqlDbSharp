namespace System.Data.SqlDbSharp
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Runtime.Serialization;

    /// <summary>
	/// Exception class for Hsql ADO.NET data provider.
	/// <seealso cref="SqlDbSharpConnection"/>
	/// <seealso cref="SqlDbSharpReader"/>
	/// <seealso cref="SqlDbSharpParameter"/>
	/// <seealso cref="SqlDbSharpTransaction"/>
	/// <seealso cref="SqlDbSharpCommand"/>
	/// <seealso cref="SqlDbSharpDataAdapter"/>
	/// </summary>
	/// <remarks>Not serializable for Compact Framework 1.0</remarks>
	#if !POCKETPC
	[Serializable]
	#endif
	public sealed class SqlDbSharpException : SystemException
	{
		#region Constructors

		/// <summary>
		/// Internal default constructor.
		/// </summary>
		internal SqlDbSharpException() : base()
		{
			#if !POCKETPC
			base.HResult = -2146232060;
			#endif
		}

		/// <summary>
		/// Constructor using an error string.
		/// </summary>
		/// <param name="error"></param>
		internal SqlDbSharpException( string error ) : this()
		{
			if( error == null )
				throw new ArgumentNullException("error");

			int number = 0;

			try
			{
				#if !POCKETPC
				if( Char.IsDigit( error, 0 ) )
				#else
				if( Char.IsDigit( error.ToCharArray()[0] ) )
				#endif
					number = int.Parse(error.Substring(0, 5));
				else
					number = int.Parse(error.Substring(1, 4));
			}
			catch{}

			string message = error;

			SqlDbSharpError e = new SqlDbSharpError( message, number, String.Empty, String.Empty);

			this.Errors.Add( e );
		}

		#endregion 

		#region Serialization methods

		#if !POCKETPC
		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <remarks>Not supported on Compact Framework 1.0</remarks>
		/// <param name="si"></param>
		/// <param name="sc"></param>
		private SqlDbSharpException(SerializationInfo si, StreamingContext sc) : this()
		{
			this._errors = (SqlDbSharpErrorCollection) si.GetValue("Errors", typeof(SqlDbSharpErrorCollection));
		}
		#endif

		#if !POCKETPC
		/// <summary>
		/// Serialization method.
		/// </summary>
		/// <remarks>Not supported on Compact Framework 1.0</remarks>
		/// <param name="si"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (si == null)
			{
				throw new ArgumentNullException("si");
			}
			si.AddValue("Errors", this._errors, typeof(SqlDbSharpErrorCollection));
			base.GetObjectData(si, context);
		}
		#endif

		#endregion

		#region Public Properties

		/// <summary>
		/// Error collection.
		/// </summary>
		public SqlDbSharpErrorCollection Errors
		{
			get
			{
				if (this._errors == null)
				{
					this._errors = new SqlDbSharpErrorCollection();
				}
				return this._errors;
			}
		}

		/// <summary>
		/// Exception message.
		/// </summary>
		public override string Message
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < this.Errors.Count; i++)
				{
					if (i > 0)
					{
						builder.Append("\r\n");
					}
					builder.Append(((SqlDbSharpError)this.Errors[i]).Message);
				}
				return builder.ToString();
			}
		}

		/// <summary>
		/// Exception error number.
		/// </summary>
		public int Number
		{
			get
			{
				return this.Errors[0].Number;
			}
		}

		/// <summary>
		/// Procedure where the exception was generated.
		/// </summary>
		public string Procedure
		{
			get
			{
				return this.Errors[0].Procedure;
			}
		}

		#if !POCKETPC
		/// <summary>
		/// Source of the error.
		/// </summary>
		/// <remarks>Not supported on Compact Framework 1.0</remarks>
		public override string Source
		{
			get
			{
				return this.Errors[0].Source;
			}
		}
		#endif

		#endregion

		#region Private Vars

		// Fields
		private SqlDbSharpErrorCollection _errors;

		#endregion
	}
 

}

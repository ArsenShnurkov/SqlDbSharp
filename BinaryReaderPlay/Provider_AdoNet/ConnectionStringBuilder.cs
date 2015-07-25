namespace System.Data.SqlDbSharp
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;

    /// <summary>
	/// Helper static class for building SqlDbSharp connection strings.
	/// </summary>
	internal sealed class SqlDbSharpConnectionStringBuilder : DbConnectionStringBuilder
	{
		#region Constructors

		/// <summary>
		/// Static constructor.
		/// </summary>
		static SqlDbSharpConnectionStringBuilder()
		{
			invariantComparer = CultureInfo.InvariantCulture.CompareInfo;
		}

		/// <summary>
		/// Creates a new <see cref="SqlDbSharpConnectionString"/> object
		/// using a connection string.
		/// </summary>
		/// <param name="connstring"></param>
		internal SqlDbSharpConnectionStringBuilder( string connstring )
		{
			if( connstring == null || connstring.Length == 0 || connstring.Trim().Length == 0 )
				throw new ArgumentNullException("connstring");

			string[] pairs = connstring.Split(';');
			
			if( pairs.Length < 3 )
				throw new ArgumentException("The connection string is invalid.", "connstring");

			for( int i=0;i<pairs.Length;i++)
			{
				if( pairs[i].Trim() == String.Empty )
					continue;

				string[] pair = pairs[i].Split('=');
				
				if( pair.Length != 2 )
					throw new ArgumentException("The connection string has an invalid parameter.", "connstring");

				string key = pair[0].ToLower().Trim();
				string value = pair[1].ToLower().Trim();

				if( invariantComparer.Compare( key, Initial_Catalog) == 0 ||  
					invariantComparer.Compare( key, DB ) == 0 )
				{
					Database = value;
				}
				if( invariantComparer.Compare( key, User_ID) == 0 ||  
					invariantComparer.Compare( key, UID ) == 0 )
				{
					UserName = value;
				}
				if( invariantComparer.Compare( key, Password) == 0 ||  
					invariantComparer.Compare( key, Pwd ) == 0 )
				{
					UserPassword = value;
				}
			}

			if( Database == string.Empty )
				throw new ArgumentException("Database parameter is invalid in connection string.", "Database");

			if( UserName == string.Empty )
				throw new ArgumentException("UserName parameter is invalid in connection string.", "UserName");

			_connstring = connstring;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Returns the connection string built.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _connstring;
		}

		#endregion

		#region Public Fields

		/// <summary>
		/// Database name.
		/// </summary>
		public string Database = String.Empty;
		/// <summary>
		/// User name.
		/// </summary>
		public string UserName = String.Empty;
		/// <summary>
		/// User password.
		/// </summary>
		public string UserPassword = String.Empty;

		#endregion

		#region Internal Vars

		/// <summary>
		/// Class used internally for comparisons.
		/// </summary>
		internal static CompareInfo invariantComparer;

		#endregion

		#region Internal String Constants

		internal const string Initial_Catalog = "initial catalog";
		internal const string DB = "database";
		internal const string User_ID = "user id";
		internal const string UID = "uid";
		internal const string Pwd = "pwd";
		internal const string Password = "password";

		#endregion

		#region Private Vars

		private string _connstring = String.Empty;

		#endregion
	}
}

namespace System.Data.SqlDbSharp
{
	using System.Data.Common;
	
	/// <summary>
	/// Sharp hsql db provider factory.
	/// </summary>
	public class SqlDbSharpDbProviderFactory : DbProviderFactory
	{
		/// <summary>
		/// Static instance member which returns an instanced SqlDbSharpFactory class.
		/// </summary>
		public static readonly SqlDbSharpDbProviderFactory Instance = new SqlDbSharpDbProviderFactory();	
		/// <summary>
		/// Returns a new SqlDbSharpCommand object.
		/// </summary>
		/// <returns>A SqlDbSharpCommand object.</returns>
		public override DbCommand CreateCommand()
		{
			return new SqlDbSharpCommand();
		}
		/// <summary>
		/// Returns a new SqlDbSharpCommandBuilder object.
		/// </summary>
		/// <returns>A SqlDbSharpCommandBuilder object.</returns>
		public override DbCommandBuilder CreateCommandBuilder()
		{
			return new SqlDbSharpCommandBuilder();
		}
		/// <summary>
		/// Creates a new SqlDbSharpConnection.
		/// </summary>
		/// <returns>A SqlDbSharpConnection object.</returns>
		public override DbConnection CreateConnection()
		{
			return new SqlDbSharpConnection();
		}

		/// <summary>
		/// Creates a new SqlDbSharpConnectionStringBuilder.
		/// </summary>
		/// <returns>A SqlDbSharpConnectionStringBuilder object.</returns>
		public override DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			return new SqlDbSharpConnectionStringBuilder(String.Empty);
		}

		/// <summary>
		/// Creates a new SqlDbSharpDataAdapter.
		/// </summary>
		/// <returns>A SqlDbSharpDataAdapter object.</returns>
		public override DbDataAdapter CreateDataAdapter()
		{
			return new SqlDbSharpDataAdapter();
		}

		/// <summary>
		/// Creates a new SqlDbSharpParameter.
		/// </summary>
		/// <returns>A SqlDbSharpParameter object.</returns>
		public override DbParameter CreateParameter()
		{
			return new SqlDbSharpParameter();
		}
	}
}
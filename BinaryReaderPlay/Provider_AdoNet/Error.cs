namespace System.Data.SqlDbSharp
{
    using System;

	/// <summary>
	/// Error object used in <see cref="SqlDbSharpException"/>.
	/// <seealso cref="SqlDbSharpErrorCollection"/>
	/// <seealso cref="SqlDbSharpException"/>
	/// </summary>
	public struct SqlDbSharpError
	{
		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="number"></param>
		/// <param name="procedure"></param>
		/// <param name="source"></param>
		public SqlDbSharpError( string message, int number, string procedure, string source )
		{
			this.Message = message; 
			this.Number = number; 
			this.Procedure = procedure;
			this.Source = source;
		}

		#endregion

		#region Public Fields

		/// <summary>
		/// Textual description for this error.
		/// </summary>
		public string Message;
		/// <summary>
		/// Error code for this error.
		/// </summary>
		public int Number;
		/// <summary>
		/// Procedure where this error was waised.
		/// </summary>
		public string Procedure;
		/// <summary>
		/// Source module of this error.
		/// </summary>
		public string Source;

		#endregion
	}
}

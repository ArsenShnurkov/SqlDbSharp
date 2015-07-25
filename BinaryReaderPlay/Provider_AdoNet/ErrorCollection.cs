namespace System.Data.SqlDbSharp
{
    using System;
    using System.Collections;

    /// <summary>
	/// Strong typed collection of <see cref="SqlDbSharpError"/> objects used in <see cref="SqlDbSharpException"/>.
	/// <seealso cref="SqlDbSharpError"/>
	/// <seealso cref="SqlDbSharpException"/>
	/// </summary>
	/// <remarks>Not serializable on Compact Framework 1.0</remarks>
	#if !POCKETPC
	[Serializable]
	#endif
	public sealed class SqlDbSharpErrorCollection : CollectionBase
	{
		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal SqlDbSharpErrorCollection() : base()
		{
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Strong typed version of the Add method for adding <see cref="SqlDbSharpError"/>
		/// objects to the collection.
		/// <seealso cref="SqlDbSharpError"/>
		/// <seealso cref="SqlDbSharpException"/>
		/// </summary>
		/// <param name="error"></param>
		public void Add( SqlDbSharpError error )
		{
			base.InnerList.Add(error);
		}

		/// <summary>
		/// Strong typed version of the Add method for adding <see cref="SqlDbSharpError"/>
		/// objects to the collection.
		/// <seealso cref="SqlDbSharpError"/>
		/// <seealso cref="SqlDbSharpException"/>
		/// </summary>
		public SqlDbSharpError this[ int index ]
		{
			get
			{
				return (SqlDbSharpError)base.InnerList[index];
			}
			set 
			{
				base.InnerList[index] = value;
			}
		}

		#endregion
	}
}

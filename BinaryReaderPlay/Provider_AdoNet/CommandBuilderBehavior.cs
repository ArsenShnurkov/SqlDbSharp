namespace System.Data.SqlDbSharp
{
    using System;

    #region CommandBuilderBehavior

	/// <summary>
	/// Enum describing the command builder behavior.
	/// <seealso cref="CommandBuilder"/>
	/// </summary>
	[FlagsAttribute()]
	enum CommandBuilderBehavior
	{
		/// <summary>
		/// Default behavior.
		/// </summary>
		Default = 0,
		/// <summary>
		/// When doing update use the original value if not changed.
		/// </summary>
		UpdateSetSameValue = 1,
		/// <summary>
		/// Use row version where doing updates.
		/// </summary>
		UseRowVersionInUpdateWhereClause = 2,
		/// <summary>
		/// Use row version where doing deletes.
		/// </summary>
		UseRowVersionInDeleteWhereClause = 4,
		/// <summary>
		/// Use row version in selects.
		/// </summary>
		UseRowVersionInWhereClause = 6,
		/// <summary>
		/// Compare matching row using only primary key and not all columns when updating.
		/// </summary>
		PrimaryKeyOnlyUpdateWhereClause = 16,
		/// <summary>
		/// Compare matching row using only primary key and not all columns when deleting.
		/// </summary>
		PrimaryKeyOnlyDeleteWhereClause = 32,
		/// <summary>
		/// Compare matching row using only primary key and not all columns.
		/// </summary>
		PrimaryKeyOnlyWhereClause = 48,
	}

	#endregion
}

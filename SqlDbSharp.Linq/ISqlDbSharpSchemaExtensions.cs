using System;

namespace System.Data.SqlDbSharp.Linq
{
	internal interface ISqlDbSharpSchemaExtensions
	{
		void BuildTempSchema(SqlDbSharpConnection cnn);
	}
}


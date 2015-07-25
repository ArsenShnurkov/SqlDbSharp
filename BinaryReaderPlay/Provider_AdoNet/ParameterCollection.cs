
namespace System.Data.SqlDbSharp
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using org.rufwork.mooresDb;

    public class DbParameterCollection<TParameter> : DbParameterCollection
		where TParameter : DbParameter
	{
		List<TParameter> parameters = new List<TParameter> ();

		public DbParameterCollection ()
		{
		}

		public override int Count {get {return parameters.Count;}}
		public override bool IsFixedSize {get {return false;}}
		public override bool IsReadOnly {get {return false;}}
		public override bool IsSynchronized {get {return false;}}
		public override object SyncRoot {get {return parameters;}}

		public override int Add (object value)
		{
			if (!(value is TParameter))
				throw new ArgumentException ("wrong type", "value");
			parameters.Add ((TParameter) value);
			return parameters.Count-1;
		}

		public override void AddRange (Array values)
		{
			foreach (TParameter p in values)
				Add (p);
		}

		public override void Clear ()
		{
			parameters.Clear ();
		}

		public override bool Contains (object value)
		{
			return parameters.Contains ((TParameter) value);
		}

		public override bool Contains (string value)
		{
			return parameters.Any (p => p.ParameterName == value);
		}

		public override void CopyTo (Array array, int index)
		{
			((ICollection) parameters).CopyTo (array, index);
		}

		public override IEnumerator GetEnumerator ()
		{
			return parameters.GetEnumerator ();
		}

		public override int IndexOf (object value)
		{
			return parameters.IndexOf ((TParameter) value);
		}

		public override int IndexOf (string value)
		{
			for (int i = 0; i < parameters.Count; ++i)
				if (parameters [i].ParameterName == value)
					return i;
			return -1;
		}

		public override void Insert (int index, object value)
		{
			parameters.Insert (index, (TParameter) value);
		}

		public override void Remove (object value)
		{
			parameters.Remove ((TParameter) value);
		}

		public override void RemoveAt (int index)
		{
			parameters.RemoveAt (index);
		}

		public override void RemoveAt (string value)
		{
			int idx = IndexOf (value);
			if (idx >= 0)
				parameters.RemoveAt (idx);
		}

		protected override DbParameter GetParameter (int index)
		{
			return parameters [index];
		}

		protected override DbParameter GetParameter (string value)
		{
			return parameters.Where (p => p.ParameterName == value)
				.FirstOrDefault ();
		}

		protected override void SetParameter (int index, DbParameter value)
		{
			parameters [index] = (TParameter) value;
		}

		protected override void SetParameter (string index, DbParameter value)
		{
			parameters [IndexOf (value)] = (TParameter) value;
		}
	}
	
	/// <summary>
	/// Parameter Collection class for Hsql ADO.NET data provider.
	/// <seealso cref="SqlDbSharpParameter"/>
	/// <seealso cref="SqlDbSharpCommand"/>
	/// </summary>
	/// <remarks>Not serializable on Compact Framework 1.0</remarks>
	public sealed class SqlDbSharpParameterCollection : DbParameterCollection<SqlDbSharpParameter>
	{
		SqlDbSharpCommand _cmd;
		public SqlDbSharpParameterCollection(SqlDbSharpCommand cmd)
		{
			_cmd = cmd;
		}
		/// <summary>
		///  Get or set parameters by index.
		/// </summary>
		public SqlDbSharpParameter this[int index]
		{
			get
			{
				return (SqlDbSharpParameter)base[index];
			}
			set
			{
				base[index] = value;
				//_names[((SqlDbSharpParameter)value).ParameterName] = index;
			}
		}
	}
}

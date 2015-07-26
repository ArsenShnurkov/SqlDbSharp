﻿using System.Diagnostics;

namespace System.Data.SqlDbSharp.Linq
{
	using System;
	using System.Data.Entity.Core;
	using System.Reflection;
	using System.IO;
	using System.Xml;
	using System.Data.Entity.Core.Common;
	using System.Data.Entity.Core.Metadata.Edm;

	/// <summary>
	/// The Provider Manifest for SQL Server
	/// </summary>
	internal class SqlDbSharpProviderManifest : DbXmlEnabledProviderManifest
	{
		internal SqlDbSharpDateFormats _dateFormat;

		/// <summary>
		/// Constructs the provider manifest.
		/// </summary>
		/// <remarks>
		/// We pass the token as a DateTimeFormat enum text, because all the datetime functions
		/// are vastly different depending on how the user is opening the connection
		/// </remarks>
		/// <param name="manifestToken">A token used to infer the capabilities of the store</param>
		public SqlDbSharpProviderManifest(string manifestToken)
			: base(SqlDbSharpProviderManifest.GetProviderManifest())
		{
			_dateFormat = (SqlDbSharpDateFormats)Enum.Parse(typeof(SqlDbSharpDateFormats), manifestToken, true);
		}

		internal static XmlReader GetProviderManifest()
		{
			return GetXmlResource("System.Data.SqlDbSharp.Linq.Resources.SqlDbSharpProviderServices.ProviderManifest.xml");
		}

		private XmlReader GetStoreSchemaMapping()
		{
			return GetXmlResource("System.Data.SqlDbSharp.Linq.Resources.SqlDbSharpProviderServices.StoreSchemaMapping.msl");
		}

		private XmlReader GetStoreSchemaDescription()
		{
			return GetXmlResource("System.Data.SqlDbSharp.Linq.Resources.SqlDbSharpProviderServices.StoreSchemaDefinition.ssdl");
		}

		internal static XmlReader GetXmlResource(string resourceName)
		{
			var assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(resourceName);
			#if DEBUG
			if (stream == null) {
				// http://stackoverflow.com/questions/3068736/cant-load-a-manifest-resource-with-getmanifestresourcestream
				string[] names = assembly.GetManifestResourceNames ();
				foreach (var n in names) {
					Trace.WriteLine (n);
				}
			}
			#endif
			return XmlReader.Create(stream);
		}

		/// <summary>
		/// Returns manifest information for the provider
		/// </summary>
		/// <param name="informationType">The name of the information to be retrieved.</param>
		/// <returns>An XmlReader at the begining of the information requested.</returns>
		protected override XmlReader GetDbInformation(string informationType)
		{
			if (informationType == DbProviderManifest.StoreSchemaDefinition)
				return GetStoreSchemaDescription();
			else if (informationType == DbProviderManifest.StoreSchemaMapping)
				return GetStoreSchemaMapping();
			else if (informationType == DbProviderManifest.ConceptualSchemaDefinition)
				return null;

			throw new ProviderIncompatibleException(String.Format("SqlDbSharp does not support this information type '{0}'.", informationType));
		}

		/// <summary>
		/// This method takes a type and a set of facets and returns the best mapped equivalent type 
		/// in EDM.
		/// </summary>
		/// <param name="storeType">A TypeUsage encapsulating a store type and a set of facets</param>
		/// <returns>A TypeUsage encapsulating an EDM type and a set of facets</returns>
		public override TypeUsage GetEdmType(TypeUsage storeType)
		{
			if (storeType == null)
			{
				throw new ArgumentNullException("storeType");
			}

			string storeTypeName = storeType.EdmType.Name.ToLowerInvariant();
			//if (!base.StoreTypeNameToEdmPrimitiveType.ContainsKey(storeTypeName))
			//{
			//  switch (storeTypeName)
			//  {
			//    case "integer":
			//      return TypeUsage.CreateDefaultTypeUsage(PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int64));
			//    default:
			//      throw new ArgumentException(String.Format("SqlDbSharp does not support the type '{0}'.", storeTypeName));
			//  }
			//}

			PrimitiveType edmPrimitiveType;

			if (base.StoreTypeNameToEdmPrimitiveType.TryGetValue(storeTypeName, out edmPrimitiveType) == false)
				throw new ArgumentException(String.Format("SqlDbSharp does not support the type '{0}'.", storeTypeName));

			int maxLength = 0;
			bool isUnicode = true;
			bool isFixedLen = false;
			bool isUnbounded = true;

			PrimitiveTypeKind newPrimitiveTypeKind;

			switch (storeTypeName)
			{
			case "tinyint":
			case "smallint":
			case "integer":
			case "bit":
			case "uniqueidentifier":
			case "int":
			case "float":
			case "real":
				return TypeUsage.CreateDefaultTypeUsage(edmPrimitiveType);

			case "varchar":
				newPrimitiveTypeKind = PrimitiveTypeKind.String;
				isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
				isUnicode = false;
				isFixedLen = false;
				break;
			case "char":
				newPrimitiveTypeKind = PrimitiveTypeKind.String;
				isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
				isUnicode = false;
				isFixedLen = true;
				break;
			case "nvarchar":
				newPrimitiveTypeKind = PrimitiveTypeKind.String;
				isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
				isUnicode = true;
				isFixedLen = false;
				break;
			case "nchar":
				newPrimitiveTypeKind = PrimitiveTypeKind.String;
				isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
				isUnicode = true;
				isFixedLen = true;
				break;
			case "blob":
				newPrimitiveTypeKind = PrimitiveTypeKind.Binary;
				isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
				isFixedLen = false;
				break;
			case "decimal":
				{
					byte precision;
					byte scale;
					if (TypeHelpers.TryGetPrecision(storeType, out precision) && TypeHelpers.TryGetScale(storeType, out scale))
					{
						return TypeUsage.CreateDecimalTypeUsage(edmPrimitiveType, precision, scale);
					}
					else
					{
						return TypeUsage.CreateDecimalTypeUsage(edmPrimitiveType);
					}
				}
			case "datetime":
				return TypeUsage.CreateDateTimeTypeUsage(edmPrimitiveType, null);
			default:
				throw new NotSupportedException(String.Format("SqlDbSharp does not support the type '{0}'.", storeTypeName));
			}

			switch (newPrimitiveTypeKind)
			{
			case PrimitiveTypeKind.String:
				if (!isUnbounded)
				{
					return TypeUsage.CreateStringTypeUsage(edmPrimitiveType, isUnicode, isFixedLen, maxLength);
				}
				else
				{
					return TypeUsage.CreateStringTypeUsage(edmPrimitiveType, isUnicode, isFixedLen);
				}
			case PrimitiveTypeKind.Binary:
				if (!isUnbounded)
				{
					return TypeUsage.CreateBinaryTypeUsage(edmPrimitiveType, isFixedLen, maxLength);
				}
				else
				{
					return TypeUsage.CreateBinaryTypeUsage(edmPrimitiveType, isFixedLen);
				}
			default:
				throw new NotSupportedException(String.Format("SqlDbSharp does not support the type '{0}'.", storeTypeName));
			}
		}

		/// <summary>
		/// This method takes a type and a set of facets and returns the best mapped equivalent type 
		/// </summary>
		/// <param name="edmType">A TypeUsage encapsulating an EDM type and a set of facets</param>
		/// <returns>A TypeUsage encapsulating a store type and a set of facets</returns>
		public override TypeUsage GetStoreType(TypeUsage edmType)
		{
			if (edmType == null)
				throw new ArgumentNullException("edmType");

			PrimitiveType primitiveType = edmType.EdmType as PrimitiveType;
			if (primitiveType == null)
				throw new ArgumentException(String.Format("SqlDbSharp does not support the type '{0}'.", edmType));

			ReadOnlyMetadataCollection<Facet> facets = edmType.Facets;

			switch (primitiveType.PrimitiveTypeKind)
			{
			case PrimitiveTypeKind.Boolean:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["bit"]);
			case PrimitiveTypeKind.Byte:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["tinyint"]);
			case PrimitiveTypeKind.Int16:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["smallint"]);
			case PrimitiveTypeKind.Int32:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["int"]);
			case PrimitiveTypeKind.Int64:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["integer"]);
			case PrimitiveTypeKind.Guid:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["uniqueidentifier"]);
			case PrimitiveTypeKind.Double:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["float"]);
			case PrimitiveTypeKind.Single:
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["real"]);
			case PrimitiveTypeKind.Decimal: // decimal, numeric, smallmoney, money
				{
					byte precision;
					if (!TypeHelpers.TryGetPrecision(edmType, out precision))
					{
						precision = 18;
					}

					byte scale;
					if (!TypeHelpers.TryGetScale(edmType, out scale))
					{
						scale = 0;
					}

					return TypeUsage.CreateDecimalTypeUsage(StoreTypeNameToStorePrimitiveType["decimal"], precision, scale);
				}
			case PrimitiveTypeKind.Binary: // binary, varbinary, varbinary(max), image, timestamp, rowversion
				{
					bool isFixedLength = null != facets["FixedLength"].Value && (bool)facets["FixedLength"].Value;
					Facet f = facets["MaxLength"];

					bool isMaxLength = f.IsUnbounded || null == f.Value || (int)f.Value > Int32.MaxValue;
					int maxLength = !isMaxLength ? (int)f.Value : Int32.MinValue;

					TypeUsage tu;
					if (isFixedLength)
						tu = TypeUsage.CreateBinaryTypeUsage(StoreTypeNameToStorePrimitiveType["blob"], true, maxLength);
					else
					{
						if (isMaxLength)
							tu = TypeUsage.CreateBinaryTypeUsage(StoreTypeNameToStorePrimitiveType["blob"], false);
						else
							tu = TypeUsage.CreateBinaryTypeUsage(StoreTypeNameToStorePrimitiveType["blob"], false, maxLength);
					}
					return tu;
				}
			case PrimitiveTypeKind.String: // char, nchar, varchar, nvarchar, varchar(max), nvarchar(max), ntext, text
				{
					bool isUnicode = null == facets["Unicode"].Value || (bool)facets["Unicode"].Value;
					bool isFixedLength = null != facets["FixedLength"].Value && (bool)facets["FixedLength"].Value;
					Facet f = facets["MaxLength"];
					// maxlen is true if facet value is unbounded, the value is bigger than the limited string sizes *or* the facet
					// value is null. this is needed since functions still have maxlength facet value as null
					bool isMaxLength = f.IsUnbounded || null == f.Value || (int)f.Value > (isUnicode ? Int32.MaxValue : Int32.MaxValue);
					int maxLength = !isMaxLength ? (int)f.Value : Int32.MinValue;

					TypeUsage tu;

					if (isUnicode)
					{
						if (isFixedLength)
							tu = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["nchar"], true, true, maxLength);
						else
						{
							if (isMaxLength)
								tu = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["nvarchar"], true, false);
							else
								tu = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["nvarchar"], true, false, maxLength);
						}
					}
					else
					{
						if (isFixedLength)
							tu = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["char"], false, true, maxLength);
						else
						{
							if (isMaxLength)
								tu = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["varchar"], false, false);
							else
								tu = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["varchar"], false, false, maxLength);
						}
					}
					return tu;
				}
			case PrimitiveTypeKind.DateTime: // datetime, smalldatetime
				return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["datetime"]);
			default:
				throw new NotSupportedException(String.Format("There is no store type corresponding to the EDM type '{0}' of primitive type '{1}'.", edmType, primitiveType.PrimitiveTypeKind));
			}
		}

		private static class TypeHelpers
		{
			public static bool TryGetPrecision(TypeUsage tu, out byte precision)
			{
				Facet f;

				precision = 0;
				if (tu.Facets.TryGetValue("Precision", false, out f))
				{
					if (!f.IsUnbounded && f.Value != null)
					{
						precision = (byte)f.Value;
						return true;
					}
				}
				return false;
			}

			public static bool TryGetMaxLength(TypeUsage tu, out int maxLength)
			{
				Facet f;

				maxLength = 0;
				if (tu.Facets.TryGetValue("MaxLength", false, out f))
				{
					if (!f.IsUnbounded && f.Value != null)
					{
						maxLength = (int)f.Value;
						return true;
					}
				}
				return false;
			}

			public static bool TryGetScale(TypeUsage tu, out byte scale)
			{
				Facet f;

				scale = 0;
				if (tu.Facets.TryGetValue("Scale", false, out f))
				{
					if (!f.IsUnbounded && f.Value != null)
					{
						scale = (byte)f.Value;
						return true;
					}
				}
				return false;
			}
		}
	}
}
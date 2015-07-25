namespace System.Data.SqlDbSharp
{
    using System;
    using System.ComponentModel;
    using System.Collections;
    using System.Diagnostics;
    using System.Data.Common;

	/// <summary>
	/// CommandBuilder component for design time.
	/// <seealso cref="SqlDbSharpConnection"/>
	/// <seealso cref="SqlDbSharpReader"/>
	/// <seealso cref="SqlDbSharpParameter"/>
	/// <seealso cref="SqlDbSharpTransaction"/>
	/// <seealso cref="SqlDbSharpDataAdapter"/>
	/// </summary>
	public class SqlDbSharpCommandBuilder : DbCommandBuilder
	{
		#region Constructors

		/// <summary>
		/// Component constructor.
		/// </summary>
		/// <param name="container"></param>
		public SqlDbSharpCommandBuilder(System.ComponentModel.IContainer container)
		{
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			container.Add((IComponent)this);
			InitializeComponent();
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SqlDbSharpCommandBuilder()
		{
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			InitializeComponent();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Constructor using an <see cref="SqlDbSharpDataAdapter"/>.
		/// </summary>
		/// <param name="adapter"></param>
		public SqlDbSharpCommandBuilder(SqlDbSharpDataAdapter adapter)
		{
			GC.SuppressFinalize(this);
			this.DataAdapter = adapter;
		}

		#endregion

		#region Dispose Methods

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Public Methods & Properties

		/// <summary>
		/// Derive command parameters.
		/// </summary>
		/// <param name="command"></param>
		public static void DeriveParameters(SqlDbSharpCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			command.DeriveParameters();
		}

		/// <summary>
		/// Gets the delete command.
		/// </summary>
		/// <returns></returns>
		public SqlDbSharpCommand GetDeleteCommand()
		{
			return (SqlDbSharpCommand) this.GetBuilder().GetDeleteCommand();
		}
 
		/// <summary>
		/// Gets the insert command.
		/// </summary>
		/// <returns></returns>
		public SqlDbSharpCommand GetInsertCommand()
		{
			return (SqlDbSharpCommand) this.GetBuilder().GetInsertCommand();
		}

		/// <summary>
		/// Gets the update command.
		/// </summary>
		/// <returns></returns>
		public SqlDbSharpCommand GetUpdateCommand()
		{
			return (SqlDbSharpCommand) this.GetBuilder().GetUpdateCommand();
		}

		/// <summary>
		/// Refresh the database schema.
		/// </summary>
		public void RefreshSchema()
		{
			this.GetBuilder().RefreshSchema();
		}

		/// <summary>
		/// Get or set the <see cref="SqlDbSharpDataAdapter"/> object used.
		/// </summary>
		public SqlDbSharpDataAdapter DataAdapter
		{
			get
			{
				return (SqlDbSharpDataAdapter) this.GetBuilder().DataAdapter;
			}
			set
			{
				this.GetBuilder().DataAdapter = value;
			}
		}
 
		/// <summary>
		/// Get or set the quote prefix.
		/// </summary>
		public string QuotePrefix
		{
			get
			{
				return this.GetBuilder().QuotePrefix;
			}
			set
			{
				this.GetBuilder().QuotePrefix = value;
			}
		}

		/// <summary>
		/// Get or set the quote suffix.
		/// </summary>
		public string QuoteSuffix
		{
			get
			{
				return this.GetBuilder().QuoteSuffix;
			}
			set
			{
				this.GetBuilder().QuoteSuffix = value;
			}
		}

		#endregion

		#region Private Methods

		private CommandBuilder GetBuilder()
		{
			if (this.cmdBuilder == null)
			{
				this.cmdBuilder = new CommandBuilder();
			}
			return this.cmdBuilder;
		}

		#endregion

		#region Private Vars

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CommandBuilder cmdBuilder;

		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
		protected override void ApplyParameterInfo (DbParameter parameter, DataRow row, StatementType statementType, bool whereClause)
		{
			throw new NotImplementedException ();
		}
		
		protected override string GetParameterName (string parameterName)
		{
			throw new NotImplementedException ();
		}

		protected override string GetParameterName (int parameterOrdinal)
		{
			throw new NotImplementedException ();
		}
		protected override string GetParameterPlaceholder (int parameterOrdinal)
		{
			throw new NotImplementedException ();
		}
		protected override void SetRowUpdatingHandler (DbDataAdapter adapter)
		{
			throw new NotImplementedException ();
		}
	}
}

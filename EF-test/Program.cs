using System;
using System.Data.Entity;
using System.Reflection;

public class Person
{
	public int Id {
		get;
		set;
	}

	public string FirstName {
		get;
		set;
	}

	public string LastName {
		get;
		set;
	}
}
public class SimpleContext : DbContext
{
	public SimpleContext () : base("name = ConnStr")
	{
	}
	public DbSet<Person> People { 
		get; 
		set;
	}
}

class MainClass
{
	public static void Main (string[] args)
	{
		/*
		Assembly a = Assembly.LoadFrom ("SqlDbSharp.dll");
		Console.WriteLine (a.CodeBase);
		Assembly al = Assembly.LoadFrom ("SqlDbSharp.Linq.dll");
		Console.WriteLine (al.CodeBase);
		*/
		using (var context = new SimpleContext ()) {
			var person = new Person { 
				FirstName = "Joe", 
				LastName = "Bloggs" 
			};

			context.People.Add (person);
			context.SaveChanges ();
		}
	}
}

/*
A null was returned after calling the 'get_ProviderFactory' method on a store provider instance of type 'System.Data.SqlDbSharp.SqlDbSharpConnection'.
The store provider might not be functioning correctly.
*/
/*
Failed to find or load the registered .Net Framework Data Provider.
DbProviderFactory System.Data.Common.DbProviderFactories:GetFactory (DataRow)
*/
/*
No Entity Framework provider found for the ADO.NET provider with invariant name 'System.Data.SqlDbSharp'.
Make sure the provider is registered in the 'entityFramework' section of the application config file.
See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
*/
/*
The Entity Framework provider type 'MyProvider.MyProviderServices, MyAssembly' registered in the application config file 
for the ADO.NET provider with invariant name 'My.Invariant.Name' could not be loaded.
Make sure that the assembly-qualified name is used and that the assembly is available to the running application.
*/
/*
Entity Framework providers must declare a static property or field named 'Instance' that returns the singleton instance of the provider.
*/
/*
Not all resources was transferred
		System.Data.SqlDbSharp.Linq.SqlDbSharpProviderManifest.GetXmlResource (resourceName="System.Data.SqlDbSharp.Linq.Resources.SqlDbSharpProviderServices.ProviderManifest.xml") 
		in /var/calculate/remote/distfiles/egit-src/SqlDbSharp.git/src/SqlDbSharp.Linq/SqlDbSharpProviderManifest.cs:305
*/
/*
DatabaseExists is not supported by the provider.
		http://stackoverflow.com/questions/6101812/use-entity-framework-4-1-with-sqlite
		you have a problem because your provider doesn't support all necessary functionality used by EF code first. 
		http://w3facility.org/question/databaseexists-is-not-supported-by-the-provider-efwrappertoolkit/
		more detailed description - 
		DbProviderServicesBase in the DbDatabaseExists method

		NpgSql does like this:
		https://github.com/npgsql/npgsql/pull/91/files#diff-8eba631855901aefb64b3ab6cafe4489R113
*/
/*
Unexpected connection state. When using a wrapping provider ensure that the StateChange event is implemented on the wrapped DbConnection.
		http://stackoverflow.com/questions/28653352/entity-framework-unexpected-connection-state-exception
*/
/*
		37000 Unexpected token: [ in statement [SELECT 
			[GroupBy1].[A1] AS [C1]
			FROM ( SELECT 
				Count([Filter1].[A1]) AS [A1]
				FROM ( SELECT 
					1 AS [A1]
					FROM [__MigrationHistory] AS [Extent1]
					WHERE [Extent1].[ContextKey] = @'SimpleContext'
				)  AS [Filter1]
			)  AS [GroupBy1]]

Table aliases are supported in HypersonicSQL DB starting from version 2.3
C# port was forked from version 1.4 (so, it doesn't support table aliases)
*/
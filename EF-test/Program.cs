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

namespace System.Data.SqlDbSharp
{
    using System;

    using org.rufwork.mooresDb;
    using org.rufwork.mooresDb.infrastructure;

    public class DatabaseController{
        public static Database GetDatabase(string name){
            return new Database (name);
        }
    }
    public class Database: org.rufwork.mooresDb.infrastructure.contexts.DatabaseContext{
        public Database(string name):base(name){
        }
        public Channel Connect(string name, string pass){
            var c = new Channel ();
            c.Database = this;
            return c;
        }
    }
    public class Channel {
        public Database Database { get; set; }
        public void Disconnect(){
        }
        public Result Execute(string q)
        {
            var r = new Result ();

            CommandParser parser = new CommandParser(Database);

            object o = parser.executeCommand(q);

            if (o is DataTable) {
                r.Data = o as DataTable;
            }
            if (o is string) {
                r.Error = o as string;
            }
            return r;
        }
    }
    public class Result {
        public int UpdateCount{ get; set;}
        public string Error {get; set; }
        public DataTable Data { get; set; } 
        public int ColumnCount { get { return Data.Columns.Count; } }
       
    }
    public class Record {}
}


namespace System.Data.SqlDbSharp
{
    using System;

    using org.rufwork.mooresDb;

    public class DatabaseController{
        public static Database GetDatabase(string name){
            return new Database (name);
        }
    }
    public class Result :  org.rufwork.mooresDb.infrastructure.contexts.TableContext{
        public int UpdateCount{ get; set;}
        public string Error {get; set; }
        public int ColumnCount { get { return base.getColumns ().Length; } }
    }
    public class Channel {
        public Database Database { get; set; }
        public void Disconnect(){
        }
        public Result Execute(string q)
        {
            return new Result ();
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
    public class Record {}
    public enum ColumnType {}


}


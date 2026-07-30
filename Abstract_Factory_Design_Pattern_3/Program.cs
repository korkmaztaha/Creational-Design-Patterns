
//Database msSQL = new();
//
//msSQL.Connection = new();
//msSQL.Connection.ConnectionString = "...";
//msSQL.Command = new();
//
//var result = msSQL.Connection.Connect();
//
//if (result && msSQL.Connection.State == ConnectionState.Open)
//{
//    msSQL.Command.Execute("Select * from ....");
//}
//
//msSQL.Connection.Disconnect();



//Database oracle = new();
//oracle.Connection = new();
//oracle.Connection.ConnectionString = "...";
//oracle.Command = new();




// Factory bizim yerimize oluşturuyor.
//////////////////////////////////////////////////////////////

DatabaseCreator creator = new();


// Oracle veritabanı oluştur.
Database database = creator.Create(new OracleDatabaseFactory());


// MySQL veritabanı oluştur.
Database database2 = creator.Create(new MYSqlDatabaseFactory());

Console.WriteLine();



//////////////////////////////////////////////////////////////
// DatabaseType Enum
//
// Sistemde desteklenen veritabanı türlerini tutar.
//////////////////////////////////////////////////////////////

enum DatabaseType
{
    Oracle,
    MSSql,
    MYSql,
    PostgreSql
}



//////////////////////////////////////////////////////////////
// Database Sınıfı
//
// Bir veritabanının sahip olduğu nesneleri temsil eder.
//////////////////////////////////////////////////////////////

class Database
{
    public Database() { }


    // Constructor
    //
    // Veritabanı türünü,
    // Connection nesnesini
    // ve Command nesnesini alır.
    public Database(DatabaseType type, Connection connection, Command command)
    {
        Type = type;
        Connection = connection;
        Command = command;
    }


    // Veritabanı türü
    public DatabaseType Type { get; set; }


    // Bağlantı nesnesi
    //
    // AbstractConnection kullanıldığı için
    // Oracle, MSSQL, MySQL vb. tüm bağlantıları tutabilir.
    public AbstractConnection Connection { get; set; }


    // Komut nesnesi
    public AbstractCommand Command { get; set; }
}



//////////////////////////////////////////////////////////////
// Connection Durumu
//////////////////////////////////////////////////////////////

enum ConnectionState
{
    Open,
    Close
}



//////////////////////////////////////////////////////////////
// Abstract Product
//
// Tüm bağlantı sınıflarının sahip olması gereken özellikleri
// belirler.
//////////////////////////////////////////////////////////////

abstract class AbstractConnection
{

    // Connection String
    public abstract string ConnectionString { get; set; }


    // Bağlantı durumu
    public abstract ConnectionState State { get; set; }


    // Bağlan
    public abstract bool Connect();


    // Bağlantıyı kapat
    public abstract bool Disconnect();

}



//////////////////////////////////////////////////////////////
// Abstract Product
//
// Bütün Command sınıflarının uygulaması gereken metot.
//////////////////////////////////////////////////////////////

abstract class AbstractCommand
{

    // SQL sorgusunu çalıştır.
    public abstract void Execute(string query);

}



//////////////////////////////////////////////////////////////
// Concrete Product
//
// Gerçek Connection sınıfı
//////////////////////////////////////////////////////////////

class Connection : AbstractConnection
{

    string _connectionString;

    public Connection()
    {

    }

    public Connection(string connectionString)
        => _connectionString = connectionString;


    // Connection String
    public override string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value;
    }


    // Açık/Kapalı bilgisi
    public override ConnectionState State { get; set; }


    // Veritabanına bağlan.
    public override bool Connect()
    {
        // Gerçek projede burada bağlantı kurulacaktır.

        State = ConnectionState.Open;

        return true;
    }


    // Bağlantıyı kapat.
    public override bool Disconnect()
    {
        // Gerçek projede bağlantı kapatılır.

        State = ConnectionState.Close;

        return true;
    }

}



//////////////////////////////////////////////////////////////
// Concrete Product
//
// SQL komutlarını çalıştırır.
//////////////////////////////////////////////////////////////

class Command : AbstractCommand
{

    public override void Execute(string query)
    {
        // SQL sorgusu burada çalıştırılır.
    }

}



//////////////////////////////////////////////////////////////
// Abstract Factory
//
// Tüm veritabanı fabrikalarının uygulaması gereken metotlar.
//////////////////////////////////////////////////////////////

abstract class DatabaseFactory
{

    // Connection üret.
    public abstract AbstractConnection CreateConnection();


    // Command üret.
    public abstract AbstractCommand CreateCommand();

}



//////////////////////////////////////////////////////////////
// MSSQL Factory
//////////////////////////////////////////////////////////////

class MSSqlDatabaseFactory : DatabaseFactory
{

    // MSSQL Command üret.
    public override AbstractCommand CreateCommand()
    {
        return new Command();
    }


    // MSSQL Connection üret.
    public override AbstractConnection CreateConnection()
    {
        Connection connection = new();

        connection.ConnectionString = "MSSQL connection string";

        return connection;
    }

}



//////////////////////////////////////////////////////////////
// Oracle Factory
//////////////////////////////////////////////////////////////

class OracleDatabaseFactory : DatabaseFactory
{

    public override AbstractCommand CreateCommand()
    {
        return new Command();
    }


    public override AbstractConnection CreateConnection()
    {
        Connection connection = new();

        connection.ConnectionString = "Oracle connection string";

        return connection;
    }

}



//////////////////////////////////////////////////////////////
// MySQL Factory
//////////////////////////////////////////////////////////////

class MYSqlDatabaseFactory : DatabaseFactory
{

    public override AbstractCommand CreateCommand()
    {
        return new Command();
    }


    public override AbstractConnection CreateConnection()
    {
        Connection connection = new();

        connection.ConnectionString = "MYSql connection string";

        return connection;
    }

}



//////////////////////////////////////////////////////////////
// PostgreSQL Factory
//////////////////////////////////////////////////////////////

class PostgreSQLDatabaseFactory : DatabaseFactory
{

    public override AbstractCommand CreateCommand()
    {
        return new Command();
    }


    public override AbstractConnection CreateConnection()
    {
        Connection connection = new();

        connection.ConnectionString = "PostgreSQL connection string";

        return connection;
    }

}



//////////////////////////////////////////////////////////////
// Creator
//
// Factory'den Connection ve Command alır,
// Database nesnesini oluşturur.
//////////////////////////////////////////////////////////////

class DatabaseCreator
{

    // Üretilecek nesneler
    AbstractConnection _connection;
    AbstractCommand _command;



    public Database Create(DatabaseFactory databaseFactory)
    {

        //////////////////////////////////////////////////////
        // Factory'den Connection oluştur.
        //////////////////////////////////////////////////////

        _connection = databaseFactory.CreateConnection();


        //////////////////////////////////////////////////////
        // Factory'den Command oluştur.
        //////////////////////////////////////////////////////

        _command = databaseFactory.CreateCommand();


        //////////////////////////////////////////////////////
        // Database nesnesini oluştur.
        //////////////////////////////////////////////////////

        return new()
        {

            // Üretilen Command nesnesi
            Command = _command,


            // Üretilen Connection nesnesi
            Connection = _connection,


            //////////////////////////////////////////////////
            // Factory ismine bakarak DatabaseType belirleniyor.
            //
            // Örneğin:
            //
            // OracleDatabaseFactory
            //
            // Replace sonucu:
            //
            // Oracle
            //
            // Enum.Parse ile:
            //
            // DatabaseType.Oracle
            //////////////////////////////////////////////////

            Type = (DatabaseType)Enum.Parse(
                        typeof(DatabaseType),
                        databaseFactory
                            .GetType()
                            .Name
                            .Replace("DatabaseFactory", "")
                   )
        };

    }

}
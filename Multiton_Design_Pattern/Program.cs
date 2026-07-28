// MSSQL için ilk nesne oluşturulur.
var msSql = Database.GetInstance("MSSQL", "....");

// Oracle için farklı bir nesne oluşturulur.
var oracle = Database.GetInstance("Oracle", "....");

// MongoDB için farklı bir nesne oluşturulur.
var mongoDB = Database.GetInstance("MongoDB", "....");

// Daha önce oluşturulan MSSQL nesnesi döndürülür.
var msSql2 = Database.GetInstance("MSSQL");

// Daha önce oluşturulan Oracle nesnesi döndürülür.
var oracle2 = Database.GetInstance("Oracle");

// Daha önce oluşturulan MongoDB nesnesi döndürülür.
var mongoDB2 = Database.GetInstance("MongoDB");

class Database
{
    // Dışarıdan new Database() yapılmasını engeller.
    private Database()
    {
        Console.WriteLine($"{nameof(Database)} nesnesi üretildi.");
    }

    // Singleton'da:
    // static Database _database;  --> Tek nesne tutulur.

    // Multiton'da:
    // Her key (MSSQL, Oracle, MongoDB) için ayrı bir nesne tutulur.
    static Dictionary<string, Database> _databases = new();

    // Geriye key dönmek zorunda olduğumuz için singelton aksine method olmak zorunda
    public static Database GetInstance(string key)
    {
        // Eğer bu key'e ait nesne yoksa oluştur.
        if (!_databases.ContainsKey(key))
            _databases[key] = new Database();

        // Aynı key için her zaman aynı nesne döner.
        return _databases[key];
    }

  
    string connectionString = "";

    // İlk oluşturulurken connection string de atanabilir.
    public static Database GetInstance(string key, string connectionString)
    {
        Database database = GetInstance(key);

        
        database.ConnectionString(connectionString);

        return database;
    }

   
    public void Connection()
    {
        Console.WriteLine("Connected");
    }

    public void Disconnect()
    {
        Console.WriteLine("Disconnected");
    }

  
    public void ConnectionString(string connectionString)
    {
        this.connectionString = connectionString;
    }
}
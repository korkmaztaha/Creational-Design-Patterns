// Eskiden her banka new ile oluşturuluyordu.
// Factory Method + Singleton sayesinde artık hem Factory'ler
// hem de Banka nesneleri uygulama boyunca tek kez oluşturulur.

BankCreator bankCreator = new();

// Factory üzerinden banka nesneleri alınır.
// Her Create() çağrısında yeni nesne oluşmaz.
GarantiBank? garanti = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank = bankCreator.Create(BankType.Halkbank) as HalkBank;
VakifBank? vakifbank = bankCreator.Create(BankType.Vakifbank) as VakifBank;

// Aynı Singleton nesneleri tekrar döner.
GarantiBank? garanti2 = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank2 = bankCreator.Create(BankType.Halkbank) as HalkBank;
VakifBank? vakifbank2 = bankCreator.Create(BankType.Vakifbank) as VakifBank;

GarantiBank? garanti3 = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank3 = bankCreator.Create(BankType.Halkbank) as HalkBank;
VakifBank? vakifbank3 = bankCreator.Create(BankType.Vakifbank) as VakifBank;



#region Abstract Product

// Tüm bankaların ortak arayüzü.
interface IBank
{

}

#endregion



#region Concrete Products

class GarantiBank : IBank
{
    string _userCode, _password;

    // Constructor private.
    // Dışarıdan new GarantiBank() yapılamaz.
    GarantiBank(string userCode, string password)
    {
        Console.WriteLine($"{nameof(GarantiBank)} nesnesi oluşturuldu.");

        _userCode = userCode;
        _password = password;
    }

    // Static Constructor yalnızca 1 kez çalışır.
    // Singleton nesnesini oluşturur.
    static GarantiBank()
        => _garantiBank = new("asd", "123");

    // Tek GarantiBank nesnesi tutulur.
    static GarantiBank _garantiBank;

    // Aynı nesneyi döndürür.
    static public GarantiBank GetInstance => _garantiBank;

    public void ConnectGaranti()
        => Console.WriteLine($"{nameof(GarantiBank)} Connected.");

    public void SendMoney(int amount)
        => Console.WriteLine($"{amount} money sent.");
}



class HalkBank : IBank
{
    string _userCode, _password;

    // Constructor private.
    HalkBank(string userCode)
    {
        Console.WriteLine($"{nameof(HalkBank)} nesnesi oluşturuldu.");

        _userCode = userCode;
    }

    // Singleton nesnesi oluşturulur.
    static HalkBank()
        => _halkBank = new("asd");

    // Tek HalkBank nesnesi.
    static HalkBank _halkBank;

    // Aynı nesneyi döndürür.
    static public HalkBank GetInstance => _halkBank;

    // Şifre atanır.
    public string Password
    {
        set => _password = value;
    }

    public void Send(int amount, string accountNumber)
        => Console.WriteLine($"{amount} money sent.");
}



class CredentialVakifBank
{
    // VakıfBank giriş bilgileri.
    public string UserCode { get; set; }
    public string Mail { get; set; }
}



class VakifBank : IBank
{
    string _userCode, _email, _password;

    // Kullanıcı doğrulandı mı?
    public bool isAuthentcation { get; set; }

    // Constructor private.
    VakifBank(CredentialVakifBank credential, string password)
    {
        Console.WriteLine($"{nameof(VakifBank)} nesnesi oluşturuldu.");

        _userCode = credential.UserCode;
        _email = credential.Mail;
        _password = password;
    }

    // Singleton nesnesi oluşturulur.
    static VakifBank()
        => _vakifBank = new(
            new()
            {
                Mail = "aaa@bbbb.com",
                UserCode = "aaa"
            },
            "123");

    // Tek VakifBank nesnesi.
    static VakifBank _vakifBank;

    // Aynı nesneyi döndürür.
    static public VakifBank GetInstance => _vakifBank;

    // Kimlik doğrulama yapılır.
    public void ValidateCredential()
    {
        if (true)
            isAuthentcation = true;
    }

    public void SendMoneyToAccountNumber(int amount, string recipientName, string accountNumber)
        => Console.WriteLine($"{amount} money sent.");
}

#endregion



#region Abstract Factory

// Tüm Factory sınıflarının ortak arayüzü.
interface IBankFactory
{
    // Banka nesnesi oluşturur.
    IBank CreateInstance();
}

#endregion



#region Concrete Factories

class GarantiFactory : IBankFactory
{
    // Factory de Singleton'dır.
    GarantiFactory() { }

    // Factory yalnızca 1 kez oluşturulur.
    static GarantiFactory()
        => _garantiFactory = new();

    // Tek Factory nesnesi tutulur.
    static GarantiFactory _garantiFactory;

    // Aynı Factory nesnesi döndürülür.
    static public GarantiFactory GetInstance => _garantiFactory;

    public IBank CreateInstance()
    {
        // Yeni GarantiBank oluşturmaz.
        // Mevcut Singleton nesneyi alır.
        GarantiBank garanti = GarantiBank.GetInstance;

        // Gerekli hazırlık işlemleri yapılır.
        garanti.ConnectGaranti();

        return garanti;
    }
}



class HalkBankFactory : IBankFactory
{
    HalkBankFactory() { }

    // Singleton Factory oluşturulur.
    static HalkBankFactory()
        => _halkBankFactory = new();

    static HalkBankFactory _halkBankFactory;

    static public HalkBankFactory GetInstance => _halkBankFactory;

    public IBank CreateInstance()
    {
        // Singleton HalkBank alınır.
        HalkBank halkBank = HalkBank.GetInstance;

        // Gerekli ayarlar yapılır.
        halkBank.Password = "123";

        return halkBank;
    }
}



class VakifBankFactory : IBankFactory
{
    VakifBankFactory() { }

    // Singleton Factory oluşturulur.
    static VakifBankFactory()
        => _vakifBankFactory = new();

    static VakifBankFactory _vakifBankFactory;

    static public VakifBankFactory GetInstance => _vakifBankFactory;

    public IBank CreateInstance()
    {
        // Singleton VakifBank alınır.
        VakifBank vakifBank = VakifBank.GetInstance;

        // Kimlik doğrulaması yapılır.
        vakifBank.ValidateCredential();

        return vakifBank;
    }
}

#endregion



#region Creator

// Oluşturulacak banka tipleri.
enum BankType
{
    Garanti,
    Halkbank,
    Vakifbank
}

class BankCreator
{
    public IBank Create(BankType bankType)
    {
        // Banka tipine göre ilgili Singleton Factory seçilir.
        IBankFactory _bankFactory = bankType switch
        {
            BankType.Garanti => GarantiFactory.GetInstance,
            BankType.Halkbank => HalkBankFactory.GetInstance,
            BankType.Vakifbank => VakifBankFactory.GetInstance
        };

        // Factory aynı banka nesnesini döndürür.
        return _bankFactory.CreateInstance();
    }
}

#endregion

/*
=====================================================
ÖNCEKİ FACTORY METHOD ÖRNEĞİNDEN FARKI
=====================================================

Önceki örnekte:

Create()
    ↓
GarantiFactory
    ↓
new GarantiBank()

Create() her çağrıldığında yeni nesne oluşturuluyordu.

-----------------------------------------------------

Bu örnekte:

Create()
    ↓
GarantiFactory.GetInstance
    ↓
GarantiBank.GetInstance

Artık new GarantiBank() çalışmaz.

Daha önce oluşturulan Singleton nesne döndürülür.

-----------------------------------------------------

Aynı durum HalkBank ve VakifBank için de geçerlidir.

Create()
    ↓
HalkBankFactory.GetInstance
    ↓
HalkBank.GetInstance

Create()
    ↓
VakifBankFactory.GetInstance
    ↓
VakifBank.GetInstance

-----------------------------------------------------

Bu örnekte iki tasarım deseni birlikte kullanılmaktadır.

✔ Factory Method
    - Hangi bankanın oluşturulacağına karar verir.

✔ Singleton
    - Factory'ler tek nesnedir.
    - Bankalar tek nesnedir.

Sonuç:

Create() kaç defa çağrılırsa çağrılsınF

GarantiBank
HalkBank
VakifBank

yalnızca 1 kez oluşturulur ve uygulama boyunca aynı nesneler kullanılır.
*/

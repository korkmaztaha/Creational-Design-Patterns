// Normalde her banka farklı şekilde oluşturuluyordu.

// GarantiBank garantiBank = new("asd", "123");
// garantiBank.ConnectGaranti();

// VakifBank vakifBank = new(new() { UserCode = "aaa", Mail = "aaa@bbbb.com" }, "123");
// vakifBank.ValidateCredential();

// HalkBank halkBank = new("aaa");
// halkBank.Password = "123";

// Factory Pattern sayesinde artık oluşturma işlemleri tek merkezden yapılır.
BankCreator bankCreator = new();

// Factory üzerinden GarantiBank nesnesi oluşturulur.
GarantiBank? garanti = bankCreator.Create(BankType.Garanti) as GarantiBank;

// Factory üzerinden HalkBank nesnesi oluşturulur.
HalkBank? halkBank = bankCreator.Create(BankType.Halkbank) as HalkBank;

// Factory üzerinden VakifBank nesnesi oluşturulur.
VakifBank? vakifbank = bankCreator.Create(BankType.Vakifbank) as VakifBank;

// Her Create() çağrısında yeni nesne oluşturulur.
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

    // GarantiBank kullanıcı kodu ve şifre ile oluşturulur.
    public GarantiBank(string userCode, string password)
    {
        Console.WriteLine($"{nameof(GarantiBank)} nesnesi oluşturuldu.");

        _userCode = userCode;
        _password = password;
    }

    // Garanti Bankası'na özel bağlantı işlemi.
    public void ConnectGaranti()
        => Console.WriteLine($"{nameof(GarantiBank)} - Connected.");

    public void SendMoney(int amount)
        => Console.WriteLine($"{amount} money sent.");
}



class HalkBank : IBank
{
    string _userCode, _password;

    // HalkBank sadece kullanıcı kodu ile oluşturulur.
    public HalkBank(string userCode)
    {
        Console.WriteLine($"{nameof(HalkBank)} nesnesi oluşturuldu.");

        _userCode = userCode;
    }

    // Şifre sonradan atanır.
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

    // Credential nesnesi ile oluşturulur.
    public VakifBank(CredentialVakifBank credential, string password)
    {
        Console.WriteLine($"{nameof(VakifBank)} nesnesi oluşturuldu.");

        _userCode = credential.UserCode;
        _email = credential.Mail;
        _password = password;
    }

    // Kimlik doğrulama işlemi.
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
    public IBank CreateInstance()
    {
        // GarantiBank oluşturulur.
        GarantiBank garanti = new("asd", "123");

        // Garanti'ye özel hazırlık işlemi yapılır.
        garanti.ConnectGaranti();

        return garanti;
    }
}



class HalkBankFactory : IBankFactory
{
    public IBank CreateInstance()
    {
        // HalkBank oluşturulur.
        HalkBank halkBank = new("asd");

        // Şifre atanır.
        halkBank.Password = "123";

        return halkBank;
    }
}



class VakifBankFactory : IBankFactory
{
    public IBank CreateInstance()
    {
        // Credential bilgileri hazırlanır.
        VakifBank vakifBank =
            new(
                new()
                {
                    Mail = "aaa@bbbb.com",
                    UserCode = "bbbb"
                },
                "123");

        // Kimlik doğrulama yapılır.
        vakifBank.ValidateCredential();

        return vakifBank;
    }
}

#endregion



#region Creator


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
        // Banka tipine göre uygun Factory seçilir.
        IBankFactory _bankFactory = bankType switch
        {
            BankType.Garanti => new GarantiFactory(),
            BankType.Halkbank => new HalkBankFactory(),
            BankType.Vakifbank => new VakifBankFactory()
        };

        // Factory ilgili banka nesnesini oluşturur
        // ve gerekli başlangıç işlemlerini tamamlar.
        return _bankFactory.CreateInstance();
    }
}

#endregion

/*
------------------------------------------
ÖNCEKİ FACTORY ÖRNEĞİNDEN FARKI
------------------------------------------

Simple Factory'de:

ProductCreator
    -> new A()
    -> new B()
    -> new C()

Factory sadece nesneyi oluşturuyordu.


Bu örnekte (Factory Method):

BankCreator
        |
        +--> GarantiFactory
        |       |
        |       +--> new GarantiBank()
        |       +--> ConnectGaranti()
        |
        +--> HalkBankFactory
        |       |
        |       +--> new HalkBank()
        |       +--> Password = "123"
        |
        +--> VakifBankFactory
                |
                +--> new VakifBank()
                +--> ValidateCredential()


Yani Factory sadece "new" yapmakla kalmıyor.

✔ Nesneyi oluşturuyor.
✔ Gerekli ayarları yapıyor.
✔ Bağlantıyı açıyor.
✔ Kimlik doğrulaması yapıyor.
✔ Nesneyi kullanıma hazır halde döndürüyor.

Bu yüzden gerçek hayatta Factory Method Pattern
genellikle bu şekilde kullanılır.
*/

// Bu örnekte hangi Factory'nin oluşturulacağı Reflection ile
// çalışma anında (Runtime) belirlenmektedir.

using System.Reflection;

BankCreator bankCreator = new();

// Factory ismi Reflection ile bulunur.
GarantiBank? garanti = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank = bankCreator.Create(BankType.HalkBank) as HalkBank;
VakifBank? vakifbank = bankCreator.Create(BankType.VakifBank) as VakifBank;

// Her Create() çağrısında yeni nesne oluşturulur.
GarantiBank? garanti2 = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank2 = bankCreator.Create(BankType.HalkBank) as HalkBank;
VakifBank? vakifbank2 = bankCreator.Create(BankType.VakifBank) as VakifBank;



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

    // GarantiBank oluşturulur.
    public GarantiBank(string userCode, string password)
    {
        Console.WriteLine($"{nameof(GarantiBank)} nesnesi oluşturuldu.");

        _userCode = userCode;
        _password = password;
    }

    // Garanti'ye özel bağlantı.
    public void ConnectGaranti()
        => Console.WriteLine($"{nameof(GarantiBank)} Connected.");

    public void SendMoney(int amount)
        => Console.WriteLine($"{amount} money sent.");
}



class HalkBank : IBank
{
    string _userCode, _password;

    // HalkBank oluşturulur.
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

    public bool isAuthentcation { get; set; }

    // Credential ile oluşturulur.
    public VakifBank(CredentialVakifBank credential, string password)
    {
        Console.WriteLine($"{nameof(VakifBank)} nesnesi oluşturuldu.");

        _userCode = credential.UserCode;
        _email = credential.Mail;
        _password = password;
    }

    // Kimlik doğrulama yapılır.
    public void ValidateCredential()
    {
        if (true)
            isAuthentcation = true;
    }

    public void SendMoneyToAccountNumber(int amount, string recipientName, string accountNumber)
        => Console.WriteLine($"{amount} money sent.");
}



// Yeni banka eklemek artık çok kolaydır.
// Sadece yeni Product sınıfı oluşturulur.
class IsBank : IBank
{

}

#endregion



#region Abstract Factory

// Tüm Factory'lerin ortak arayüzü.
interface IBankFactory
{
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

        // Gerekli hazırlık yapılır.
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
        // VakifBank oluşturulur.
        VakifBank vakifBank =
            new(
                new()
                {
                    Mail = "aaa@bbb.com",
                    UserCode = "aaa"
                },
                "123");

        // Doğrulama yapılır.
        vakifBank.ValidateCredential();

        return vakifBank;
    }
}



// Yeni banka için sadece Factory yazılması yeterlidir.
class IsBankFactory : IBankFactory
{
    public IBank CreateInstance()
    {
        return new IsBank();
    }
}

#endregion



#region Creator

// Desteklenen banka tipleri.
enum BankType
{
    Garanti,
    HalkBank,
    VakifBank,
    IsBank
}

class BankCreator
{
    public IBank Create(BankType bankType)
    {
        // Önceki örnekte burada switch vardı.

        // IBankFactory factory = bankType switch
        // {
        //      BankType.Garanti => new GarantiFactory(),
        //      ...
        // };



        // Bu örnekte switch tamamen kaldırılmıştır.

        // Enum ismi alınır.
        // Örneğin:
        // Garanti
        string factory = $"{bankType.ToString()}Factory";

        // Sonuç:
        // "GarantiFactory"



        // Reflection ile bu isimdeki sınıf bulunur.
        Type? type = Assembly
            .GetExecutingAssembly()
            .GetType(factory);



        // Reflection sayesinde nesne oluşturulur.
        // new GarantiFactory() yazmaya gerek kalmaz.
        IBankFactory? bankFactory =
            Activator.CreateInstance(type) as IBankFactory;



        // Factory üzerinden ilgili banka oluşturulur.
        return bankFactory.CreateInstance();
    }
}

#endregion



/*
==========================================================
ÖNCEKİ FACTORY METHOD ÖRNEĞİNDEN FARKI
==========================================================

Önceki örnekte;

switch vardı.

BankType.Garanti
        ↓
new GarantiFactory()

BankType.HalkBank
        ↓
new HalkBankFactory()

BankType.VakifBank
        ↓
new VakifBankFactory()

Her yeni banka eklendiğinde

switch

değiştiriliyordu.

==========================================================

Bu örnekte ise switch tamamen kaldırıldı.

Yerine Reflection kullanılıyor.

Örneğin;

BankType.Garanti

önce String'e çevrilir.

"Garanti"

sonra sonuna Factory eklenir.

"GarantiFactory"

Reflection bu isimdeki sınıfı bulur.

Activator.CreateInstance()

ile nesneyi oluşturur.

Artık

new GarantiFactory()

yazılmasına gerek kalmaz.

==========================================================

Yeni bir banka eklemek için;

1- Product oluştur.

class IsBank : IBank

2- Factory oluştur.

class IsBankFactory : IBankFactory

3- Enum'a ekle.

IsBank

Başka hiçbir kod değişmez.

BankCreator sınıfındaki Reflection
otomatik olarak

IsBankFactory

sınıfını bulup çalıştıracaktır.

==========================================================

Bu örneğin avantajı

✔ switch-case ortadan kalkar.

✔ BankCreator yeni Factory'leri bilmek zorunda değildir.

✔ Yeni Factory eklemek daha kolaydır.

✔ Kod daha esnek (Flexible) hale gelir.

==========================================================

Bu örnek;

Factory Method Pattern
+
Reflection

birlikte kullanılarak oluşturulmuş gelişmiş
bir Factory Method örneğidir.
*/
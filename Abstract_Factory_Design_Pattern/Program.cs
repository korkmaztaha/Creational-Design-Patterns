// ComputerCreator sınıfından bir nesne oluşturuyoruz.
// Bu sınıf bilgisayar üretme işlemini yönetecek.
ComputerCreator creator = new();


// Asus bilgisayarı oluşturuyoruz.
// CreateComputer metoduna AsusFactory gönderiyoruz.
// AsusFactory bize Asus marka CPU, RAM ve VideoCard üretecek.
Computer asus = creator.CreateComputer(new AsusFactory());


// Toshiba bilgisayarı oluşturuyoruz.
// CreateComputer metoduna ToshibaFactory gönderiyoruz.
// ToshibaFactory bize Toshiba marka CPU, RAM ve VideoCard üretecek.
Computer toshiba = creator.CreateComputer(new ToshibaFactory());



////////////////////////////////////////////////////////////
// Computer sınıfı
//
// Bir bilgisayarın sahip olduğu parçaları temsil eder.
// Bilgisayarın içinde CPU, RAM ve VideoCard bulunur.
////////////////////////////////////////////////////////////

class Computer
{

    // Constructor
    //
    // Dışarıdan gelen CPU, RAM ve VideoCard nesnelerini alır.
    // Bu parçaları bilgisayarın özelliklerine atar.
    public Computer(ICPU cPU, IRAM rAM, IVideoCard videoCard)
    {
        CPU = cPU;
        RAM = rAM;
        VideoCard = videoCard;
    }


    // Parametresiz constructor
    //
    // Boş bir bilgisayar nesnesi oluşturmak için kullanılır.
    public Computer()
    {

    }


    // Bilgisayarın işlemcisi
    public ICPU CPU { get; set; }


    // Bilgisayarın belleği
    public IRAM RAM { get; set; }


    // Bilgisayarın ekran kartı
    public IVideoCard VideoCard { get; set; }

}




////////////////////////////////////////////////////////////
// Abstract Products (Soyut Ürünler)
//
// Burada ürünlerin sadece kurallarını tanımlarız.
// CPU, RAM ve ekran kartının nasıl üretileceğini söylemeyiz.
//
// Sadece:
// "Bir CPU olmalı"
// "Bir RAM olmalı"
// "Bir VideoCard olmalı"
// deriz.
////////////////////////////////////////////////////////////


interface ICPU
{

}


interface IRAM
{

}


interface IVideoCard
{

}




////////////////////////////////////////////////////////////
// Concrete Products (Somut Ürünler)
//
// Gerçek olarak üretilecek sınıflardır.
//
// Bu sınıflar yukarıdaki interface'leri uygular.
////////////////////////////////////////////////////////////


// CPU ürününün gerçek hali
class CPU : ICPU
{

    // CPU oluşturulduğunda ekrana mesaj yazdırır.
    public CPU(string text)
        => Console.WriteLine(text);

}



// RAM ürününün gerçek hali
class RAM : IRAM
{

    // RAM oluşturulduğunda ekrana mesaj yazdırır.
    public RAM(string text)
        => Console.WriteLine(text);

}



// VideoCard ürününün gerçek hali
class VideoCard : IVideoCard
{

    // VideoCard oluşturulduğunda ekrana mesaj yazdırır.
    public VideoCard(string text)
        => Console.WriteLine(text);

}




////////////////////////////////////////////////////////////
// Abstract Factory (Soyut Fabrika)
//
// Tüm bilgisayar fabrikalarının uygulaması gereken
// metotları belirler.
//
// Her fabrika:
// - CPU üretmeli
// - RAM üretmeli
// - VideoCard üretmeli
////////////////////////////////////////////////////////////


interface IComputerFactory
{

    // CPU üretme metodu
    ICPU CreateCPU();


    // RAM üretme metodu
    IRAM CreateRAM();


    // Ekran kartı üretme metodu
    IVideoCard CreateVideoCard();

}




////////////////////////////////////////////////////////////
// Concrete Factory (Somut Fabrikalar)
//
// Gerçek üretim işlemleri burada yapılır.
//
// Her marka kendi parçalarını üretir.
////////////////////////////////////////////////////////////



// Asus marka bilgisayar parçalarını üreten fabrika
class AsusFactory : IComputerFactory
{


    // Asus CPU üretir.
    public ICPU CreateCPU()
        => new CPU("Asus CPU üretildi.");



    // Asus RAM üretir.
    public IRAM CreateRAM()
        => new RAM("Asus RAM üretildi.");



    // Asus ekran kartı üretir.
    public IVideoCard CreateVideoCard()
        => new VideoCard("Asus Video Card Üretildi");

}




// Toshiba marka bilgisayar parçalarını üreten fabrika
class ToshibaFactory : IComputerFactory
{


    // Toshiba CPU üretir.
    public ICPU CreateCPU()
        => new CPU("Toshiba CPU üretildi.");



    // Toshiba RAM üretir.
    public IRAM CreateRAM()
        => new RAM("Toshiba RAM üretildi.");



    // Toshiba ekran kartı üretir.
    public IVideoCard CreateVideoCard()
        => new VideoCard("Toshiba Video Card Üretildi");

}




// MSI marka bilgisayar parçalarını üreten fabrika
//
// Yeni bir marka eklemek istersek mevcut kodları değiştirmeyiz.
// Sadece yeni bir Factory oluştururuz.
class MSIFactory : IComputerFactory
{


    public ICPU CreateCPU()
        => new CPU("MSI CPU üretildi.");



    public IRAM CreateRAM()
        => new RAM("MSI RAM üretildi.");



    public IVideoCard CreateVideoCard()
        => new VideoCard("MSI Video Card Üretildi");

}




////////////////////////////////////////////////////////////
// Creator (Üretici)
//
// Fabrikadan parçaları ister.
// Gelen parçalar ile Computer nesnesi oluşturur.
//
// Burada önemli nokta:
//
// ComputerCreator hangi marka olduğunu bilmez.
//
// Asus mu?
// Toshiba mı?
// MSI mı?
//
// Bunu bilmez.
//
// Sadece IComputerFactory interface'i ile çalışır.
////////////////////////////////////////////////////////////


class ComputerCreator
{


    // Üretilecek parçaları tutacak değişkenler
    ICPU _cpu;
    IRAM _ram;
    IVideoCard _videoCard;



    // Gelen fabrikaya göre bilgisayar oluşturur.
    public Computer CreateComputer(IComputerFactory computerFactory)
    {


        // Fabrikadan CPU iste.
        //
        // AsusFactory geldiyse Asus CPU,
        // ToshibaFactory geldiyse Toshiba CPU üretir.
        _cpu = computerFactory.CreateCPU();



        // Fabrikadan RAM iste.
        _ram = computerFactory.CreateRAM();



        // Fabrikadan VideoCard iste.
        _videoCard = computerFactory.CreateVideoCard();



        // Üretilen parçalarla yeni bilgisayar oluştur.
        return new(_cpu, _ram, _videoCard);

    }

}
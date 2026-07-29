// Eskiden bilgisayar parçalarını tek tek oluşturuyorduk.
//
// Computer computer1 = new();
//
// CPU cpu = new();
// computer1.CPU = cpu;
//
// RAM ram = new();
// computer1.RAM = ram;
//
// VideoCard videoCard = new();
// computer1.VideoCard = videoCard;
//
// Bu yöntem zahmetlidir çünkü her parçayı tek tek oluşturup
// bilgisayara atamamız gerekir.


// ComputerCreator sınıfından bir nesne oluşturuyoruz.
// Bu sınıf bilgisayar oluşturma işlemini bizim yerimize yapacaktır.
ComputerCreator creator = new();


// Asus marka bilgisayar oluştur.
// Marka bilgisi enum olarak gönderiliyor.
Computer asus = creator.CreateComputer(ComputerType.Asus);


// Toshiba marka bilgisayar oluştur.
Computer toshiba = creator.CreateComputer(ComputerType.Toshiba);


// MSI marka bilgisayar oluştur.
Computer msi = creator.CreateComputer(ComputerType.MSI);



//////////////////////////////////////////////////////////////
// Computer Sınıfı
//
// Bir bilgisayarın sahip olduğu parçaları temsil eder.
//////////////////////////////////////////////////////////////

class Computer
{

    // Constructor
    //
    // Dışarıdan CPU, RAM ve VideoCard alarak
    // bilgisayarı oluşturur.
    public Computer(ICPU cPU, IRAM rAM, IVideoCard videoCard)
    {
        CPU = cPU;
        RAM = rAM;
        VideoCard = videoCard;
    }


    // Parametresiz constructor
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



#region Abstract Products

//////////////////////////////////////////////////////////////
// Abstract Products (Soyut Ürünler)
//
// Burada ürünlerin sadece sözleşmesi tanımlanır.
// Nasıl üretilecekleri belirtilmez.
//////////////////////////////////////////////////////////////

interface ICPU { }

interface IRAM { }

interface IVideoCard { }

#endregion



#region Concrete Products

//////////////////////////////////////////////////////////////
// Concrete Products (Somut Ürünler)
//
// Gerçek ürün sınıflarıdır.
//
// Constructor içerisinde ekrana bilgi yazdırılıyor.
//////////////////////////////////////////////////////////////


// Gerçek CPU sınıfı
class CPU : ICPU
{
    public CPU(string text)
        => Console.WriteLine(text);
}


// Gerçek RAM sınıfı
class RAM : IRAM
{
    public RAM(string text)
        => Console.WriteLine(text);
}


// Gerçek VideoCard sınıfı
class VideoCard : IVideoCard
{
    public VideoCard(string text)
        => Console.WriteLine(text);
}

#endregion



#region Abstract Factory

//////////////////////////////////////////////////////////////
// Abstract Factory (Soyut Fabrika)
//
// Bütün fabrikaların uygulaması gereken metotları belirler.
//////////////////////////////////////////////////////////////

interface IComputerFactory
{

    // CPU üret
    ICPU CreateCPU();

    // RAM üret
    IRAM CreateRAM();

    // VideoCard üret
    IVideoCard CreateVideoCard();

}

#endregion




#region Concrete Factories

//////////////////////////////////////////////////////////////
// Concrete Factories (Somut Fabrikalar)
//
// Her fabrika kendi markasına ait parçaları üretir.
//////////////////////////////////////////////////////////////



// Asus fabrikası
class AsusFactory : IComputerFactory
{

    // Asus CPU üret
    public ICPU CreateCPU()
        => new CPU("Asus CPU üretildi.");


    // Asus RAM üret
    public IRAM CreateRAM()
        => new RAM("Asus RAM üretildi.");


    // Asus VideoCard üret
    public IVideoCard CreateVideoCard()
        => new VideoCard("Asus Video Card Üretildi");

}



// Toshiba fabrikası
class ToshibaFactory : IComputerFactory
{

    // Toshiba CPU üret
    public ICPU CreateCPU()
        => new CPU("Toshiba CPU üretildi.");


    // Toshiba RAM üret
    public IRAM CreateRAM()
        => new RAM("Toshiba RAM üretildi.");


    // Toshiba VideoCard üret
    public IVideoCard CreateVideoCard()
        => new VideoCard("Toshiba Video Card Üretildi");

}



// MSI fabrikası
class MSIFactory : IComputerFactory
{

    // MSI CPU üret
    public ICPU CreateCPU()
        => new CPU("MSI CPU üretildi.");


    // MSI RAM üret
    public IRAM CreateRAM()
        => new RAM("MSI RAM üretildi.");


    // MSI VideoCard üret
    public IVideoCard CreateVideoCard()
        => new VideoCard("MSI Video Card Üretildi");

}

#endregion




#region Creator

//////////////////////////////////////////////////////////////
// ComputerType Enum
//
// Kullanıcının hangi marka bilgisayar istediğini belirtir.
//////////////////////////////////////////////////////////////

enum ComputerType
{
    Asus,
    MSI,
    Toshiba
}



//////////////////////////////////////////////////////////////
// ComputerCreator
//
// Bilgisayar üretimini yöneten sınıftır.
//
// Önce hangi fabrikanın kullanılacağını belirler,
// sonra o fabrikadan parçaları üretmesini ister.
//////////////////////////////////////////////////////////////

class ComputerCreator
{

    // Üretilecek parçalar için alanlar
    ICPU _cpu;
    IRAM _ram;
    IVideoCard _videoCard;



    // Kullanıcı sadece marka seçer.
    //
    // Factory seçimini artık kullanıcı yapmaz.
    // Bu işi ComputerCreator yapar.
    public Computer CreateComputer(ComputerType computerType)
    {

        //////////////////////////////////////////////////////
        // switch expression
        //
        // Gelen marka bilgisine göre uygun Factory seçiliyor.
        //////////////////////////////////////////////////////

        IComputerFactory computerFactory = computerType switch
        {

            // Eğer MSI seçilmişse
            // MSIFactory oluştur.
            ComputerType.MSI => new MSIFactory(),


            // Eğer Toshiba seçilmişse
            // ToshibaFactory oluştur.
            ComputerType.Toshiba => new ToshibaFactory(),


            // Eğer Asus seçilmişse
            // AsusFactory oluştur.
            ComputerType.Asus => new AsusFactory()

        };


        //////////////////////////////////////////////////////
        // Artık elimizde doğru Factory var.
        // Şimdi parçaları üretmesini istiyoruz.
        //////////////////////////////////////////////////////

        // CPU üret
        _cpu = computerFactory.CreateCPU();


        // RAM üret
        _ram = computerFactory.CreateRAM();


        // VideoCard üret
        _videoCard = computerFactory.CreateVideoCard();


        //////////////////////////////////////////////////////
        // Üretilen parçalarla bilgisayarı oluştur.
        //////////////////////////////////////////////////////

        return new(_cpu, _ram, _videoCard);

    }

}

#endregion
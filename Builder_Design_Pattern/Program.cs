
// ============================================================
// BUILDER DESIGN PATTERN
// ============================================================
//
// Amaç:
//
// Araba gibi bir nesnenin oluşturulması birden fazla adımdan
// oluşuyorsa, bu adımları doğrudan ana kod içerisinde yapmak
// yerine bir "Builder" sınıfına bırakabiliriz.
//
// Örneğin normalde:
//
//     Araba mercedes = new();
//     mercedes.KM = 100;
//     mercedes.Marka = "Mercedes";
//     mercedes.Model = "xyz";
//     mercedes.Vites = true;
//
// şeklinde nesneyi kendimiz oluşturabiliriz.
//
// Ancak farklı araba türleri varsa:
//
//     Opel
//     Mercedes
//     BMW
//
// her biri için farklı değerler vermemiz gerekir.
//
// Builder Pattern burada devreye girer.
//
// Her araba için farklı bir Builder oluştururuz:
//
//     OpelBuilder
//     MercedesBuilder
//     BMWBuilder
//
// Director ise bu Builder'ların hangi sırayla
// çalıştırılacağını yönetir.
//


// ============================================================
// CLIENT
// ============================================================

// ArabaDirector nesnesi oluşturuyoruz.
//
// Director'ın görevi, ArabaBuilder üzerinden
// arabanın hangi adımlarla oluşturulacağını yönetmektir.
ArabaDirector director = new();


// ============================================================
// OPEL
// ============================================================

// OpelBuilder oluşturuyoruz.
//
// Director'a "bana Opel oluştur" demiyoruz.
// Aslında:
//
//     OpelBuilder
//
// nesnesini Director'a veriyoruz.
//
// Director da bu Builder'ın metotlarını çalıştırarak
// Opel nesnesini oluşturuyor.
Araba opel = director.Build(new OpelBuilder());


// Arabanın bilgilerini ekrana yazdırıyoruz.
//
// Burada ToString() override edildiği için
// Araba sınıfındaki ToString() metodu çalışacaktır.
opel.ToString();


// ============================================================
// MERCEDES
// ============================================================

// Mercedes için MercedesBuilder kullanıyoruz.
//
// Director yine aynı Build() metodunu kullanıyor.
//
// Ancak gönderdiğimiz Builder farklı olduğu için
// farklı özelliklere sahip bir Araba oluşturuluyor.
Araba mercedes = director.Build(new MercedesBuilder());


// Mercedes bilgilerini ekrana yazdırıyoruz.
mercedes.ToString();


// ============================================================
// BMW
// ============================================================

// BMW için BMWBuilder kullanıyoruz.
Araba bmw = director.Build(new BMWBuilder());


// BMW bilgilerini ekrana yazdırıyoruz.
bmw.ToString();


// ============================================================
// PRODUCT
// ============================================================
//
// Builder Pattern'da oluşturulmak istenen gerçek nesneye
// "Product" denir.
//
// Bizim Product'ımız:
//
//     Araba
//
// sınıfıdır.
class Araba
{
  
    public string Marka { get; set; }


    
    public string Model { get; set; }


    
    public double KM { get; set; }


    
    public bool Vites { get; set; }


    public override string ToString()
    {
        Console.WriteLine(
            $"{Marka} marka araba " +
            $"{Model} modelinde " +
            $"{KM} kilometrede " +
            $"{Vites} vites olarak üretilmiştir."
        );


       
        return base.ToString();
    }
}


// ============================================================
// ABSTRACT BUILDER
// ============================================================
//
// Abstract Builder:
//
// Araba üretirken hangi adımların bulunacağını tanımlar.
//
// Bizim örneğimizde her araba için:
//
//     SetMarka()
//     SetModel()
//     SetKM()
//     SetVites()
//
// işlemlerinin yapılması gerekiyor.
//
// Bu metotların nasıl yapılacağını Concrete Builder'lar
// belirleyecek.
abstract class ArabaBuilder
{
    // ========================================================
    // PRODUCT
    // ========================================================

    // Oluşturulacak Araba nesnesini tutuyoruz.
    //
    // protected olduğu için bu alana:
    //
    //     ArabaBuilder
    //
    // ve ondan türeyen sınıflar erişebilir.
    protected Araba araba;


    // Product'a dışarıdan erişebilmek için property.
    //
    // Director, işlemler tamamlandıktan sonra
    // oluşturulan Araba nesnesini buradan alıyor.
    public Araba Araba
    {
        get => araba;
    }


    // ========================================================
    // CONSTRUCTOR
    // ========================================================

    // Builder oluşturulduğunda otomatik olarak
    // yeni bir Araba oluşturuyoruz.
    public ArabaBuilder()
        => araba = new();


    // ========================================================
    // ABSTRACT BUILD STEPS
    // ========================================================

    // Bu metotlar Builder'ın gerçekleştirmesi gereken
    // adımları tanımlıyor.
    //
    // Nasıl yapılacağını alt sınıflar belirleyecek.

    public abstract ArabaBuilder SetMarka();

    public abstract ArabaBuilder SetModel();

    public abstract ArabaBuilder SetKM();

    public abstract ArabaBuilder SetVites();
}


// ============================================================
// CONCRETE BUILDER - OPEL
// ============================================================
//
// OpelBuilder, ArabaBuilder'dan miras alıyor.
//
// Abstract sınıfta tanımlanan bütün abstract metotları
// override etmek zorunda.
class OpelBuilder : ArabaBuilder
{
    // Opel'in kilometre bilgisini ayarlıyoruz.
    public override ArabaBuilder SetKM()
    {
        araba.KM = 0;

        // this döndürülüyor.
        //
        // Bunun sayesinde method chaining yapabiliyoruz:
        //
        // SetMarka()
        //     .SetModel()
        //     .SetKM()
        //     .SetVites()
        //
        return this;
    }


    // Opel'in markasını ayarlıyoruz.
    public override ArabaBuilder SetMarka()
    {
        araba.Marka = "Opel";

        return this;
    }


    // Opel'in modelini ayarlıyoruz.
    public override ArabaBuilder SetModel()
    {
        araba.Model = "...";

        return this;
    }


    // Opel'in vites bilgisini ayarlıyoruz.
    public override ArabaBuilder SetVites()
    {
        araba.Vites = true;

        return this;
    }
}


// ============================================================
// CONCRETE BUILDER - MERCEDES
// ============================================================
//
// Mercedes için ArabaBuilder'dan türeyen
// ayrı bir Builder oluşturuyoruz.
class MercedesBuilder : ArabaBuilder
{
    // Mercedes'in kilometresi.
    public override ArabaBuilder SetKM()
    {
        araba.KM = 100;

        return this;
    }


    // Mercedes'in markası.
    public override ArabaBuilder SetMarka()
    {
        araba.Marka = "Mercedes";

        return this;
    }


    // Mercedes'in modeli.
    public override ArabaBuilder SetModel()
    {
        araba.Model = "xyz";

        return this;
    }


    // Mercedes'in vites bilgisi.
    public override ArabaBuilder SetVites()
    {
        araba.Vites = true;

        return this;
    }
}


// ============================================================
// CONCRETE BUILDER - BMW
// ============================================================
//
// BMW için de ayrı bir Concrete Builder oluşturuyoruz.
class BMWBuilder : ArabaBuilder
{
    // BMW'nin kilometresi.
    public override ArabaBuilder SetKM()
    {
        araba.KM = 10;

        return this;
    }


    // BMW'nin markası.
    public override ArabaBuilder SetMarka()
    {
        araba.Marka = "BMW";

        return this;
    }


    // BMW'nin modeli.
    public override ArabaBuilder SetModel()
    {
        araba.Model = "XY5";

        return this;
    }


    // BMW'nin vites bilgisi.
    public override ArabaBuilder SetVites()
    {
        araba.Vites = false;

        return this;
    }
}


// ============================================================
// DIRECTOR
// ============================================================
//
// Director'ın görevi Product'ı doğrudan oluşturmak değildir.
//
// Director, Builder'a:
//
//     hangi adımların
//     hangi sırayla
//
// uygulanacağını söyler.
//
// Yani oluşturma sürecini yönetir.
class ArabaDirector
{
    // Build metodu herhangi bir ArabaBuilder kabul ediyor.
    //
    // Buraya:
    //
    //     OpelBuilder
    //     MercedesBuilder
    //     BMWBuilder
    //
    // gönderebiliriz.
    //
    // Çünkü bunların hepsi ArabaBuilder'dan türemiştir.
    public Araba Build(ArabaBuilder arabaBuilder)
    {
        // ====================================================
        // BUILD STEPS
        // ====================================================

        // Arabanın markasını belirle.
        //
        // arabaBuilder.SetMarka();


        // Modelini belirle.
        //
        // arabaBuilder.SetModel();


        // Kilometresini belirle.
        //
        // arabaBuilder.SetKM();


        // Vites bilgisini belirle.
        //
        // arabaBuilder.SetVites();


        // Yukarıdaki işlemlerin hepsini
        // method chaining kullanarak tek satırda yapıyoruz.
        //
        // Her metot "this" döndürdüğü için
        // bir sonraki metodu çağırabiliyoruz.
        arabaBuilder
            .SetMarka()
            .SetModel()
            .SetKM()
            .SetVites();


        // Bütün build işlemleri tamamlandıktan sonra
        // oluşturulan Araba nesnesini döndürüyoruz.
        return arabaBuilder.Araba;
    }
}


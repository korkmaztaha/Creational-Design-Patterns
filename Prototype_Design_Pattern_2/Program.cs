//////////////////////////////////////////////////////////////
// Yeni bir Person nesnesi oluşturuyoruz.
//
// Constructor çalışır ve bellekte yeni bir nesne oluşturulur.
//////////////////////////////////////////////////////////////

Person person1 = new(
    "aaa",
    "bbb",
    Department.A,
    2500,
    500);



//////////////////////////////////////////////////////////////
// Clone işlemi
//
// ICloneable.Clone() metodu object döndürdüğü için
// geri dönen değeri Person'a çevirmemiz gerekir.
//////////////////////////////////////////////////////////////


// 1. yöntem (Explicit Cast)
//
// Eğer dönen nesne Person değilse hata oluşur.
//
// Person person2 = (Person)person1.Clone();



// 2. yöntem (as operatörü)
//
// Eğer dönüşüm başarısız olursa null döner.
//
// Bu yüzden değişken Person? olarak tanımlanmıştır.
Person? person2 = person1.Clone() as Person;



//////////////////////////////////////////////////////////////
// Kopyalanan nesnenin bilgilerini değiştiriyoruz.
//////////////////////////////////////////////////////////////

person2.Name = "ccc";

person2.Salary = 1000;

Console.WriteLine();



//////////////////////////////////////////////////////////////
// Concrete Prototype
//
// ICloneable interface'ini uygulayan sınıf.
//////////////////////////////////////////////////////////////

class Person : ICloneable
{

    //////////////////////////////////////////////////////////
    // Constructor
    //
    // Yeni Person oluşturulduğunda çalışır.
    //////////////////////////////////////////////////////////

    public Person(
        string name,
        string surname,
        Department department,
        int salary,
        int premium)
    {
        Name = name;
        Surname = surname;
        Department = department;
        Salary = salary;
        Premium = premium;

        Console.WriteLine("Person nesnesi oluşturuldu.");
    }



    //////////////////////////////////////////////////////////
    // Person bilgileri
    //////////////////////////////////////////////////////////

    public string Name { get; set; }

    public string Surname { get; set; }

    public Department Department { get; set; }

    public int Salary { get; set; }

    public int Premium { get; set; }



    //////////////////////////////////////////////////////////
    // Clone metodu
    //
    // ICloneable interface'i object döndürmek zorundadır.
    //////////////////////////////////////////////////////////

    public object Clone()
    {

        //////////////////////////////////////////////////////
        // MemberwiseClone()
        //
        // Mevcut nesnenin yüzeysel (Shallow Copy)
        // kopyasını oluşturur.
        //
        // Constructor tekrar çalışmaz.
        //////////////////////////////////////////////////////

        return base.MemberwiseClone();

    }

}



//////////////////////////////////////////////////////////////
// Departmanlar
//////////////////////////////////////////////////////////////

enum Department
{
    A,
    B,
    C
}
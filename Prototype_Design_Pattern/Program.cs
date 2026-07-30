//////////////////////////////////////////////////////////////
// Yeni bir Person nesnesi oluşturuyoruz.
//
// Constructor çalışır ve nesne oluşturulur.
//////////////////////////////////////////////////////////////

Person person1 = new(
    "aaa",
    "bbb",
    Department.C,
    100,
    10
);


// Eğer aşağıdaki gibi ikinci kişiyi oluşturursak
// constructor tekrar çalışacaktır.
//
// Person person2 = new(
//    "vvv",
//    "bbb",
//     Department.C,
//     100,
//     10
// );
//
// Bu durumda bellekte sıfırdan yeni bir nesne oluşturulur.



//////////////////////////////////////////////////////////////
// Bunun yerine Prototype Pattern kullanıyoruz.
//
// person1 nesnesinin birebir kopyasını oluşturuyoruz.
//////////////////////////////////////////////////////////////

Person person2 = person1.Clone();


// Kopyalanan nesnenin sadece adını değiştiriyoruz.
person2.Name = "vvv";

Console.WriteLine();



//////////////////////////////////////////////////////////////
// Abstract Prototype
//
// Clone edilebilecek bütün sınıfların uygulaması gereken
// sözleşmedir.
//////////////////////////////////////////////////////////////

interface IPersonCloneable
{

    // Nesnenin kopyasını oluştur.
    Person Clone();

}



//////////////////////////////////////////////////////////////
// Concrete Prototype
//
// Gerçek clone işlemini yapan sınıf.
//////////////////////////////////////////////////////////////

class Person : IPersonCloneable
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
    // Mevcut nesnenin birebir kopyasını oluşturur.
    //////////////////////////////////////////////////////////

    public Person Clone()
    {

        //////////////////////////////////////////////////////
        // MemberwiseClone()
        //
        // Mevcut nesnenin yüzeysel (Shallow Copy)
        // kopyasını oluşturur.
        //
        // Yeni constructor çalışmaz.
        //////////////////////////////////////////////////////

        return (Person)base.MemberwiseClone();

    }

}



//////////////////////////////////////////////////////////////
// Person'ın çalıştığı departmanlar
//////////////////////////////////////////////////////////////

enum Department
{
    A,
    B,
    C
}
// new Example();           // Hata verir. Constructor private olduğu için dışarıdan nesne oluşturulamaz.


Example ex1 = Example.GetInstance; // İlk erişim
Example ex2 = Example.GetInstance; // Aynı nesne gelir
Example ex3 = Example.GetInstance; // Aynı nesne gelir
Example ex4 = Example.GetInstance;
Example ex5 = Example.GetInstance;
Example ex6 = Example.GetInstance;
Example ex7 = Example.GetInstance;
Example ex8 = Example.GetInstance;

class Example
{
    // Dışarıdan new Example() yapılmasını engeller.
    private Example()
    {
        // Nesne oluşturulduğunda sadece 1 kez çalışmalıdır.
        Console.WriteLine($"{nameof(Example)} nesnesi oluşturuldu.");
    }

    // Tek oluşturulacak nesneyi tutan static const.
    static Example _example;

    // Nesneye erişmek için kullanılan özellik.
    public static Example GetInstance
    {
        get
        {
            #region 1. Yöntem 

            // Eğer nesne daha önce oluşturulmadıysa oluştur.
            if (_example == null)
                _example = new Example();
            // Oluşturulan tek nesneyi döndür.
            return _example;
            #endregion
            
            #region 2. Yöntem
            // Sadece değişkeni döndürür.
            // _example oluşturulmadığı için null döner.
            // return _example;
            #endregion
        }
    }
}

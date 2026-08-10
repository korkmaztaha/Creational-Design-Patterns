
using System.Collections.Concurrent;

// ObjectPool sınıfımızı oluşturuyoruz.
ObjectPool<X> pools = new ObjectPool<X>();

// Havuzdan bir X nesnesi istiyoruz.
//
// Havuzda kullanılabilir X varsa onu döndürür.
// Havuz boşsa () => new X() çalışır ve yeni X oluşturulur.
var x1 = pools.Get(() => new X());

x1.Count++;


// x1'i kullandıktan sonra tekrar havuza bırakıyoruz.
// Artık x1 başka bir Get() çağrısında tekrar kullanılabilir.
pools.Return(x1);


// --------------------------------------------------


// Tekrar havuzdan X istiyoruz.
//
// Dikkat: Havuzda daha önce Return() ettiğimiz x1 var.
// Bu yüzden yeni X oluşturulmayacak.
// x2 aslında x1 ile AYNI nesneyi gösterebilir.
var x2 = pools.Get(() => new X());

x2.Count++;

pools.Return(x2);


// --------------------------------------------------


// Yine havuzdan X istiyoruz.
//
// Havuzda x2 (aslında aynı nesne) bulunduğu için
// tekrar new X() yapılmayacak.
var x3 = pools.Get(() => new X());

x3.Count++;

pools.Return(x3);



Console.WriteLine();


// ==================================================
// OBJECT POOL
// ==================================================

class ObjectPool<T> where T : class
{
    // Havuzdaki nesneleri tutuyoruz.
    //
    // ConcurrentBag thread-safe bir koleksiyondur.
    // Yani birden fazla thread aynı anda havuza
    // nesne ekleyip çıkarabilir.
    private readonly ConcurrentBag<T> _instances;


    // Constructor
    public ObjectPool()
    {
        // Boş bir ConcurrentBag oluşturuyoruz.
        _instances = new ConcurrentBag<T>();
    }


    // İstenirse havuzdaki nesnelere erişilebilir.
    public ConcurrentBag<T> Instances
    {
        get => _instances;
    }


    // Havuzdan nesne alma metodu.
    //
    // Func<T>? objectGenerator:
    // Havuzda nesne yoksa yeni nesne üretmek için
    // kullanılacak fonksiyondur.
    //
    // Örneğin:
    // pools.Get(() => new X());
    //
    // Buradaki () => new X(), Func<X> tipindedir.
    
    
    public T Get(Func<T>? objectGenerator = null)
    {
        //Havuzdan generic parametrede bildirilen türdeki nesneyi geri döndürmek.
        return _instances.TryTake(out T instance) ? instance : objectGenerator();
    }

   


    // Kullanılan nesneyi tekrar havuza iade eder.
    public void Return(T instance)
    {
        // Kullanımı biten nesneyi havuza ekliyoruz.
        //
        // Böylece bir sonraki Get() çağrısında
        // bu nesne tekrar kullanılabilir.
        _instances.Add(instance);
    }
}


// ==================================================
// X SINIFI
// ==================================================

class X
{
    // Nesnenin Count değerini tutuyoruz.
    public int Count { get; set; }


    // Count değerini ekrana yazdırır.
    public void Write()
        => Console.WriteLine(Count);


    // Constructor
    //
    // Gerçek hayatta burada pahalı bir işlem olduğunu düşünelim.
    // Örneğin:
    // - Büyük bir buffer oluşturmak
    // - Database connection hazırlamak
    // - Büyük bir array oluşturmak
    // - Karmaşık bir obje oluşturmak
    public X()
        => Console.WriteLine("X üretim maliyeti...");


   
    ~X()
        => Console.WriteLine("X imha maliyeti...");
}


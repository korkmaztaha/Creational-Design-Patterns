
using System.Collections.Concurrent;


// =

// ObjectPool'un Singleton instance'ını alıyoruz.
//
// GetInstance sayesinde ObjectPool'dan kendimiz new ObjectPool()
// yapmak zorunda kalmıyoruz.
//
// Uygulama boyunca aynı ObjectPool instance'ı kullanılır.
ObjectPool<X> pools = ObjectPool<X>.GetInstance;


// ------------------------------------------------------------
// 1. NESNE İSTEME
// ------------------------------------------------------------

// Havuzdan bir X nesnesi istiyoruz.
//
// Eğer havuzda X nesnesi varsa:
//     Havuzdaki mevcut X geri döndürülür.
//
// Eğer havuz boşsa:
//     () => new X() çalışır ve yeni X oluşturulur.
var x1 = pools.Get(() => new X());


// x1 nesnesinin Count değerini 1 artırıyoruz.
x1.Count++;


// Kullanımımız bitti.
// Nesneyi Garbage Collector'a bırakmak yerine
// tekrar ObjectPool'a iade ediyoruz.
pools.Return(x1);


// ------------------------------------------------------------
// 2. NESNE İSTEME
// ------------------------------------------------------------

// Havuzdan tekrar X istiyoruz.
//
// Havuzda az önce Return(x1) ile bıraktığımız X var.
//
// Dolayısıyla:
//     new X()
// çalışmayacak.
//
// x2, havuzdan aldığımız mevcut nesnedir.
var x2 = pools.Get(() => new X());


// x2'nin Count değerini artırıyoruz.
x2.Count++;


// Kullanım bitince tekrar havuza bırakıyoruz.
pools.Return(x2);


// ------------------------------------------------------------
// 3. NESNE İSTEME
// ------------------------------------------------------------

// Havuzda yine X bulunduğu için
// yeni bir X oluşturulmayacak.
//
// Havuzdaki mevcut X tekrar kullanılacak.
var x3 = pools.Get(() => new X());


// Count değerini tekrar artırıyoruz.
x3.Count++;


// Nesneyi tekrar havuza bırakıyoruz.
pools.Return(x3);


Console.WriteLine();


// ============================================================
// OBJECT POOL
// ============================================================

class ObjectPool<T> where T : class
{
    // ========================================================
    // POOL
    // ========================================================

    // Havuzda kullanılmayı bekleyen nesneleri tutuyoruz.

    // Birden fazla thread aynı anda nesne alıp bırakabilir.
    private readonly ConcurrentBag<T> _instances;


    // ========================================================
    // CONSTRUCTOR
    // ========================================================

    // Constructor private.
    //
    // Bunun önemli bir sebebi var:
    //
    // Dışarıdan:
    //
    // new ObjectPool<X>()
    //
    // yapılmasını engelliyoruz.
    //
    // Çünkü ObjectPool'u Singleton olarak kullanmak istiyoruz.
    private ObjectPool()
        => _instances = new();


    // ========================================================
    // SINGLETON
    // ========================================================

    // ObjectPool'un tek instance'ını burada tutuyoruz.
    //
    // static olduğu için ObjectPool<T>'ye ait tek bir alan vardır.
    private static ObjectPool<T> _objectPool;


    // Static constructor.
    //
    // ObjectPool<T> ilk defa kullanıldığında çalışır.
    //
    // Burada yalnızca bir tane ObjectPool oluşturuyoruz.
    static ObjectPool()
        => _objectPool = new ObjectPool<T>();


    // Singleton instance'ına erişmek için kullanılır.
    //
    // Örneğin:
    //
    // ObjectPool<X>.GetInstance
    //
    // dediğimizde yukarıda oluşturulan tek ObjectPool instance'ı
    // bize döndürülür.
    public static ObjectPool<T> GetInstance
    {
        get => _objectPool;
    }


    // ========================================================
    // INSTANCES
    // ========================================================

    // Havuzdaki nesnelere erişmemizi sağlar.
    //
    // Örneğin:
    //
    // pools.Instances
    //
    // dediğimizde ConcurrentBag<T>'yi alabiliriz.
    public ConcurrentBag<T> Instances
    {
        get => _instances;
    }


    // ========================================================
    // GET
    // ========================================================

    // Havuzdan bir nesne almak için kullanılır.
    //
    // objectGenerator:
    // Havuz boş olduğunda yeni nesne oluşturmak için
    // kullanılacak fonksiyondur.
    //
    // Örneğin:
    //
    // pools.Get(() => new X());
    //
    // Buradaki:
    //
    // () => new X()
    //
    // bir Func<X> değeridir.
    public T Get(Func<T>? objectGenerator = null)
    {
        // Önce havuzda kullanılabilir bir nesne var mı
        // diye kontrol ediyoruz.
        //
        // TryTake başarılı olursa:
        //
        //     instance
        //
        // değişkenine havuzdaki nesne atanır ve true döner.
        //
        // Başarısız olursa havuz boştur ve false döner.
        return _instances.TryTake(out T instance)
            
            // Havuzda nesne varsa mevcut nesneyi döndür.
            ? instance

            // Havuz boşsa yeni nesne oluştur.
            : objectGenerator();
    }


    // ========================================================
    // RETURN
    // ========================================================

    // Kullanımı biten nesneyi tekrar havuza bırakır.
    public void Return(T instance)
    {
        // Nesneyi havuza ekliyoruz.
        //
        // Böylece sonraki Get() çağrısında bu nesne
        // tekrar kullanılabilir.
        _instances.Add(instance);
    }
}


// ============================================================
// X
// ============================================================

class X
{
    // Nesnenin Count değerini tutar.
    public int Count { get; set; }


    // Count değerini ekrana yazdırır.
    public void Write()
        => Console.WriteLine(Count);


    // ========================================================
    // CONSTRUCTOR
    // ========================================================


    public X()
        => Console.WriteLine("X üretim maliyeti...");


    // ========================================================
    // FINALIZER
    // ========================================================


    ~X()
        => Console.WriteLine("X imha maliyeti...");
}


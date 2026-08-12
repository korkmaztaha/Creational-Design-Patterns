
using System.Collections.Concurrent;


// ============================================================
// OBJECT POOL'A ERİŞİM
// ============================================================

// ObjectPool'un Singleton instance'ını alıyoruz.
//
// Burada uygulama içerisinde tek bir ObjectPool<X>
// instance'ı kullanılması amaçlanıyor.
ObjectPool<X> pool = ObjectPool<X>.GetInstance;


// ============================================================
// 1. THREAD / TASK
// ============================================================

// Task.Run ile ayrı bir thread üzerinde çalışabilecek
// bir iş başlatıyoruz.
var t1 = Task.Run(() =>
{
    // Sürekli çalışacak.
    //
  
    while (true)
    {
        // ObjectPool'dan X nesnesi istiyoruz.
        //
        // Havuzda nesne varsa:
        //     mevcut X alınır.
        //
        // Havuz boşsa:
        //     () => new X()
        //     ile yeni X oluşturulabilir.
        var x = pool.Get(() => new X());


        // Get() null döndürmüş olabilir.
        // Bu yüzden kontrol ediyoruz.
        if (x != null)
        {
            // X'in Count değerini 1 artırıyoruz.
            x.Count++;


            // Count değerini ekrana yazdırıyoruz.
            x.Write();


            // Kullanımımız bitti.
            //
            // X nesnesini tekrar ObjectPool'a bırakıyoruz.
            pool.Return(x);
        }
    }
});


// ============================================================
// 2. THREAD / TASK
// ============================================================

// İkinci bir Task daha başlatıyoruz.
//
// Böylece aynı ObjectPool'a iki farklı iş parçacığı
// aynı anda erişmeye çalışıyor.
var t2 = Task.Run(() =>
{
    while (true)
    {
        // ObjectPool'dan X istiyoruz.
        var x = pool.Get(() => new X());


        // Nesne alınabildiyse devam ediyoruz.
        if (x != null)
        {
            // Count değerini artırıyoruz.
            x.Count++;


            // Değeri ekrana yazdırıyoruz.
            x.Write();


            // Nesneyi tekrar havuza bırakıyoruz.
            pool.Return(x);
        }
    }
});


// ============================================================
// TASK'LERİ BEKLEME
// ============================================================

// İki Task'in de tamamlanmasını bekliyoruz.
//
// Ancak yukarıdaki Task'lerde:
//
//     while (true)
//
// olduğu için Task'ler hiçbir zaman kendiliğinden
// tamamlanmayacaktır.
//
// Dolayısıyla program burada sürekli bekleyecektir.
await Task.WhenAll(t1, t2);



// ============================================================
// CONCURRENTBAG
// ============================================================

/*
    ConcurrentBag<T>, thread-safe bir koleksiyondur.

    Birden fazla thread aynı ConcurrentBag üzerinde
    Add() ve TryTake() işlemlerini güvenli şekilde yapabilir.

    Örneğin:

        Thread 1 ---> Add(X)
        Thread 2 ---> TryTake(X)
        Thread 3 ---> Add(X)

    gibi işlemler aynı anda gerçekleşebilir.


    ConcurrentBag'in önemli özelliklerinden biri,
    thread-local yapılardan yararlanarak performansı
    artırmaya çalışmasıdır.

    Yani her thread'in kendi tarafında tuttuğu
    elemanlar bulunabilir.

    Bir thread kendi tarafında uygun eleman bulamazsa
    diğer thread'lerin taraflarından eleman "çalabilir"
    (stealing).

    Bu yüzden ConcurrentBag'i:

        "Her thread kesinlikle sadece kendi eklediği
         son elemanı alır."

    şeklinde düşünmemek gerekir.

    Daha doğru ifade:

        "ConcurrentBag, thread-safe ve sırasız bir
         koleksiyondur; eleman alma sırası garanti edilmez."

    Dolayısıyla Stack gibi kesin LIFO,
    Queue gibi kesin FIFO davranışı beklememeliyiz.
*/


// ============================================================
// OBJECT POOL
// ============================================================

class ObjectPool<T> where T : class
{
    // ========================================================
    // POOL
    // ========================================================

    // Havuzda kullanılmayı bekleyen nesneleri tutuyoruz.
    //
    // ConcurrentBag olduğu için birden fazla thread
    // bu koleksiyona güvenli şekilde erişebilir.
    private readonly ConcurrentBag<T> _instances;


    // ========================================================
    // TYPE LISTESI
    // ========================================================

    // Daha önce bu tipten bir nesne üretilmiş mi
    // bilgisini tutmak için kullanılıyor.
    //
    // Örneğin:
    //
    // _types = [ "T" ]
    //
    // gibi bir yapı oluşması amaçlanıyor.
    //
   
    private readonly List<string> _types = new();


    // ========================================================
    // CONSTRUCTOR
    // ========================================================

    // Constructor private.
    //
    // Çünkü ObjectPool'u Singleton olarak kullanıyoruz.
    //
    // Dışarıdan:
    //
    //     new ObjectPool<X>()
    //
    // yapılmasını istemiyoruz.
    private ObjectPool()
        => _instances = new();


    // ========================================================
    // SINGLETON
    // ========================================================

    // ObjectPool<T>'nin tek instance'ını tutuyor.
    private static ObjectPool<T> _objectPool;


    // Static constructor.
    //
    // ObjectPool<T> ilk kez kullanıldığında çalışır.
    //
    // Burada tek bir ObjectPool<T> oluşturulur.
    static ObjectPool()
        => _objectPool = new ObjectPool<T>();


    // Singleton instance'a erişim sağlar.
    //
    // Örneğin:
    //
    //     ObjectPool<X>.GetInstance
    //
    // dediğimizde aynı ObjectPool<X> instance'ı gelir.
    public static ObjectPool<T> GetInstance
    {
        get => _objectPool;
    }


    // ========================================================
    // LOCK OBJECT
    // ========================================================

    // lock mekanizmasında kullanılacak ortak nesne.
    //
    // Aynı anda iki thread'in Get() içerisindeki kritik
    // bölgeye girmesini engellemek için kullanılıyor.
    private static readonly object _o = new();


    // ========================================================
    // INSTANCES
    // ========================================================

    // Havuzdaki nesnelere dışarıdan erişim sağlar.
    public ConcurrentBag<T> Instances
    {
        get => _instances;
    }


    // ========================================================
    // GET
    // ========================================================

    // ObjectPool'dan nesne almak için kullanılır.
    //
    // objectGenerator:
    // Havuzda uygun nesne yoksa yeni nesne oluşturmak
    // için kullanılacak fonksiyondur.
    //
    // Örnek:
    //
    //     pool.Get(() => new X());
    public T? Get(Func<T>? objectGenerator = null)
    {
        // lock ile kritik bölgeyi koruyoruz.
        //
        // Aynı anda iki thread'in aşağıdaki kodu
        // çalıştırmasını engelliyoruz.
        lock (_o)
        {
            // Önce havuzdan bir nesne almaya çalışıyoruz.
            //
            // TryTake:
            //
            // true  -> nesne bulundu.
            // false -> havuz boş.
            var state = _instances.TryTake(out T? instance);


            // Burada iki şartı kontrol ediyoruz:
            //
            // 1. Havuzdan nesne alınamadı.
            //
            // 2. Bu tipten daha önce nesne oluşturulmamış.
            //
            // Amaç:
            //
            // Aynı anda gelen iki thread'in
            // ikisinin de "havuz boş, yeni nesne oluştur"
            // diyerek iki farklı nesne üretmesini engellemek.
            if (!state && !_types.Any(t => t == nameof(T)))
            {
                // Havuz boş ve daha önce bu tipten
                // nesne oluşturulmamış.
                //
                // O zaman yeni nesne oluşturuyoruz.
                T generatedInstance = objectGenerator();


                // Bu tipten nesne oluşturuldu bilgisini
                // listeye ekliyoruz.
                _types.Add(nameof(T));


                // Yeni oluşturduğumuz nesneyi döndürüyoruz.
                return generatedInstance;
            }


            // Eğer yukarıdaki if'e girmediysek
            // burada instance döndürülüyor.
            //
            // Ancak önemli bir problem var:
            //
            // Havuz boşsa ve _types içerisinde T zaten varsa
            // instance NULL olabilir.
            //
            // Yani bu kod null döndürebilir.
            return instance;


            // Özet:
            //
            // Havuzda nesne varsa:
            //     mevcut nesneyi döndür.
            //
            // Havuz boş ve daha önce hiç oluşturulmamışsa:
            //     yeni nesne oluştur.
            //
            // Havuz boş ama daha önce oluşturulmuşsa:
            //     mevcut kod null döndürebilir.
        }
    }


    // ========================================================
    // RETURN
    // ========================================================

    // Kullanımı biten nesneyi tekrar havuza bırakır.
    public void Return(T instance)
    {
        // ConcurrentBag thread-safe olduğu için
        // burada ayrıca lock kullanmaya gerek yoktur.
        _instances.Add(instance);
    }
}


// ============================================================
// X SINIFI
// ============================================================

class X
{
    // X nesnesinin Count değerini tutar.
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


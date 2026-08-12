
// Microsoft.Extensions.ObjectPool kütüphanesini kullanıyoruz.
//
// Bu namespace içerisinde hazır ObjectPool,
// ObjectPoolProvider ve PooledObjectPolicy gibi sınıflar bulunur.
using Microsoft.Extensions.ObjectPool;


// ============================================================
// OBJECT POOL PROVIDER
// ============================================================

// ObjectPool oluşturmak için kullanılan provider'ı oluşturuyoruz.
//
// DefaultObjectPoolProvider, Microsoft'un sunduğu
// varsayılan ObjectPool oluşturucusudur.
DefaultObjectPoolProvider provider = new();


// ============================================================
// OBJECT POOL OLUŞTURMA
// ============================================================

// ObjectPool oluşturuyoruz.
//
// Burada:
//     DefaultPooledObjectPolicy
//
// kullanılıyor.
//
// Bu policy ObjectPool'un X nesnelerini nasıl oluşturacağını,
// hangi nesneleri pool'a geri kabul edeceğini vb. belirler.
ObjectPool<X> pool =
    provider.Create(new DefaultPooledObjectPolicy<X>());


// ============================================================
// 1. NESNEYİ AL
// ============================================================

// Pool'dan bir X nesnesi istiyoruz.
//
// Eğer pool'da kullanılabilir bir X varsa:
//     mevcut nesne döndürülür.
//
// Eğer pool'da nesne yoksa:
//     yeni X oluşturulur.
//
// İlk Get() çağrısında pool boş olduğu için
// büyük ihtimalle new X() çalışacaktır.
var x1 = pool.Get();


// x1 nesnesinin Count değerini 1 artırıyoruz.
x1.Count++;


// Count değerini ekrana yazdırıyoruz.
x1.Write();


// x1 ile işimiz bitti.
//
// Nesneyi Garbage Collector'a bırakmak yerine
// tekrar ObjectPool'a iade ediyoruz.
pool.Return(x1);


// ============================================================
// 2. NESNEYİ AL
// ============================================================

// Pool'dan tekrar X istiyoruz.
//
// Bir önceki satırlarda x1'i pool'a Return ettiğimiz için
// pool'da kullanılabilir bir X bulunması muhtemeldir.
//
// Bu durumda yeni X oluşturulmaz.
//
// x2 aslında x1 ile aynı nesne olabilir.
var x2 = pool.Get();


// Aynı nesnenin Count değerini tekrar artırıyoruz.
x2.Count++;


// Count değerini ekrana yazdırıyoruz.
x2.Write();


// Nesneyi tekrar pool'a bırakıyoruz.
pool.Return(x2);


// ============================================================
// 3. NESNEYİ AL
// ============================================================

// Tekrar pool'dan X istiyoruz.
//
// Pool'da daha önce Return() edilmiş nesne varsa
// o nesne tekrar kullanılacaktır.
var x3 = pool.Get();


// Count değerini tekrar artırıyoruz.
x3.Count++;


// Count değerini ekrana yazdırıyoruz.
x3.Write();


// Kullanımımız bitti.
// Nesneyi tekrar pool'a iade ediyoruz.
pool.Return(x3);


Console.WriteLine();


// ============================================================
// X SINIFI
// ============================================================

class X
{
    
    public int Count { get; set; }


    
    public void Write()
        => Console.WriteLine(Count);


    public X()
        => Console.WriteLine("X üretim maliyeti...");

    
    ~X()
        => Console.WriteLine("X imha maliyeti...");
}


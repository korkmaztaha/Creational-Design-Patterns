while (true)
{
    for (int i = 0; i < 100; i++)
    {
        try
        {
            // A nesnesi Factory üzerinden oluşturulur.
            A? a = ProductCreator.GetInstance(ProductType.A) as A;
            a.Run();

            // B nesnesi Factory üzerinden oluşturulur.
            B? b = ProductCreator.GetInstance(ProductType.B) as B;
            b.Run();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

#region Abstract Product

// Tüm ürünlerin ortak arayüzü.
interface IProduct
{
    void Run();
}

#endregion

#region Concrete Products

// A ürünü
class A : IProduct
{
    public A()
    {
     
        Console.WriteLine($"{nameof(A)} nesnesi üretildi.");
    }

    public void Run()
    {
        throw new NotImplementedException();
    }
}

// B ürünü
class B : IProduct
{
    public B()
    {
        Console.WriteLine($"{nameof(B)} nesnesi üretildi.");
    }

    public void Run()
    {
        throw new NotImplementedException();
    }
}

// C ürünü
class C : IProduct
{
    public C()
    {
        Console.WriteLine($"{nameof(C)} nesnesi üretildi.");
    }

    public void Run()
    {
        throw new NotImplementedException();
    }
}

#endregion
#region Abstract Factory

// Önceki örnekte Factory sınıfı yoktu.
// Burada tüm Factory sınıflarının uygulayacağı ortak arayüz tanımlanıyor.
interface IFactory
{
    // Her Factory kendi ürününü oluşturacak.
    IProduct CreateProduct();
}

#endregion

#region Concrete Factories

// Önceki örnekte ProductCreator içinde new A() yazılıyordu.
// Şimdi A nesnesini oluşturma işi AFactory'ye taşındı.
class AFactory : IFactory
{
    public IProduct CreateProduct()
    {
        return new A();
    }
}

// Önceki örnekte new B() ProductCreator içindeydi.
// Artık BFactory kendi nesnesini kendi oluşturuyor.
class BFactory : IFactory
{
    public IProduct CreateProduct()
    {
        return new B();
    }
}

// Aynı mantık C için de geçerli.
class CFactory : IFactory
{
    public IProduct CreateProduct()
    {
        return new C();
    }
}

#endregion

#region Creator

enum ProductType
{
    A, B, C
}

class ProductCreator
{
    static public IProduct GetInstance(ProductType productType)
    {
        // Önceki örnekte burada switch içinde
        // direkt new A(), new B(), new C() oluşturuluyordu.

        // Bu örnekte ise hangi Factory'nin kullanılacağı seçiliyor.
        IFactory _factory = productType switch
        {
            ProductType.A => new AFactory(),
            ProductType.B => new BFactory(),
            ProductType.C => new CFactory()
        };

        // Önceki örnekten farkı:
        // ProductCreator artık nesne üretmiyor.
        // Üretme işlemini ilgili Factory'ye devrediyor.
        return _factory.CreateProduct();
    }
}

#endregion
while (true)
{
   
    for (int i = 0; i < 100; i++)
    {
        try
        {
            // Factory'den A nesnesi istenir.
            A? a = ProductCreator.GetInstance(ProductType.A) as A;
            a.Run();

            // Factory'den B nesnesi istenir.
            B? b = ProductCreator.GetInstance(ProductType.B) as B;
            b.Run();
        }
        catch (Exception ex)
        {
            // Oluşan hata tekrar fırlatılır.
            throw;
        }
    }
}

#region Abstract Product

// Tüm ürünlerin uygulayacağı ortak sözleşme.
interface IProduct
{
    void Run();
}

#endregion

#region Concrete Products

// A ürünü
class A : IProduct
{
    public void Run()
    {
        throw new NotImplementedException();
    }
}

// B ürünü
class B : IProduct
{
    public void Run()
    {
        throw new NotImplementedException();
    }
}

// C ürünü
class C : IProduct
{
    public void Run()
    {
        throw new NotImplementedException();
    }
}

#endregion

#region Creator

// Hangi ürünün oluşturulacağını belirtir.
enum ProductType
{
    A, B, C
}

class ProductCreator
{
    // Ürün oluşturma işlemi tek merkezden yapılır.
    static public IProduct GetInstance(ProductType productType)
    {
        IProduct _product = null;

        switch (productType)
        {
            case ProductType.A:

                // A nesnesi oluşturulur.
                _product = new A();

                // A'ya özel işlemler yapılabilir.
                // ...

                break;

            case ProductType.B:

                // B nesnesi oluşturulur.
                _product = new B();

                // B'ye özel işlemler yapılabilir.
                // ...

                break;

            case ProductType.C:

                // C nesnesi oluşturulur.
                _product = new C();

                break;
        }

        // Oluşturulan nesne döndürülür.
        return _product;
    }
}

#endregion


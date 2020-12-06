using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfModel.Interfaces
{
    public interface IProduct
    {
        List<Product> GetAllProducts();
        Product GetProductByName(string name);
        Product GetProductById(int id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
    }
}

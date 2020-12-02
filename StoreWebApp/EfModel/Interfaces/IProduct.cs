using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfModel.Interfaces
{
    public interface IProductRepo
    {
        public void AddProduct(Product product);
        public List<Product> GetProducts();
        public Product GetProductById(int productId);
        public Product GetProductByName(string productName);

    }
}

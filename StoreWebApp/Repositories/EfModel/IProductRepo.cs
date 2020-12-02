using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.EfModel
{
    public interface IProductRepo
    {
        public void AddProduct(Product product);
        public ICollection<Product> GetAllProducts();
    }
}

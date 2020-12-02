using Microsoft.EntityFrameworkCore;
using Repositories.EfModel;
using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;
        public ProductRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        //Add Product
        public void AddProduct(Product product)
        {
            using var context = new project0Context(_contextOptions);
            var _product = new Product()
            {
                ProductName = product.ProductName,
                Price = product.Price
            };
            context.Products.Add(_product);
            context.SaveChanges();
        }

        //Get all Products
        public ICollection<Product> GetAllProducts()
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products.ToList();
            return dbProduct;
        }
        //Get Product By Name
        public Product GetProductByName(string productName)
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products
                .Where(p => p.ProductName == productName)
                .FirstOrDefault();
            var _product = new Product()
            {
                ProductId = dbProduct.ProductId,
                ProductName = dbProduct.ProductName,
                Price = dbProduct.Price
            };
            return _product;
        }
        //Update Product
        public void UpdateProduct(Product product)
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();
            dbProduct.ProductName = product.ProductName;
            dbProduct.Price = product.Price;
            context.SaveChanges();
        }
    }
}

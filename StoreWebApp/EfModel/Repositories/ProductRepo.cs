using EfModel.EfModel;
using EfModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoreLibrary;


namespace EfModel.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;
        public ProductRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public void AddProduct(StoreLibrary.Product product)
        {
            using var context = new project0Context(_contextOptions);
            var _product = new EfModel.Product()
            {
                ProductName = product.ProductName,
                Price = product.Price
            };
            context.Products.Add(_product);
            context.SaveChanges();
        }

        public StoreLibrary.Product GetProductById(int productId)
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products
                .Where(p => p.ProductId == productId)
                .FirstOrDefault();
            var product = new StoreLibrary.Product()
            {
                ProductId = dbProduct.ProductId,
                ProductName = dbProduct.ProductName,
                Price = dbProduct.Price
            };
            return product;
        }

        public StoreLibrary.Product GetProductByName(string productName)
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products
                .Where(p => p.ProductName == productName)
                .FirstOrDefault();
            var _product = new StoreLibrary.Product()
            {
                ProductId = dbProduct.ProductId,
                ProductName = dbProduct.ProductName,
                Price = dbProduct.Price
            };
            return _product;
        }

        public List<StoreLibrary.Product> GetProducts()
        {
            using var context = new project0Context(_contextOptions);
            var dbProducts = context.Products.Distinct().ToList();
            var result = new List<StoreLibrary.Product>();
            foreach (var prod in dbProducts)
            {
                var product = new StoreLibrary.Product()
                {
                    ProductId = prod.ProductId,
                    ProductName = prod.ProductName,
                    Price = prod.Price
                };
                result.Add(product);
            };
            return result;
        }
    }
}

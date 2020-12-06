using EfModel.Interfaces;
using EfModel.Models;
using Microsoft.EntityFrameworkCore;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Product = StoreLibrary.Product;

namespace EfModel.Repositories
{
    public class ProductRepo : IProduct
    {
        private readonly DbContextOptions<project0Context> _contextOptions;

        public ProductRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public void CreateProduct(Product product)
        {
            using var context = new project0Context(_contextOptions);
            var newEntry = new Models.Product()
            {
                ProductName = product.ProductName,
                Price = product.Price
            };
            context.Products.Add(newEntry);
            context.SaveChanges();
        }

        public List<Product> GetAllProducts()
        {
            using var context = new project0Context(_contextOptions);
            var dbProducts = context.Products.Distinct().ToList();
            var result = new List<Product>();
            foreach (var product in dbProducts)
            {
                var newProduct = new Product()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price
                };
                result.Add(newProduct);
            };
            return result;
        }

        public Product GetProductById(int id)
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products
                .Where(a => a.ProductId == id)
                .FirstOrDefault();
            if (dbProduct == null)
            {
                return null;
            }
            else
            {
                var newProduct = new Product()
                {
                    ProductId = dbProduct.ProductId,
                    ProductName = dbProduct.ProductName,
                    Price = dbProduct.Price
                };
                return newProduct;
            }
        }

        public Product GetProductByName(string name)
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products
                .Where(a => a.ProductName == name)
                .FirstOrDefault();
            if (dbProduct == null)
            {
                return null;
            }
            else
            {
                var newProduct = new Product()
                {
                    ProductId = dbProduct.ProductId,
                    ProductName = dbProduct.ProductName,
                    Price = dbProduct.Price
                };
                return newProduct;
            }
        }

        public void UpdateProduct(Product product)
        {
            using var context = new project0Context(_contextOptions);
            var dbProduct = context.Products
                .Where(a => a.ProductId == product.ProductId)
                .FirstOrDefault();
            dbProduct.ProductName = product.ProductName;
            dbProduct.Price = product.Price;
            context.SaveChanges();
        }
    }
}

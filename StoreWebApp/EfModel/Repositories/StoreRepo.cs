using EfModel.Interfaces;
using EfModel.Models;
using Microsoft.EntityFrameworkCore;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inventory = StoreLibrary.Inventory;
using Product = StoreLibrary.Product;
using Store = StoreLibrary.Store;

namespace EfModel.Repositories
{
    public class StoreRepo : IStore
    {
        private readonly DbContextOptions<project0Context> _context;

        public StoreRepo(DbContextOptions<project0Context> context)
        {
            _context = context;
        }
        public void CreateInventory(Store store, Product product, int stock)
        {
            using var context = new project0Context(_context);
            var currentLocation = store;
            var currentProduct = product;
            var newEntry = new Models.Inventory()
            {
                StoreId = currentLocation.StoreId,
                ProductId = currentProduct.ProductId,
                Stock = stock
            };
            context.Inventories.Add(newEntry);
            context.SaveChanges();
        }

        public void CreateStore(Store location)
        {
            using var context = new project0Context(_context);
            var newEntry = new Models.Store()
            {
                StoreName = location.StoreName
            };
            context.Stores.Add(newEntry);
            context.SaveChanges();
        }

        public void DeleteStore(Store store)
        {
            using var context = new project0Context(_context);
            var dbLocation = context.Stores
                .Where(i => i.StoreId == store.StoreId)
                .FirstOrDefault();
            context.Remove(dbLocation);
            context.SaveChanges();
        }

        public List<Store> GetAllStore()
        {
            using var context = new project0Context(_context);
            var dbLocations = context.Stores.Distinct().ToList();
            var result = new List<Store>();
            foreach (var location in dbLocations)
            {
                var newLocation = new Store()
                {
                    StoreId = location.StoreId,
                    StoreName = location.StoreName
                };
                result.Add(newLocation);
            };
            return result;
        }

        public Inventory GetInventoryById(int? id)
        {
            using var context = new project0Context(_context);
            var dbInventory = context.Inventories
                .Where(i => i.InventoryId == id)
                .Include(i => i.Product)
                .FirstOrDefault();
            if (dbInventory == null)
            {
                return null;
            }
            else
            {
                var result = new Inventory()
                {
                    InventoryId = dbInventory.InventoryId,
                    StoreId = dbInventory.StoreId,
                    ProductName = dbInventory.Product.ProductName,
                    Stock = dbInventory.Stock
                };
                return result;
            }
        }

        public List<Inventory> GetInventoryByStore(Store store)
        {
            using var context = new project0Context(_context);
            var dbInventory = context.Inventories
                .Where(i => i.StoreId == store.StoreId)
                .Include(i => i.Product)
                .ToList();
            var result = new List<Inventory>();
            foreach (var item in dbInventory)
            {
                var newItem = new Inventory(item.StoreId, item.Product.ProductName, item.Stock)
                {
                    InventoryId = item.InventoryId
                };
                result.Add(newItem);
            }
            return result;
        }

        public Store GetLocationByName(string storeName)
        {
            using var context = new project0Context(_context);
            var dbLocation = context.Stores
                .FirstOrDefault(l => l.StoreName == storeName);
            if (dbLocation != null)
            {
                var result = new Store()
                {
                    StoreId = dbLocation.StoreId,
                    StoreName = dbLocation.StoreName
                };
                var resultInv = GetInventoryByStore(result);
                foreach (var thing in resultInv)
                {
                    result.Inventories.Add(thing);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public Store GetStoreById(int storeId)
        {
            using var context = new project0Context(_context);
            var dbLocation = context.Stores
                .Where(l => l.StoreId == storeId)
                .FirstOrDefault();
            if (dbLocation == null)
            {
                return null;
            }
            else
            {
                var result = new Store()
                {
                    StoreId = dbLocation.StoreId,
                    StoreName = dbLocation.StoreName
                };
                var resultInv = GetInventoryByStore(result);
                foreach (var thing in resultInv)
                {
                    result.Inventories.Add(thing);
                }
                return result;
            }
        }

        public void UpdateInventory(int storeId, string productName, int stock)
        {
            using var context = new project0Context(_context);
            var dbInventory = context.Inventories
                .Include(i => i.Product)
                .Where(i => i.StoreId == storeId && i.Product.ProductName == productName)
                .FirstOrDefault();
            dbInventory.Stock = stock;
            context.SaveChanges();
        }

        public void UpdateStore(Store store)
        {
            using var context = new project0Context(_context);
            var dbLocation = context.Stores
                .Where(i => i.StoreId == store.StoreId)
                .FirstOrDefault();
            dbLocation.StoreName = store.StoreName;
            context.SaveChanges();
        }
    }
}

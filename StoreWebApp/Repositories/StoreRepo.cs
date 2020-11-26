using Microsoft.EntityFrameworkCore;
using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Repositories
{
    public class StoreRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;

        public StoreRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        //Get All Stores
        public List<Store> GetAllStores()
        {
            using var context = new project0Context(_contextOptions);
            var dbStore = context.Stores.ToList();
            var result = new List<Store>();
            foreach(var store in dbStore)
            {
                var _store = new Store()
                {
                    StoreId = store.StoreId,
                    StoreName = store.StoreName
                };
                result.Add(_store);
            }
            return result;
        }
        //Get Store by Name
        public Store GetStoreByName(string storeName)
        {
            using var context = new project0Context(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(s => s.StoreName == storeName);
            var result = new Store()
            {
                StoreId = dbStore.StoreId,
                StoreName = dbStore.StoreName
            };
            result.StoreId = dbStore.StoreId;
            var resultInventory = GetInventories(result);
            foreach (var item in resultInventory)
            {
                result.Inventories.Add(item);
            }
            return result;
        }
        //Get Store Inventory 
        public List<Inventory> GetInventories(Store store)
        {
            using var context = new project0Context(_contextOptions);
            var dbInventory = context.Inventories
                .Where(i => i.StoreId == store.StoreId)
                .Include(i => i.Product)
                .ToList();
            var result = new List<Inventory>();
            foreach(var item in dbInventory)
            {
                var _item = new Inventory(item.StoreName,item.StoreId, item.ProductId, item.ProductName, item.Stock);
                _item.InventoryId = item.InventoryId;

            }
            return result;
        }
        //Add Store
        public void AddStore(Store store)
        {
            using var context = new project0Context(_contextOptions);
            var _store = new Store()
            {
                StoreName = store.StoreName
            };
            context.Stores.Add(_store);
            context.SaveChanges();
        }
        //Add Inventory
        public void AddInventory(Store store, Product product, int stock)
        {
            using var context = new project0Context(_contextOptions);
            var currentStore = store;
            var currentProduct = product;
            var _inventory = new Inventory()
            {
                StoreId = currentStore.StoreId,
                ProductId = currentProduct.ProductId,
                Stock = stock
            };
            context.Inventories.Add(_inventory);
            context.SaveChanges();
        }
        //Update Store
        public void UpdateStore(Store store)
        {
            using var context = new project0Context(_contextOptions);
            var dbStore = context.Stores
                .Where(s => s.StoreId == store.StoreId)
                .FirstOrDefault();
            dbStore.StoreName = store.StoreName;
            context.SaveChanges();
        }
        //Update Inventory
        public void UpdateInventory(Store store, Product product, int stock)
        {
            using var context = new project0Context(_contextOptions);
            var currentStore = GetStoreByName(store.StoreName);
            var dbInventory = context.Inventories
                .Include(i => i.Product)
                .Where(i => i.StoreId == currentStore.StoreId && i.ProductId == product.ProductId)
                .FirstOrDefault();
            dbInventory.Stock += stock;
            context.SaveChanges();
        }
    }
}

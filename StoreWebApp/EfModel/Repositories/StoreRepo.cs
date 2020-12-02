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
    public class StoreRepo : IStoreRepo
    {
        private readonly DbContextOptions<project0Context> _contextOptions;

        public StoreRepo(DbContextOptions<project0Context> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public void AddStore(StoreLibrary.Store store)
        {
            using var context = new project0Context(_contextOptions);
            var _store = new EfModel.Store()
            {
                StoreName = store.StoreName
            };
            context.Stores.Add(_store);
            context.SaveChanges();
        }

        public List<StoreLibrary.Store> GetAllStores()
        {
            using var context = new project0Context(_contextOptions);
            var dbStore = context.Stores.ToList();
            var result = new List<StoreLibrary.Store>();
            foreach (var store in dbStore)
            {
                var _store = new StoreLibrary.Store()
                {
                    StoreId = store.StoreId,
                    StoreName = store.StoreName
                };
                result.Add(_store);
            }
            return result;
        }

        public StoreLibrary.Store GetStoreById(int storeId)
        {
            using var context = new project0Context(_contextOptions);
            var dbStore = context.Stores
                .Where(s => s.StoreId == storeId)
                .FirstOrDefault();
            var result = new StoreLibrary.Store()
            {
                StoreId = dbStore.StoreId,
                StoreName = dbStore.StoreName
            };
            result.StoreId = dbStore.StoreId;
            var inventory = GetInventoriesByStore(result);
            foreach (var item in inventory)
            {
                result.Inventories.Add(item);
            }
            return result;
        }

        public StoreLibrary.Store GetStoreByName(string storeName)
        {
            using var context = new project0Context(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(s => s.StoreName == storeName);
            var result = new StoreLibrary.Store()
            {
                StoreId = dbStore.StoreId,
                StoreName = dbStore.StoreName
            };
            result.StoreId = dbStore.StoreId;
            var resultInventory = GetInventoriesByStore(result);
            foreach (var item in resultInventory)
            {
                result.Inventories.Add(item);
            }
            return result;
        }

       
        //Inventories
        public void AddInventories(StoreLibrary.Store store, StoreLibrary.Product product, int stock)
        {
            using var context = new project0Context(_contextOptions);
            var currentStore = store;
            var currentProduct = product;
            var _inventory = new EfModel.Inventory()
            {
                StoreId = currentStore.StoreId,
                ProductId = currentProduct.ProductId,
                Stock = stock
            };
            context.Inventories.Add(_inventory);
            context.SaveChanges();
        }

        public List<StoreLibrary.Inventory> GetInventoriesByStore(StoreLibrary.Store store)
        {
            using var context = new project0Context(_contextOptions);
            var dbInventory = context.Inventories
                .Where(i => i.StoreId == store.StoreId)
                .Include(i => i.Product)
                .ToList();
            var result = new List<StoreLibrary.Inventory>();
            foreach (var item in dbInventory)
            {
                var _item = new StoreLibrary.Inventory(item.StoreName, item.StoreId, item.ProductId, item.ProductName, item.Stock);
                _item.InventoryId = item.InventoryId;

            }
            return result;
        }

        public StoreLibrary.Inventory GetInventoryById(int inventoryId)
        {
            using var context = new project0Context(_contextOptions);
            var dbInventory = context.Inventories
                .Where(i => i.InventoryId == inventoryId)
                .Include(i => i.Product)
                .FirstOrDefault();
            var result = new StoreLibrary.Inventory()
            {
                InventoryId = dbInventory.InventoryId,
                StoreId = dbInventory.StoreId,
                StoreName = dbInventory.StoreName,
                ProductName = dbInventory.ProductName,
                Stock = dbInventory.Stock
            };
            return result;
        }
    }
}

using EfModel.EfModel;
using System;
using System.Collections.Generic;
using System.Text;
using StoreLibrary;

namespace EfModel.Interfaces
{
    public interface IStoreRepo
    {
        public void AddStore(StoreLibrary.Store store);
        public List<StoreLibrary.Store> GetAllStores();
        public StoreLibrary.Store GetStoreById(int storeId);
        public StoreLibrary.Store GetStoreByName(string storeName);

        //Inventories
        public void AddInventories(StoreLibrary.Store store, StoreLibrary.Product product, int stock);
        public List<StoreLibrary.Inventory> GetInventoriesByStore(StoreLibrary.Store store);

        public List<StoreLibrary.Inventory> GetAllInventories();
        public StoreLibrary.Inventory GetInventoryById(int id);

    }
}

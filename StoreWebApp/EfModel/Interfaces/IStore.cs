using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfModel.Interfaces
{
    public interface IStore
    {
        List<Store> GetAllStore();
        Store GetLocationByName(string storeName);
        Store GetStoreById(int storeId);
        List<Inventory> GetInventoryByStore(Store store);
        Inventory GetInventoryById(int? id);
        void CreateInventory(Store store, Product product, int stock);
        void CreateStore(Store location);
        public void UpdateInventory(int storeId, string productName, int stock);
        public void UpdateStore(Store store);
        public void DeleteStore(Store store);
    }
}

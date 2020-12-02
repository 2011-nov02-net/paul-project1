using StoreWebApp.EfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.EfModel
{
    public interface IStoreRepo
    {
        List<Store> GetAllStores();
        Store GetAllStoresById(int storeId);

        List<Inventory> GetInventories(Store store);
        public void AddInventory(Store store, Product product, int stock);

        public void AddStore(Store store);
    }
}

using StoreLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models
{
    public class StoreViewModel
    {
        public StoreViewModel()
        {
        }
        public StoreViewModel(Store store)
        {
            StoreId = store.StoreId;
            StoreName = store.StoreName;
            Inventory = store.Inventory;
        }
        public int StoreId { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string StoreName { get; set; }
        public List<Inventory> Inventory { get; set; }
    }
}

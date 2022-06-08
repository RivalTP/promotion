using System.ComponentModel.DataAnnotations;

namespace Promotion.Models
{
    public class Store
    {
        [Key]
        public int storeID { get; set; }

        public string storeName { get; set; }
    }
}

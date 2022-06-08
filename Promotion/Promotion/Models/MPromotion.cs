using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promotion.Models
{
    public class MPromotion
    {
        [Display(Name = "Promo ID")]
        public string promoID { get; set; }
        [Display(Name = "Promo Type")]
        public string promoType { get; set; }
        [Display(Name = "Value Type")]
        public string valueType { get; set; }

        public int valueTotal { get; set; }
        [Display(Name = "Promo Description")]
        public string promoDescription { get; set; }
        [Display(Name = "Promo Duration Start Date")]
        public DateTime promoStartDate { get; set; }
        [Display(Name = "Promo Duration End Date")]
        public DateTime promoEndDate { get; set; }
        [Display(Name = "Item")]
        public string itemUpload { get; set; }
        [Display(Name = "Store")]
        public IEnumerable<Store> Stores { get; set; }



    }
}

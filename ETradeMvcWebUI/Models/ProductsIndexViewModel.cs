using ETradeBusiness.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace ETradeMvcWebUI.Models
{
    public class ProductsIndexViewModel
    {
        public List<ProductModel> Products { get; set; }

        public string Name { get; set; }

        [DisplayName("Category")]
        public SelectList Categories { get; set; }

        public int? CategoryId { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPriceMin { get; set; }

        public string UnitPriceMax { get; set; }

        [DisplayName("Create Date")]
        public string CreateDateMin { get; set; }

        public string CreateDateMax { get; set; }

        public SelectList Pages { get; set; }

        [DisplayName("Page")] 
        public int PageNo { get; set; } = 1;

        public SelectList Order { get; set; }

        [DisplayName("Order")]
        public string OrderBy { get; set; } = "name asc";
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace ETradeEntities.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public double UnitPrice { get; set; }

        public int? StockAmount { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(255)]
        public string ImagePath { get; set; }
    }
}

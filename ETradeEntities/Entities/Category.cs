using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETradeEntities.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required] 
        [StringLength(100)] 
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
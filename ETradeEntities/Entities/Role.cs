using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETradeEntities.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETradeEntities.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        //[Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required]
        [StringLength(25)]
        public string Password { get; set; }
        
        [StringLength(300)]
        public string EMail { get; set; }

        public DateTime? BirthDay { get; set; }

        public bool Active { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

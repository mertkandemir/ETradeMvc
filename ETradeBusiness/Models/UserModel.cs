using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETradeBusiness.Models
{
    public class UserModel
    {
        #region Entity özellikleri
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} can not be empty!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} can not be empty!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(25, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Password { get; set; }

        [StringLength(300, ErrorMessage = "{0} must be maximum {1} characters!")]
        [EmailAddress(ErrorMessage = "{0} format is not valid!")]
        [DisplayName("E-Mail")]
        public string EMail { get; set; }

        public DateTime? BirthDay { get; set; }

        public bool Active { get; set; }

        public int RoleId { get; set; }
        #endregion

        #region Uygulamada ihtiyacımız olabilecek özellikler
        public RoleModel Role { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Birth Day")]
        public string BirthDayText { get; set; }
        #endregion
    }
}

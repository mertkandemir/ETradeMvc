using System.ComponentModel.DataAnnotations;

namespace ETradeBusiness.Models
{
    public class CategoryModel
    {
        #region Entity özellikleri
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
        #endregion

        #region Uygulamada ihtiyacımız olabilecek özellikler
        public int ProductCount { get; set; }
        #endregion
    }
}

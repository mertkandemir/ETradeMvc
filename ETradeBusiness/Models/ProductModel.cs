using AppCore.Business.Validations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETradeBusiness.Models
{
    public class ProductModel
    {
        #region Entity özellikleri
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be less than {1} characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Unit Price")]
        [MinValue(0, ErrorMessage = "{0} must be minimum {1}!")]
        public double UnitPrice { get; set; }

        [DisplayName("Stock Amount")]
        [Range(0, Int32.MaxValue, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int? StockAmount { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required!")]
        public int CategoryId { get; set; }

        [DisplayName("Create Date")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Update Date")]
        public DateTime? UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(255)]
        [DisplayName("Image")]
        public string ImagePath { get; set; }
        #endregion

        #region Uygulamada ihtiyacımız olabilecek özellikler
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Create Date")]
        public string CreateDateText { get; set; }

        [DisplayName("Update Date")] 
        public string UpdateDateText { get; set; }

        public string StockAmountText { get; set; }
        #endregion
    }
}

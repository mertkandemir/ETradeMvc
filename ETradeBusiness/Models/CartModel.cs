using System.ComponentModel;

namespace ETradeBusiness.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Unit Price")]
        public double UnitPrice { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }
    }
}

using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugShoppingCartMvcUI.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int DrugId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }   
        public Drug Drug { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}

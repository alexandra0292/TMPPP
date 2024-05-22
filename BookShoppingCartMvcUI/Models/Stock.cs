using System.ComponentModel.DataAnnotations.Schema;

namespace DrugShoppingCartMvcUI.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int Quantity { get; set; }

        public Drug? Drug { get; set; }
    }
}

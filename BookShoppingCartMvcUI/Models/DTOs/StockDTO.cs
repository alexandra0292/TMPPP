using System.ComponentModel.DataAnnotations;

namespace DrugShoppingCartMvcUI.Models.DTOs
{
    public class StockDTO
    {
        public int DrugId { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; }
    }
}

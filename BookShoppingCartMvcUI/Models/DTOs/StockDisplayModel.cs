namespace DrugShoppingCartMvcUI.Models.DTOs
{
    public class StockDisplayModel
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int Quantity { get; set; }
        public string? DrugName { get; set; }
    }
}

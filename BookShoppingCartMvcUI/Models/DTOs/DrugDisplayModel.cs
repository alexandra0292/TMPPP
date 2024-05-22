namespace DrugShoppingCartMvcUI.Models.DTOs
{
    public class DrugDisplayModel
    {
        public IEnumerable<Drug> Drugs { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public string STerm { get; set; } = "";
        public int CategoryId { get; set; } = 0;
    }
}

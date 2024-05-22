using System.ComponentModel.DataAnnotations;

namespace DrugShoppingCartMvcUI.Models.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }
    }
}

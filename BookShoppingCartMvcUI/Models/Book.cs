using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugShoppingCartMvcUI.Models
{
    [Table("Drug")]
    public class Drug
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? DrugName { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Producer { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        public Stock Stock { get; set; }

        [NotMapped]
        public string GenreName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }


    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DrugShoppingCartMvcUI.Models.DTOs;
public class DrugDTO
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
    public IFormFile? ImageFile { get; set; }
    public IEnumerable<SelectListItem>? GenreList { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Bean_API.Dtos
{
    public class CreateCoffeeBeanDto
    {
        [Required(ErrorMessage = "ID cannot be null")]
        public required string Id { get; set; }

        public int IndexNum { get; set; }

        public ulong IsBotd { get; set; }

        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Image must be provided")]
        public required string Image { get; set; }

        public int? ColourId { get; set; }

        [Required(ErrorMessage = "Must provide a name")]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public int? CountryId { get; set; }
    }
}

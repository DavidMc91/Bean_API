using System.ComponentModel.DataAnnotations;

namespace Bean_API.Dtos
{
    public class ResponseCoffeeBeanDto
    {
        [Required(ErrorMessage = "ID cannot be null")]
        public required string Id { get; set; }

        public int IndexNum { get; set; }

        public bool IsBotd { get; set; }

        public decimal Cost { get; set; }

        public string? Image { get; set; }

        public string? ColourName { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? CountryName { get; set; }

    }
}

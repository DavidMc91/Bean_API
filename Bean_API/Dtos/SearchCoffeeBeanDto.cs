namespace Bean_API.Dtos
{
    public class SearchCoffeeBeanDto
    {
        public string? Name { get; set; }
        public int? ColourId { get; set; }
        public int? CountryId { get; set; }
        public decimal? MaxCost { get; set; }
        public bool? IsBotd { get; set; }
    }
}

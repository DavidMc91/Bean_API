using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bean_API.Models;

public partial class Coffeebean
{
    public string Id { get; set; } = null!;

    public int IndexNum { get; set; }

    public ulong IsBotd { get; set; }

    public decimal Cost { get; set; }

    public string Image { get; set; } = null!;

    public int? ColourId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? CountryId { get; set; }

    public virtual ICollection<Coffeebeanoftheday> Coffeebeanofthedays { get; set; } = new List<Coffeebeanoftheday>();

    public virtual Colour? Colour { get; set; }

    public virtual Country? Country { get; set; }
}

using System;
using System.Collections.Generic;

namespace Bean_API.Models;

public partial class Colour
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Coffeebean> Coffeebeans { get; set; } = new List<Coffeebean>();
}

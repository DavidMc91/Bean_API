using System;
using System.Collections.Generic;

namespace Bean_API.Models;

public partial class Coffeebeanoftheday
{
    public int Id { get; set; }

    public string CoffeeBeanId { get; set; } = null!;

    public DateOnly BotdDate { get; set; }

    public virtual Coffeebean CoffeeBean { get; set; } = null!;
}

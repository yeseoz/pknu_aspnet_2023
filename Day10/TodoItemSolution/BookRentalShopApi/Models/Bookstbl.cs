﻿using System;
using System.Collections.Generic;

namespace BookRentalShopApi.Models;

public partial class Bookstbl
{
    public int BookIdx { get; set; }

    public string Author { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string Names { get; set; } = null!;

    public DateTime? ReleaseDate { get; set; }

    public string? Isbn { get; set; }

    public decimal? Price { get; set; }

    public virtual Divtbl DivisionNavigation { get; set; } = null!;

    public virtual ICollection<Rentaltbl> Rentaltbls { get; set; } = new List<Rentaltbl>();
}

﻿using System;
using System.Collections.Generic;

namespace BookRentalShopApi.Models;

public partial class Usertbl
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;
}

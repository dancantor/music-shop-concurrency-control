using System;
using System.Collections.Generic;

namespace MusicShop.DataAccess.Entity;

public partial class Employee
{
    public int Id { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? Position { get; set; }

    public string? Name { get; set; }
}

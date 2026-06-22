using System;
using System.Collections.Generic;

namespace Library.DAL.Models;

public partial class Librarian
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }
}

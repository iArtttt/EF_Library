using System;
using System.Collections.Generic;

namespace Library.DAL.Models;

public partial class Reader
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int DocumentTypeId { get; set; }

    public string DocumentNumber { get; set; } = null!;

    public virtual DocumentType DocumentType { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Library.DAL.Models;

public partial class DocumentType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();
}

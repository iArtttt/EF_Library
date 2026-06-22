using System;
using System.Collections.Generic;

namespace Library.DAL.Models;

public partial class PublusherType
{
    public int Id { get; set; }

    public string Publusher { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

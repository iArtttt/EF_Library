using System;
using System.Collections.Generic;

namespace Library.DAL.Models;

public partial class Autor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? SecondName { get; set; }

    public DateTime Birthday { get; set; }

    public virtual ICollection<AutorsBook> AutorsBooks { get; set; } = new List<AutorsBook>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

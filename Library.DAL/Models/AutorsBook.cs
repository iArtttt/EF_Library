using System;
using System.Collections.Generic;

namespace Library.DAL.Models;

public partial class AutorsBook
{
    public int Id { get; set; }

    public int? AutorId { get; set; }

    public int? BookId { get; set; }

    public virtual Autor? Autor { get; set; }

    public virtual Book? Book { get; set; }
}

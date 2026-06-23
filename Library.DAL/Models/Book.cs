using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [StringLength(400)]
    public string? Genre { get; set; }

    public int AutorId { get; set; }

    public string? PublishCode { get; set; }

    public int CodeTypeId { get; set; }

    public DateTime? Year { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public virtual Autor Autor { get; set; } = null!;

    public virtual ICollection<AutorsBook> AutorsBooks { get; set; } = new List<AutorsBook>();

    public virtual PublusherType CodeType { get; set; } = null!;
}

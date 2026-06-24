using Library.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Shared.Interfaces
{
    public interface IDocument : IID
    {
        public string DocumentNumber { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}

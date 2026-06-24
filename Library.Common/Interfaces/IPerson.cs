using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Shared.Interfaces
{
    public interface IPerson : IID
    {
        public string Name { get; set; }
        public string LastName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Backend.Models
{
    public enum TodoTaskStatus
    {
        Undefined = 0,
        Reserved = 1,
        Ongoing = 2,
        Done = 3,
        Pending = 4

    }
}

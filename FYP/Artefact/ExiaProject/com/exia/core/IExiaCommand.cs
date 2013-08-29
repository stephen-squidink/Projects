using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExiaProject.com.exia.core
{
    interface IExiaCommand
    {
        void execute(PhysicsObject pObj);
    }
}

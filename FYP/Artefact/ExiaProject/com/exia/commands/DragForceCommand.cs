using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.util;

namespace ExiaProject.com.exia.commands
{
    class DragForceCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float Fdrag = 0;
            float CDrag = 0;

            CDrag = 0.5f * pObj._spec.DragCoefficient * pObj._spec.FrontalArea * PhysicsConstants.rho;

            Fdrag = -CDrag * pObj._velocity.Z * pObj._velocity.Z;
            
            pObj._cDrag = CDrag;
            pObj._dragForce = Fdrag;
        }
    }
}

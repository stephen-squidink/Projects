using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.util;

namespace ExiaProject.com.exia.commands
{
    class BrakeForceCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float Fb = 0;

            if (pObj._velocity.Z > 0)
                Fb = ((PhysicsConstants.cof * PhysicsConstants.g * pObj._spec.CurbWeight))* -pObj._brakingPosition;

            pObj._brakingForce = Fb;
        }
    }
}

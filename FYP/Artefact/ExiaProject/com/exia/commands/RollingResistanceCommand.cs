using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.util;

namespace ExiaProject.com.exia.commands
{
    class RollingResistanceCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float rr = 0;
            float Frr = 0;

            if(pObj._velocity.Z >0)
                rr = 1.0f;

            Frr = -rr * PhysicsConstants.rcoef * (pObj._spec.CurbWeight) * PhysicsConstants.g;

            pObj._rollingForce = Frr;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.util;

namespace ExiaProject.com.exia.commands
{
    class AngularAccelerationCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float angularAccel = 0;
            float b = (pObj._spec.WeightDistributionFront / 100) * (UnitConverter.MMtoM(pObj._spec.WheelBase));
            float c = (pObj._spec.WeightDistributionRear / 100) * (UnitConverter.MMtoM(pObj._spec.WheelBase));

            float bodyTorque = b * pObj._flatF.X - c * pObj._flatR.X;

            angularAccel = bodyTorque / pObj._spec.CurbWeight;

            if (pObj._velocity.Z <= 0)
                angularAccel = 0;

            pObj._angularAcceleration = angularAccel;
        }
    }
}

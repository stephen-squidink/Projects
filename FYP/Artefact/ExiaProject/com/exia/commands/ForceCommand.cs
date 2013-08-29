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
    class ForceCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            Vector3 netForce = new Vector3();

            netForce.Z = (pObj._tractionForce + pObj._brakingForce) + pObj._rollingForce + pObj._dragForce;
            netForce.X = ((float)Math.Cos(pObj._steeringAngle)) * (pObj._flatF.X + pObj._flatR.X);

            if ((netForce.Z < 0 && pObj._velocity.X != 0) || (pObj._velocity.Z <= 0 && pObj._velocity.X != 0) || (pObj._brakingPosition > 0 && pObj._velocity.X != 0) || (pObj._steeringPosition == 0 && pObj._velocity.X != 0))
            {
                if (pObj._velocity.X < 0)
                {
                    netForce.X -= (pObj._rollingForce + pObj._dragForce + (pObj._brakingForce/10)) * 20;
                }

                if (pObj._velocity.X > 0)
                {
                    netForce.X += (pObj._rollingForce + pObj._dragForce + (pObj._brakingForce/10)) * 20;
                }

                if (pObj._velocity.X < 2 && pObj._velocity.X > -2)
                {
                    pObj._velocity.X = 0;
                }
            }

            if (pObj._currentRPM > 6000)
            {
                if (netForce.Z > 0)
                    netForce.Z = 0;
            }

            pObj._force = netForce;
        }
    }
}

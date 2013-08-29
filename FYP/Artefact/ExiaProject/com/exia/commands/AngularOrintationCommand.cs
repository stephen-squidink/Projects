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
    class AngularOrintationCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float factor = 1;

            if (pObj._currentGear == "Reverse")
            {
                factor = -1;
            }

            pObj._angle = 0;

            pObj._steeringAngle = pObj._steeringPosition * MathHelper.Pi / 6.5f;

            pObj._angle += pObj._delta * pObj._angularVelocity * factor;

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), pObj._angle);
            pObj._rotation *= additionalRot;
        }
    }
}

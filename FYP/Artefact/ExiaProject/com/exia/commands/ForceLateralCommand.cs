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
    class ForceLateralCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float sn = (float)Math.Sin(pObj._angle);
            float cs = (float)Math.Cos(pObj._angle);

            Vector3 velocity = new Vector3();

            velocity.X = (float)(cs * pObj._velocity.X + sn * pObj._velocity.Z);
            velocity.Y = (float)(-sn * pObj._velocity.X + cs * pObj._velocity.Z);

            float yawspeed = pObj._spec.WheelBase * 0.5f * pObj._angularVelocity;

            float rot_angle = (float)Math.Atan2(yawspeed, velocity.Z);
            float sideslip = (float)Math.Atan2(velocity.X, velocity.Z);

            if (velocity.Z == 0)
            {
                rot_angle = 0;
                sideslip = 0;
            }

            float slipangleFront = sideslip + rot_angle - pObj._steeringAngle;
            float slipangleRear = sideslip - rot_angle;

            Vector3 flatf = new Vector3();
            Vector3 flatr = new Vector3();

            flatf.Z = 0;
            flatf.X = (float)(-5.0f * slipangleFront);
            flatf.X = (float)Math.Min(2.0f, flatf.X);
            flatf.X = (float)Math.Max(-2.0f, flatf.X);
            flatf.X *= pObj._spec.CurbWeight * 0.5f * PhysicsConstants.g;

            flatr.Z = 0;
            flatr.X = (float)(-5.20f * slipangleRear);
            flatr.X = (float)Math.Min(2.0f, flatr.X);
            flatr.X = (float)Math.Max(-2.0f, flatr.X);
            flatr.X *= pObj._spec.CurbWeight * 0.5f * PhysicsConstants.g;

            pObj._flatF = flatf;
            pObj._flatR = flatr;
        }
    }
}

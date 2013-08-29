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
    class DisplacementCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            Vector3 v = pObj._velocity;
            Quaternion r = pObj._rotation;

            if (pObj._currentGear == "Reverse")
                v *= -1;
         
            pObj._speed = Vector3.Transform(-v, r);
            pObj._position += pObj._delta * pObj._speed;
        }
    }
}

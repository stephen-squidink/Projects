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
    class AngularVelocityCommand : IExiaCommand
    {
        public void execute(PhysicsObject pObj)
        {
            float angularVel = 0;

            angularVel = pObj._delta * pObj._angularAcceleration;
            
            pObj._angularVelocity += angularVel;
            pObj._angularVelocity *= 0.8f;
        }
    }
}

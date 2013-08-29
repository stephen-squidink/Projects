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

namespace ExiaProject.com.game.camera
{
    class StationaryCamera : Camera
    {
        public StationaryCamera(Vector3 position, Viewport viewport, Vector2 rotAngles) : base(position, viewport, rotAngles)
        {
            _cameraPosition = position;
        }

        public override void initialise()
        {
            UpdateViewMatrix();

            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _viewport.AspectRatio, 0.3f, 1000.0f);
        }

        public override void update(GameTime gameTime)
        {

        }
    }
}

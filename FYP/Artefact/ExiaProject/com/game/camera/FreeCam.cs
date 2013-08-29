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
    class FreeCam : Camera
    {
        const float rotationSpeed = 0.3f;
        const float moveSpeed = 30.0f;

        MouseState originalMouseState;

        public FreeCam(Vector3 position, Viewport viewport, Vector2 rotAngles): base(position, viewport, rotAngles)
        {
            _cameraPosition = position;
        }

        public override void initialise()
        {
            UpdateViewMatrix();

            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _viewport.AspectRatio, 0.3f, 1000.0f);

            Mouse.SetPosition(_viewport.Width / 2, _viewport.Height / 2);
            originalMouseState = Mouse.GetState();
        }

        public override void update(GameTime gameTime)
        {
            float amount = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                _rotAngles.X -= rotationSpeed * xDifference * amount;
                _rotAngles.Y -= rotationSpeed * yDifference * amount;
                Mouse.SetPosition(_viewport.Width / 2, _viewport.Height / 2);
                UpdateViewMatrix();
            }

            Vector3 moveVector = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.Up))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.Down))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.Right))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.Left))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Z))
                moveVector += new Vector3(0, -1, 0);
            AddToCameraPosition(moveVector * amount);
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(_rotAngles.Y) * Matrix.CreateRotationY(_rotAngles.X);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            _cameraPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

    }
}

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
    public struct ViewTransformations
    {
        public Matrix viewMatrix;
        public Matrix projectionMatrix;
        public Vector3 cameraPosition;
    }

    abstract class Camera
    {
        protected Matrix _viewMatrix;
        protected Matrix _projectionMatrix;
        protected Vector3 _position;
        protected Vector3 _cameraPosition;
        protected Viewport _viewport;
        protected Vector2 _rotAngles;
        protected Quaternion _cameraRotation;

        public Camera(Vector3 position, Viewport viewport, Vector2 rotAngles)
        {
            _rotAngles = rotAngles;
            _position = position;
            _viewport = viewport;
            _cameraRotation = Quaternion.Identity;
        }

        public abstract void initialise();
        public abstract void update(GameTime gameTime);

        public ViewTransformations GetViewTransformation()
        {
            ViewTransformations vt = new ViewTransformations();

            vt.cameraPosition = _cameraPosition;
            vt.projectionMatrix = _projectionMatrix;
            vt.viewMatrix = _viewMatrix;

            return vt;
        }

        protected void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(_rotAngles.Y) * Matrix.CreateRotationY(_rotAngles.X);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = _cameraPosition + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            _viewMatrix = Matrix.CreateLookAt(_cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}

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
using ExiaProject.com.game.objects;

namespace ExiaProject.com.game.camera
{
    class ChaseCamera : Camera
    {
        CarObject _followObject;
        bool _realView = false;


        public ChaseCamera(CarObject follow, Vector3 position, Viewport viewport, Vector2 rotAngles, bool realView)
            : base(position, viewport, rotAngles)
        {
            _followObject = follow;
            _realView = realView;
        }

        public override void initialise()
        {
            updateView(); 

            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _viewport.AspectRatio, 0.3f, 1000.0f);
        }

        public override void update(GameTime gameTime)
        {
            updateView();
         }

        public void updateView()
        {
            float totalWt = _followObject._Wf + _followObject._Wr;

            float percentageWf = 0; 
            float percentageWr = 0;
            float accelRot = 0;
            float cornerRot = 0;

            if (_realView)
            {
                if (totalWt != 0)
                {
                    percentageWf = (_followObject._Wf / totalWt);
                    percentageWr = (_followObject._Wr / totalWt);

                    if (percentageWf > percentageWr)
                        accelRot = (percentageWf) * 0.15f;
                    else if (percentageWr > percentageWf)
                        accelRot = (percentageWf) * 0.07f;
                }

                if (_followObject._velocity.X != 0)
                {
                    cornerRot = 0.001f * _followObject._velocity.X;   
                }
            }

            Quaternion accel = Quaternion.CreateFromAxisAngle(new Vector3(-1, 0, 0), accelRot);
            Quaternion corn = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), cornerRot);

            _cameraRotation = Quaternion.Lerp(_cameraRotation, _followObject.CarRotation * accel * corn, 0.1f);

            _cameraPosition = _position;
            _cameraPosition = Vector3.Transform(_cameraPosition, Matrix.CreateFromQuaternion(_cameraRotation));
            _cameraPosition += _followObject.CarPosition;

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, Matrix.CreateFromQuaternion(_cameraRotation));
            Vector3 cameraFinalTarget = _cameraPosition + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, Matrix.CreateFromQuaternion(_cameraRotation));

            _viewMatrix = Matrix.CreateLookAt(_cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }


    }
}

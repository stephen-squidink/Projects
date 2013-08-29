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
using ExiaProject.com.game.camera;
using ExiaProject.com.game.input;
using ExiaProject.com.game.core;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.objects;
using ExiaProject.com.game.primitive;
using ExiaProject.com.exia.util;

namespace ExiaProject.com.game.objects
{
    class CarObject : PhysicsObject
    {
        Model _carModel;
        BoundingSphere _boundingSphere;
        SWInput _steeringWheelDevice;

        GamePadState _currentGamePadState;
        GamePadState _previousGamePadState = new GamePadState();

        SWInputStates _previousState;
        SWInputStates _currentState;

        private PhysicsManager _physicsManager;
      
        public CarObject(Model carModel, Specification carSpec, SWInput swInput)
        {
            _carModel = carModel;
            _spec = carSpec;
            _steeringWheelDevice = swInput;
        }

        public Quaternion CarRotation
        {
            get { return _rotation; }
        }

        public Vector3 CarPosition
        {
            get { return _position; }
        }

        public void initialise()
        { 
            foreach (ModelMesh mesh in _carModel.Meshes)
            {
                _boundingSphere = BoundingSphere.CreateMerged(_boundingSphere, mesh.BoundingSphere);
            }

            _physicsManager = new PhysicsManager();

            _position = new Vector3((150 * 2), 0, (200 * 2));
            _rotation = Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), MathHelper.PiOver2);
            _we = 0;
            _tractionForce = 0;
            _dragForce = 0;
            _cDrag = 0;
            _rollingForce = 0;
            
            _steeringAngle = 0;
            _steeringRadius = 0;

            _throttlePosition = 0;
            _brakingPosition = 0;
            _velocity = Vector3.Zero;
            _force = Vector3.Zero;
            _engineState = ON;
            _currentGear = "Neutral";
            _currentRPM = 1000;
                     
        }

        public void update(GameTime gameTime)
        {
            _delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _physicsManager.update(gameTime, this);

            _previousState = _currentState;
            _previousGear = _currentGear;

            _currentState = _steeringWheelDevice.GetState();

            if (_currentState.Clutch == 0)
            {
                _currentGear = _currentState.CurrentGear;

                if (_velocity.Z > 0)
                {
                    if (_currentGear == "Reverse")
                        _currentGear = _previousGear;
                }
            }

            _throttlePosition = -_currentState.Accelerator / 1000.0f;
            _brakingPosition = -_currentState.Brake / 1000.0f;
            _steeringPosition = _currentState.Steering / 1000.0f;

            if (ColoredTerrain.GetHeight(new Vector3(_position.X + (_boundingSphere.Radius * 0.3f), _position.Y, _position.Z + (_boundingSphere.Radius * 2.0f))) > 0.0f ||
                ColoredTerrain.GetHeight(new Vector3(_position.X - (_boundingSphere.Radius * 0.3f), _position.Y, _position.Z + (_boundingSphere.Radius * 2.0f))) > 0.0f ||
                ColoredTerrain.GetHeight(new Vector3(_position.X + (_boundingSphere.Radius * 0.3f), _position.Y, _position.Z - (_boundingSphere.Radius * 2.0f))) > 0.0f ||
                ColoredTerrain.GetHeight(new Vector3(_position.X - (_boundingSphere.Radius * 0.3f), _position.Y, _position.Z - (_boundingSphere.Radius * 2.0f))) > 0.0f)
            {
                _position -= _delta * _speed * 10;
                
                _velocity = Vector3.Zero;
            }

            SoundManager.getInstance().PlaySoundInstance(_currentGear, (_currentRPM / 6000), (_currentRPM / 6000));
            
            if (_previousState != null)
            {
                if (_currentGear != _previousGear)
                    SoundManager.getInstance().PlaySoundInstance("GearChange", 0.5f, 1f);
            }
        }

        public void render(GraphicsDevice device, ViewTransformations viewTrans)
        {
            device.RenderState.DepthBufferWriteEnable = true;

            Matrix[] transforms = new Matrix[_carModel.Bones.Count];
            _carModel.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix wMatrix = Matrix.CreateScale(2.8f) * Matrix.CreateFromQuaternion(_rotation) * Matrix.CreateTranslation(_position);

            foreach (ModelMesh mesh in _carModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * wMatrix;
                    effect.View = viewTrans.viewMatrix;
                    effect.Projection = viewTrans.projectionMatrix;
                }
                mesh.Draw();
            }

            _boundingSphere.Transform(wMatrix);

            device.RenderState.DepthBufferWriteEnable = true;
        }
    }
}

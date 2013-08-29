using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ExiaProject.com.game.core;
using ExiaProject.com.game.camera;
using ExiaProject.com.game.primitive;
using ExiaProject.com.game.objects;
using ExiaProject.com.game.input;
using ExiaProject.com.exia.core;
using ExiaProject.com.exia.objects;
using ExiaProject.com.exia.util;

namespace ExiaProject
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager       graphics;
        GraphicsDevice              device;

        SpriteBatch         _spriteBatch;
        SpriteFont          _spriteLargeFont;
        SpriteFont          _spriteRegularFont;

        Effect              _effect;

        SkyDome             _skyDome;
        ColoredTerrain      _terrain;

        CarObject           _car;
        Camera[]            _camera;
        Camera              _currentCam;

        Dictionary<string, Texture2D> _gears;

        Texture2D _revBG;
        Texture2D _revCount;

        SWInput _steeringWheelDevice = new SWInput();
        SoundManager _soundManager = SoundManager.getInstance();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;

            graphics.ApplyChanges();

            _steeringWheelDevice.initialize(this.Window.Handle);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            device = GraphicsDevice;

            this._spriteBatch = new SpriteBatch(device);
            this._spriteLargeFont = Content.Load<SpriteFont>("FONT\\LargeGameFont");
            this._spriteRegularFont = Content.Load<SpriteFont>("FONT\\GameFont");

            _soundManager.AddSound("Neutral", Content.Load<SoundEffect>("SOUND//Gear1"));
            _soundManager.AddSound("First", Content.Load<SoundEffect>("SOUND//Gear1"));
            _soundManager.AddSound("Second", Content.Load<SoundEffect>("SOUND//Gear2"));
            _soundManager.AddSound("Third", Content.Load<SoundEffect>("SOUND//Gear3"));
            _soundManager.AddSound("Fourth", Content.Load<SoundEffect>("SOUND//Gear4"));
            _soundManager.AddSound("Fifth", Content.Load<SoundEffect>("SOUND//Gear5"));
            _soundManager.AddSound("Reverse", Content.Load<SoundEffect>("SOUND//Gear2"));
            _soundManager.AddSound("GearChange", Content.Load<SoundEffect>("SOUND//Gear1ToGear2"));
            _soundManager.AddSound("BrakeMajor", Content.Load<SoundEffect>("SOUND//BrakeMajor"));
            _soundManager.AddSound("BrakeCurveMajor", Content.Load<SoundEffect>("SOUND//BrakeCurveMajor"));
            
            _gears = new Dictionary<string, Texture2D>();

            _gears.Add("Neutral", Content.Load<Texture2D>("SPRITES//Gear_Neutral"));
            _gears.Add("First", Content.Load<Texture2D>("SPRITES//Gear_First"));
            _gears.Add("Second", Content.Load<Texture2D>("SPRITES//Gear_Second"));
            _gears.Add("Third", Content.Load<Texture2D>("SPRITES//Gear_Third"));
            _gears.Add("Fourth", Content.Load<Texture2D>("SPRITES//Gear_Fourth"));
            _gears.Add("Fifth", Content.Load<Texture2D>("SPRITES//Gear_Fifth"));
            _gears.Add("Sixth", Content.Load<Texture2D>("SPRITES//Gear_Neutral"));
            _gears.Add("Reverse", Content.Load<Texture2D>("SPRITES//Gear_Reverse"));

            _revBG = Content.Load<Texture2D>("SPRITES//RevBG");
            _revCount = Content.Load<Texture2D>("SPRITES//RevCount"); 

            _effect = Content.Load<Effect>("EFFECT//m_effects");

            _skyDome = new SkyDome(Content.Load<Model>("MODEL//DOME//dome"), Content.Load<Texture2D>("MODEL//DOME//cloudMap"), _effect.Clone(device));
            _terrain = new ColoredTerrain(_effect.Clone(device), new Vector2(500, 500), Content.Load<Texture2D>("TEXTURE//heightmap6"));
            //_terrain = new ColoredTerrain(_effect.Clone(device), new Vector2(500, 500), Content.Load<Texture2D>("TEXTURE//flatheightmap"));

            _car = new CarObject(Content.Load<Model>("MODEL//SL500//SL500"), CreateSL500Spec(), _steeringWheelDevice);
            _car.initialise();

            _camera = new Camera[3];

            _camera[0] = new ChaseCamera(_car, new Vector3(-0.35f * 2.8f, 1.0f * 2.8f, 0.3f * 2.8f), device.Viewport, new Vector2(MathHelper.TwoPi, MathHelper.TwoPi),true);
            _camera[1] = new ChaseCamera(_car, new Vector3(0f * 2.8f, 2f * 2.8f, 7f * 2.8f), device.Viewport, new Vector2(MathHelper.TwoPi, MathHelper.TwoPi),false);
            _camera[2] = new StationaryCamera(new Vector3(1000, 150, 500), device.Viewport, new Vector2(MathHelper.PiOver2, -MathHelper.Pi / 10f));

            _camera[0].initialise();
            _camera[1].initialise();
            _camera[2].initialise();

            _currentCam = _camera[0];

            _terrain.intialize(device);
        }

        private Specification CreateSL500Spec()
        {
            Specification spec = new Specification();

            //Dimensions, values = mm
            spec.OverallLength = 4535.0f;
            spec.OverallWidth = 1827.0f;
            spec.OverallHeight = 1298.0f;
            spec.WheelBase = 2560.0f;
            spec.TrackFront = 1559;
            spec.TrackRear = 1547;

            //Dynamics, Wegihts = KG, Distributions = %
            spec.CurbWeight = 1835.0f;
            spec.DragCoefficient = 0.29f;
            spec.FrontalArea = 2.00f;
            spec.WeightDistributionFront = 51.0f;
            spec.WeightDistributionRear = 49.0f;

            //Engine, Displacment = CC, Power = Bhp, Revs = rpm, Torque = Nm;
            spec.Displacement = 4966.0f;
            spec.MaxPower = 302.0f;
            spec.MaxTorque = 460.0f;
            spec.MaxPowerAtRPM = 5600.0f;
            spec.MaxTorqueAtRPM = 2700.0f;
            spec.RedLine = 6000.0f;

            spec.AddGear("Neutral", 0.0f);
            spec.AddGear("Reverse", 2.19f);
            spec.AddGear("First", 3.59f);
            spec.AddGear("Second", 2.19f);
            spec.AddGear("Third", 1.41f);
            spec.AddGear("Fourth", 1.00f);
            spec.AddGear("Fifth", 0.83f);
            spec.AddGear("Sixth", 0.0f);

            spec.FinalDrive = 2.82f;
   
            spec.TransmissionEfficiency = 0.7f;
            spec.TopSpeed = 155;
            spec.LateralAccel = 0.90f;

            spec.WheelRadius = 33.0f;

            return spec;
        }

        protected override void UnloadContent()
        {
           
        }

        KeyboardState _currentState = Keyboard.GetState();
        KeyboardState _previousState = Keyboard.GetState();

        protected override void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (_previousState.IsKeyDown(Keys.F11) && _currentState.IsKeyUp(Keys.F11))
            {
                this.graphics.ToggleFullScreen();
            }

            _car.update(gameTime);
            _currentCam.update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                ChangeCam(0);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                ChangeCam(1);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                ChangeCam(2);

            base.Update(gameTime);
        }

        private void ChangeCam(int index)
        {
            _currentCam = _camera[index];
            _currentCam.initialise();
        }

        public TimeSpan _time;

        protected override void Draw(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;
            device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            device.RenderState.FillMode = FillMode.Solid;
            
            _time = gameTime.TotalGameTime;
            
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            _skyDome.render(device, _currentCam.GetViewTransformation());
            _terrain.render(device, _currentCam.GetViewTransformation());
            _car.render(device, _currentCam.GetViewTransformation());

            DrawSpeed();
            DrawGear();
            DrawRPM();
            DrawTime();

            base.Draw(gameTime);
        }

        public void DrawSpeed()
        {
            _spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,SaveStateMode.SaveState);

            string speed = ""+UnitConverter.MStoMPH(_car._velocity.Z);
            string unit = " mph";

            Vector2 sizeSpeed = _spriteLargeFont.MeasureString(speed);
            Vector2 sizeUnit = _spriteRegularFont.MeasureString(unit);

            Vector2 posSpeed = new Vector2(device.Viewport.Width - sizeSpeed.X - 10 - sizeUnit.X, device.Viewport.Height - sizeSpeed.Y +10);
            Vector2 posSize = new Vector2(device.Viewport.Width - sizeUnit.X - 10, device.Viewport.Height - sizeUnit.Y - 10);

            _spriteBatch.DrawString(_spriteLargeFont, speed, posSpeed, Color.White);
            _spriteBatch.DrawString(_spriteRegularFont, unit, posSize, Color.White);

            _spriteBatch.End();
        }

        public void DrawGear()
        {
            _spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            Vector2 pos = new Vector2(device.Viewport.Width - _gears[_car._currentGear].Width - 250, device.Viewport.Height - _gears[_car._currentGear].Height - _gears[_car._currentGear].Height + 10);

            _spriteBatch.Draw(_gears[_car._currentGear], pos ,null,Color.White,0,Vector2.Zero,1.5f,SpriteEffects.None,0);

            _spriteBatch.End();
        }

        public void DrawRPM()
        {
            _spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            Vector2 pos = new Vector2(device.Viewport.Width - _revBG.Width - 10, device.Viewport.Height - _revBG.Height - 100);

            _spriteBatch.Draw(_revBG, pos, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_revCount, pos, new Rectangle(0, 0, (int)(_revBG.Width*(_car._currentRPM/6000)), _revBG.Height), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            string rpm = "" + (int)_car._currentRPM;
            string unit = " rpm";

            Vector2 sizeRPM = _spriteRegularFont.MeasureString(rpm);
            Vector2 sizeUnit = _spriteRegularFont.MeasureString(unit);

            Vector2 posRPM = new Vector2(device.Viewport.Width - sizeRPM.X - 10 - sizeUnit.X, device.Viewport.Height - sizeRPM.Y - 130);
            Vector2 posSize = new Vector2(device.Viewport.Width - sizeUnit.X - 10, device.Viewport.Height - sizeUnit.Y - 130);

            _spriteBatch.DrawString(_spriteRegularFont, rpm, posRPM, Color.White);
            _spriteBatch.DrawString(_spriteRegularFont, unit, posSize, Color.White);


            _spriteBatch.End();
        }

        public void DrawTime()
        {
            _spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            string milliSeconds = ":" + _time.Milliseconds;
            string seconds = ":" + _time.Seconds;
            string minutes = ":" + _time.Minutes;
            string hours = "" + _time.Hours;

            string time = hours + minutes + seconds + milliSeconds;

            Vector2 sizetime = _spriteLargeFont.MeasureString(time);

            Vector2 posTime = new Vector2(0,0);

            _spriteBatch.DrawString(_spriteLargeFont, time, posTime, Color.White);

            _spriteBatch.End();

        }


    }
}

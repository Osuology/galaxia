using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Galaxia
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Define the stars.
        Stars stars;

        //Define a MenuManager.
        MenuManager mm;

        //Define a Camera.
        Camera cam;

        Song song;

        RenderTarget2D target;

        double rigidTimer = 0;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;

            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            StaticValues.LoadResolutions();

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            Group.InitGroups();
            AnimationManager.LoadContent(Content);
            cam = new Camera(graphics.GraphicsDevice.Viewport);
            target = new RenderTarget2D(GraphicsDevice, 1280, 720);

            StaticValues.scale = new Vector2(graphics.PreferredBackBufferWidth / 1280f, graphics.PreferredBackBufferHeight / 720f);

            //Initialize the stars.
            stars = new Stars(graphics.GraphicsDevice);
            //Initialize the Menu Manager.
            mm = new MenuManager(cam);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mm.LoadContent(Content);
            SoundManager.LoadContent(Content);

            song = Content.Load<Song>("Sounds/pasace");
        }

        protected override void UnloadContent()
        {
        }

        public void RigidUpdate(GameTime gt)
        {

            //Update inputs.
            Input.Update();

            if (StaticValues.Close)
            {
                StaticValues.SaveHighscores();

                Exit();
            }

            //Update stars.
            stars.Update(cam);

            AnimationManager.Update(gt);

            //Update menus.
            mm.Update(cam, gt);

            if (StaticValues.Gamestate == 1)
                cam.SetPos(0, 0);

            if (TargetElapsedTime == TimeSpan.FromSeconds(1f / 240))
            {
                cam.SetPos(0, 0);
            }

            //Update old inputs.
            Input.OldUpdate();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (StaticValues.fpsOptions[StaticValues.fpsLimit] == 1000)
            {
                IsFixedTimeStep = false;
                graphics.SynchronizeWithVerticalRetrace = false;
            }
            else if (StaticValues.fpsOptions[StaticValues.fpsLimit] == 0)
            {
                IsFixedTimeStep = false;
                graphics.SynchronizeWithVerticalRetrace = true;
            }
            else
            {
                graphics.SynchronizeWithVerticalRetrace = false;
                IsFixedTimeStep = true;
                TargetElapsedTime = TimeSpan.FromSeconds(1f / (float)StaticValues.fpsOptions[StaticValues.fpsLimit]);
            }

            graphics.PreferredBackBufferWidth = (int)StaticValues.res[StaticValues.currentRes].X;
            graphics.PreferredBackBufferHeight = (int)StaticValues.res[StaticValues.currentRes].Y;

            if (graphics.PreferredBackBufferWidth == GraphicsDevice.Adapter.CurrentDisplayMode.Width && !graphics.IsFullScreen)
                graphics.IsFullScreen = true;
            else if (graphics.IsFullScreen && graphics.PreferredBackBufferWidth != GraphicsDevice.Adapter.CurrentDisplayMode.Width)
                graphics.IsFullScreen = false;

            graphics.ApplyChanges();
            StaticValues.scale = new Vector2(graphics.PreferredBackBufferWidth / 1280f, graphics.PreferredBackBufferHeight / 720f);

            rigidTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (rigidTimer >= 16.666667)
            {
                RigidUpdate(gameTime);
                rigidTimer = 0;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(target);

            GraphicsDevice.Clear(new Color(10, 0, 30));

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, cam.matrix);
            //Draw stars.
            stars.Draw(spriteBatch, cam);
            //Draw the current menu sprites.
            mm.Draw(spriteBatch, cam, gameTime);
            AnimationManager.Draw(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(target, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

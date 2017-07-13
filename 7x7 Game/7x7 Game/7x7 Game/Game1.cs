using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace _7x7_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        internal static GraphicsDeviceManager graphics = null;
        internal static SpriteBatch spriteBatch = null;
        internal static ScreenManager GScreenManager;
        int? scaleFactor = null;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Services.AddService(typeof(GraphicsDeviceManager), graphics);

            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            // Make sure screen-saver doesn't interrupt the game
            Guide.IsScreenSaverEnabled = false;
            //Only allowing portrait landscape screen orientations for this game
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
    
            graphics.IsFullScreen = true;

            if (Environment.OSVersion.Version.Major == 8)
            {
                int? scaleFactor = null;
                var content = Application.Current.Host.Content;
                var scaleFactorProperty = content.GetType().GetProperty("ScaleFactor");
                if (scaleFactorProperty != null)
                {
                    scaleFactor = scaleFactorProperty.GetValue(content, null) as int?;
                }
                if (scaleFactor == null)
                    scaleFactor = 100;

                if (scaleFactor == 150)
                {
                    graphics.PreferredBackBufferHeight = 800;
                    graphics.PreferredBackBufferWidth = 450;
                }
                else
                {
                    graphics.PreparingDeviceSettings += (s1, e1) =>
                    {
                        //Setup portrait screen resolution
                        e1.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight =
                            e1.GraphicsDeviceInformation.Adapter.CurrentDisplayMode.Height;
                        e1.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth =
                            e1.GraphicsDeviceInformation.Adapter.CurrentDisplayMode.Width;
                    };
                }
            }
            else
            {
                graphics.PreparingDeviceSettings += (s1, e1) =>
                {
                    //Setup portrait screen resolution
                    e1.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight =
                        e1.GraphicsDeviceInformation.Adapter.CurrentDisplayMode.Height;
                    e1.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth =
                        e1.GraphicsDeviceInformation.Adapter.CurrentDisplayMode.Width;
                };
            }

            try
            {
                graphics.ApplyChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("graphics.ApplyChanges() failed: " + ex.Message);
                Debugger.Break();
            }

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            GScreenManager=new ScreenManager(this);
            this.Components.Add(GScreenManager);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Silver);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

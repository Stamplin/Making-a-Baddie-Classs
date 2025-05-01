using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.ExceptionServices;

namespace Making_a_Baddie_Classs
{

    //screens
    enum Screen
    {
        Title,
        House,
        End
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //random num gen
        Random generator;
        //textures
        List<Texture2D> booTextures;
        Texture2D hauntedBackgroundTexture;
        Texture2D titleTexture;
        Texture2D endTexture;
        Texture2D marioTexture;

        //rects
        Rectangle marioRect;

        //screens
        Screen screen;

        //mouse and keyboard state
        MouseState mouseState;
        KeyboardState keyboardState;

        //ghost list
        List<Ghost> ghosts;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //set current screen
            screen = Screen.Title;

            //initialize random num gen
            generator = new Random();

            //booTextures List
            booTextures = new List<Texture2D>();

            //ghostsList
            ghosts = new List<Ghost>();

            //MarioRect
            marioRect = new Rectangle(0, 0, 30, 30);

            base.Initialize();

            //hide mouse
            this.IsMouseVisible = false;

            for (int i = 0; i < 20; i++)
                ghosts.Add(new Ghost(booTextures, new Rectangle(generator.Next(500), generator.Next(500), 40, 40)));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //load textures
            hauntedBackgroundTexture = Content.Load<Texture2D>("haunted-background");
            titleTexture = Content.Load<Texture2D>("haunted-title");
            endTexture = Content.Load<Texture2D>("haunted-end-screen");
            marioTexture = Content.Load<Texture2D>("mario");

            //add boo textures to list
            booTextures.Add(Content.Load<Texture2D>("boo-stopped"));
            for (int i = 1; i < 8; i++)
            {
                booTextures.Add(Content.Load<Texture2D>("boo-move-" + i));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //update mouse and keyboard state
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            //mario follow mouse
            marioRect.Location = mouseState.Position;

            //exit the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (screen == Screen.Title)
            {
                //start the game
                if (keyboardState.IsKeyDown(Keys.Enter))
                    screen = Screen.House;
            }
            else if (screen == Screen.House)
            {
                foreach (Ghost ghost in ghosts)
                {
                    ghost.Update(gameTime, mouseState);
                    if (ghost.Intersects(marioRect) && mouseState.LeftButton == ButtonState.Pressed)
                        screen = Screen.End;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (screen == Screen.Title)
                _spriteBatch.Draw(titleTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            else if (screen == Screen.House)
            {
                _spriteBatch.Draw(hauntedBackgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                foreach (Ghost ghost in ghosts)
                    ghost.Draw(_spriteBatch);
            }
            else
                _spriteBatch.Draw(endTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            _spriteBatch.Draw(marioTexture, marioRect, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pyaterochka
{
    public class Game1 : Game
    {
        protected GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameController controller;
        private MainMenu mainMenu;
        private bool isInMenu = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            var mapWidth = GameMap.Map.GetLength(1);
            var mapHeight = GameMap.Map.GetLength(0);
            var tileSize = 40;

            graphics.PreferredBackBufferWidth = mapWidth * tileSize;
            graphics.PreferredBackBufferHeight = mapHeight * tileSize;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            mainMenu = new MainMenu();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainMenu.LoadContent(Content, GraphicsDevice.Viewport);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (isInMenu)
            {
                mainMenu.Update(gameTime);
                if (mainMenu.IsPlayClicked)
                {
                    isInMenu = false;
                    controller = new GameController(graphics);
                    controller.LoadContent(Content);
                }
            }
            else
            {
                controller.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            if (isInMenu)
            {
                mainMenu.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Begin();
                controller.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
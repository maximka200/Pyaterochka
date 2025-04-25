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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            // graphics.IsFullScreen = true;
            var mapWidth = GameMap.Map.GetLength(1); // Количество клеток по ширине
            var mapHeight = GameMap.Map.GetLength(0); // Количество клеток по высоте
            var tileSize = 40; // Размер одной клетки в пикселях

            graphics.PreferredBackBufferWidth = mapWidth * tileSize; // Ширина экрана
            graphics.PreferredBackBufferHeight = mapHeight * tileSize;
        }

        protected override void Initialize()
        {
            controller = new GameController(graphics);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            controller.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            controller.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            controller.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

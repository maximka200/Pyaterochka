using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pyaterochka.Buyers;

namespace Pyaterochka
{
    public class GameView(GameModel model, GraphicsDeviceManager graphics)
    {
        private Texture2D playerTexture;
        private Texture2D floorTexture;
        private Texture2D wallTexture;
        private Texture2D doorTexture;
        private Texture2D gameOverTexture;
        private Texture2D boozerTexture;
        private Texture2D babushkaTexture;
        private Texture2D usualTexture;
        
        private SpriteFont font;

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("player");
            floorTexture = content.Load<Texture2D>("map");
            wallTexture = content.Load<Texture2D>("wall");
            doorTexture = content.Load<Texture2D>("door");
            gameOverTexture = content.Load<Texture2D>("over");
            babushkaTexture= content.Load<Texture2D>("babushka");
            boozerTexture = content.Load<Texture2D>("boozer");
            usualTexture = content.Load<Texture2D>("usual");
            font = content.Load<SpriteFont>("font");
        }

    public void Draw(SpriteBatch spriteBatch)
    {
        var tileSize = model.TileSize;
        var mapWidth = GameMap.Map.GetLength(1) * tileSize;
        var mapHeight = GameMap.Map.GetLength(0) * tileSize;

        // Центрирование карты
        var screenCenter = new Point(
            graphics.PreferredBackBufferWidth / 2,
            graphics.PreferredBackBufferHeight / 2
        );
        var mapOffset = new Point(
            screenCenter.X - mapWidth / 2,
            screenCenter.Y - mapHeight / 2
        );

        // Рисуем карту
        for (var y = 0; y < GameMap.Map.GetLength(0); y++)
        {
            for (var x = 0; x < GameMap.Map.GetLength(1); x++)
            {
                var pos = new Rectangle(
                    mapOffset.X + x * tileSize,
                    mapOffset.Y + y * tileSize,
                    tileSize,
                    tileSize
                );

                switch (GameMap.Map[y, x])
                {
                    case 0:
                        spriteBatch.Draw(floorTexture, pos, Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(wallTexture, pos, Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(doorTexture, pos, Color.White);
                        break;
                }
            }
        }

        // Рисуем игрока
        spriteBatch.Draw(
            playerTexture,
            new Rectangle(
                (int)model.Player.Position.X + mapOffset.X - model.Player.HitBox / 2,
                (int)model.Player.Position.Y + mapOffset.Y - model.Player.HitBox / 2,
                model.Player.HitBox,
                model.Player.HitBox
            ),
            Color.White
        );

        // Рисуем покупателей
        foreach (var buyer in model.Buyers)
        {
            if (buyer.IsBanned) continue;

            var dest = new Rectangle(
                (int)buyer.Position.X + mapOffset.X,
                (int)buyer.Position.Y + mapOffset.Y,
                buyer.HitBox,
                buyer.HitBox
            );

            switch (buyer)
            {
                case Boozer:
                    spriteBatch.Draw(boozerTexture, dest, Color.White);
                    break;
                case Babushka:
                    spriteBatch.Draw(babushkaTexture, dest, Color.White);
                    break;
                case Usual:
                    spriteBatch.Draw(usualTexture, dest, Color.White);
                    break;
            }
        }

        DrawBars(spriteBatch, model.Player);

        if (!model.IsGameOver) return;

        // Затемнение экрана и вывод "Game Over"
        spriteBatch.Draw(wallTexture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black);
        spriteBatch.Draw(gameOverTexture, new Vector2(
            graphics.PreferredBackBufferWidth / 2 - gameOverTexture.Width / 2,
            graphics.PreferredBackBufferHeight / 2 - gameOverTexture.Height / 2), Color.White);
    }

        
        private void DrawBars(SpriteBatch spriteBatch, IPlayer playerInstance)
        {
            if (playerInstance != null)
            {
                var barWidth = 100;
                var barHeight = 10;
        
                var hpBar = new Rectangle(10, 10, (int)(barWidth * (playerInstance.Health / 3f)), barHeight);
                spriteBatch.Draw(wallTexture, hpBar, Color.Red);
        
                var staminaBar = new Rectangle(10, 25, (int)(barWidth * (playerInstance.Stamina / 100f)), barHeight);
                spriteBatch.Draw(wallTexture, staminaBar, Color.Green);

                // Нарисовать счёт
                var scoreText = $"Score: {playerInstance.Score}";

                // Размер текста
                var textSize = font.MeasureString(scoreText);

                // Позиция текста: правый верхний угол
                var position = new Vector2(
                    graphics.PreferredBackBufferWidth - textSize.X - 10,
                    10
                );

                spriteBatch.DrawString(font, scoreText, position, Color.White);
            }
        }

    }
}

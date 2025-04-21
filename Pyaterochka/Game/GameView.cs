using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pyaterochka.Buyers;

namespace Pyaterochka
{
    public class GameView
    {
        private GameModel model;
        private Texture2D playerTexture;
        private Texture2D floorTexture;
        private Texture2D wallTexture;
        private Texture2D doorTexture;
        private Texture2D gameOverTexture;
        private Texture2D boozerTexture;
        private Texture2D babushkaTexture;

        public GameView(GameModel model)
        {
            this.model = model;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("player");
            floorTexture = content.Load<Texture2D>("map");
            wallTexture = content.Load<Texture2D>("wall");
            doorTexture = content.Load<Texture2D>("door");
            gameOverTexture = content.Load<Texture2D>("over");
            babushkaTexture= content.Load<Texture2D>("babushka");
            boozerTexture = content.Load<Texture2D>("boozer");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var tileSize = model.TileSize;
            
            for (int y = 0; y < GameMap.Map.GetLength(0); y++)
            {
                for (int x = 0; x < GameMap.Map.GetLength(1); x++)
                {
                    var pos = new Rectangle(
                        x * tileSize - tileSize / 2,  // Смещение по X
                        y * tileSize - tileSize / 2,  // Смещение по Y
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
            
            spriteBatch.Draw(
                playerTexture,
                new Rectangle(
                    (int)model.Player.Position.X - model.Player.HitBox / 2,
                    (int)model.Player.Position.Y - model.Player.HitBox / 2,
                    model.Player.HitBox,
                    model.Player.HitBox
                ),
                Color.White
            );
            foreach (var buyer in model.Buyers)
            {
                if (!buyer.IsBanned)
                    switch (buyer)
                    {
                        case Boozer:
                            spriteBatch.Draw(boozerTexture, new Rectangle(
                                (int)buyer.Position.X - buyer.HitBox / 2, 
                                (int)buyer.Position.Y - buyer.HitBox / 2, 
                                buyer.HitBox, buyer.HitBox), 
                                Color.White);
                            break;
                        case Babushka:
                            spriteBatch.Draw(babushkaTexture, new Rectangle((int)buyer.Position.X, (int)buyer.Position.Y, buyer.HitBox, buyer.HitBox), Color.White);
                            break;
                    }
                    
            }

            DrawBars(spriteBatch, model.Player);

            if (model.IsGameOver)
            {
                spriteBatch.Draw(wallTexture, new Rectangle(0, 0, 800, 600), Color.Black);
                spriteBatch.Draw(gameOverTexture, new Vector2(400 - gameOverTexture.Width / 2, 300 - gameOverTexture.Height / 2), Color.White);
            }
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
            }
        }
    }
}

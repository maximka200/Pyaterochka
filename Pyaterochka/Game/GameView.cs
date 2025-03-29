using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pyaterochka
{
    public class GameView
    {
        private GameModel model;
        private Texture2D playerTexture;
        private Texture2D mapTexture;
        private Texture2D wallTexture;

        public GameView(GameModel model)
        {
            this.model = model;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("player");
            mapTexture = content.Load<Texture2D>("map");
            wallTexture = content.Load<Texture2D>("wall");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var player = model.Player;
            spriteBatch.Draw(mapTexture, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.Draw(playerTexture, new Rectangle((int)player.Position.X, (int)player.Position.Y, player.HitBox, player.HitBox), Color.White);
    
            foreach (var wall in model.Walls)
            {
                spriteBatch.Draw(wallTexture, wall, Color.White);
            }

            var playerInstance = player;
            DrawBars(spriteBatch, playerInstance);
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

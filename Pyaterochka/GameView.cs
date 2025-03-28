using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownGame;

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
        spriteBatch.Draw(mapTexture, new Rectangle(0, 0, 800, 600), Color.White);
        spriteBatch.Draw(playerTexture, new Rectangle((int)model.PlayerPosition.X, (int)model.PlayerPosition.Y, 16, 16), Color.White);
        foreach (var wall in model.Walls)
        {
            spriteBatch.Draw(wallTexture, wall, Color.White);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pyaterochka;

public class GameController
{
    private GameModel model;
    private GameView view;

    public GameController()
    {
        model = new GameModel();
        view = new GameView(model);
    }

    public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
    {
        view.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        model.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        view.Draw(spriteBatch);
    }
}
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pyaterochka;

public class GameController
{
    private GameModel model;
    private GameView view;
    private KeyboardState previousKeyboardState;
    private float distanseToArrest = 100f;

    public GameController(GraphicsDeviceManager graphics)
    {
        model = new GameModel();
        view = new GameView(model, graphics);
    }

    public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
    {
        view.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        model.Update(gameTime);

        var currentKeyboardState = Keyboard.GetState();

        if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
        {
            foreach (var buyer in model.Buyers)
            {
                if (AccusationSystem.Accuse(model.Player, buyer) == AccusationResult.Success)
                {
                    break;
                }
            }
        }

        previousKeyboardState = currentKeyboardState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        view.Draw(spriteBatch);
    }
}
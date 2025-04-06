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

        var currentKeyboardState = Keyboard.GetState();

        if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
        {
            foreach (var buyer in model.Buyers)
            {
                if (Vector2.Distance(model.Player.Position, buyer.Position) < distanseToArrest)
                {
                    if (buyer is Buyer concreteBuyer)
                    {
                        if (concreteBuyer.IsThief())
                        {
                            
                            buyer.Ban();
                        }
                        else
                        {
                            model.Player.TakeDamage(1);
                        }
                        break; // Обвиняем только одного
                    }
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
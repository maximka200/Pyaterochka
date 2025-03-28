using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TopDownGame;

public class GameModel
{
    public Vector2 PlayerPosition { get; private set; }
    private float playerSpeed = 2f;
    public Rectangle[] Walls { get; private set; }
    private int playerSize = 16;

    public GameModel()
    {
        PlayerPosition = new Vector2(100, 100);
        Walls = new Rectangle[]
        {
            new Rectangle(200, 200, 50, 50),
            new Rectangle(300, 100, 50, 50)
        };
    }

    public void Update()
    {
        var keyboardState = Keyboard.GetState();
        Vector2 newPosition = PlayerPosition;

        if (keyboardState.IsKeyDown(Keys.W))
            newPosition.Y -= playerSpeed;
        if (keyboardState.IsKeyDown(Keys.S))
            newPosition.Y += playerSpeed;
        if (keyboardState.IsKeyDown(Keys.A))
            newPosition.X -= playerSpeed;
        if (keyboardState.IsKeyDown(Keys.D))
            newPosition.X += playerSpeed;

        Rectangle newPlayerBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, playerSize, playerSize);
        bool collides = false;
        foreach (var wall in Walls)
        {
            if (newPlayerBounds.Intersects(wall))
            {
                collides = true;
                break;
            }
        }
            
        if (!collides)
            PlayerPosition = newPosition;
    }
}
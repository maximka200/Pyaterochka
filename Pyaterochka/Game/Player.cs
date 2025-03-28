using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pyaterochka;

public class Player
{
    public Vector2 Position { get; private set; }
    private float speed = 2f;
    public readonly int HitBox = 16;

    public Player(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Update(Rectangle[] walls)
    {
        var keyboardState = Keyboard.GetState();
        Vector2 newPosition = Position;

        if (keyboardState.IsKeyDown(Keys.W))
            newPosition.Y -= speed;
        if (keyboardState.IsKeyDown(Keys.S))
            newPosition.Y += speed;
        if (keyboardState.IsKeyDown(Keys.A))
            newPosition.X -= speed;
        if (keyboardState.IsKeyDown(Keys.D))
            newPosition.X += speed;

        Rectangle newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, HitBox, HitBox);
        bool collides = false;
        foreach (var wall in walls)
        {
            if (newBounds.Intersects(wall))
            {
                collides = true;
                break;
            }
        }
            
        if (!collides)
            Position = newPosition;
    }
}
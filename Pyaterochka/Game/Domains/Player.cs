using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pyaterochka;

public class Player : IPlayer
{
    public int Health { get; set; } = 3;
    public int Stamina { get; private set; } = StaminaMax;
    public Vector2 Position { get; private set; }
    public static int StaminaTick = 7;
    public static int StaminaMax = StaminaTick * 30;
    public static float SpeedWalk => 2f;
    public static float SpeedRun => 6f;
    public int HitBox => 40;
    public int Score {get; set;}

    public Player(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Update(GameMap gameMap)
    {
        var keyboardState = Keyboard.GetState();
        var newPosition = PlayerMove(keyboardState);
        var newBounds = new Rectangle((int)newPosition.X - HitBox/2, (int)newPosition.Y - HitBox/2, HitBox, HitBox);
        var collides = false;
        foreach (var wall in gameMap.Walls)
        {
            if (newBounds.Intersects(wall) || newBounds.Intersects(gameMap.Door))
            {
                collides = true;
                break;
            }
        }
            
        if (!collides)
            Position = newPosition;
    }

    public Vector2 PlayerMove(KeyboardState keyboardState)
    {
        var newPosition = Position;
        if (keyboardState.IsKeyDown(Keys.W))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.Y -= SpeedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.Y -= SpeedWalk;
        if (keyboardState.IsKeyDown(Keys.S))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.Y += SpeedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.Y += SpeedWalk;
        if (keyboardState.IsKeyDown(Keys.A))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.X -= SpeedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.X -= SpeedWalk;
        if (keyboardState.IsKeyDown(Keys.D))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.X += SpeedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.X += SpeedWalk;
        if (Stamina < StaminaMax - 1)
            Stamina += 1;
        
        return newPosition;
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
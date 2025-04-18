﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pyaterochka;

public class Player : IPlayer
{
    public int Health { get; private set; } = 3;
    public int Stamina { get; private set; } = StaminaMax;
    public Vector2 Position { get; private set; }
    private static int StaminaTick = 7;
    private static int StaminaMax = StaminaTick * 30;
    private static float speedWalk => 2f;
    private static float speedRun => 6f;
    public int HitBox => 60;

    public Player(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Update(Rectangle[] walls, Rectangle door)
    {
        var keyboardState = Keyboard.GetState();
        Vector2 newPosition = PlayerMove(keyboardState);
        var newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, HitBox, HitBox);
        var collides = false;
        foreach (var wall in walls)
        {
            if (newBounds.Intersects(wall) || newBounds.Intersects(door))
            {
                collides = true;
                break;
            }
        }
            
        if (!collides)
            Position = newPosition;
    }

    private Vector2 PlayerMove(KeyboardState keyboardState)
    {
        Vector2 newPosition = Position;
        if (keyboardState.IsKeyDown(Keys.W))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.Y -= speedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.Y -= speedWalk;
        if (keyboardState.IsKeyDown(Keys.S))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.Y += speedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.Y += speedWalk;
        if (keyboardState.IsKeyDown(Keys.A))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.X -= speedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.X -= speedWalk;
        if (keyboardState.IsKeyDown(Keys.D))
            if (keyboardState.IsKeyDown(Keys.LeftShift) && Stamina > StaminaTick)
            {
                newPosition.X += speedRun;
                Stamina -= StaminaTick;
            }
            else
                newPosition.X += speedWalk;
        if (Stamina < StaminaMax - 1)
            Stamina += 1;
        
        return newPosition;
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
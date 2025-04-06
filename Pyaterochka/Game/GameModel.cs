using System;
using Microsoft.Xna.Framework;
using Pyaterochka;

public class GameModel
{
    public IPlayer Player { get; private set; }
    public Rectangle Door { get; private set; }
    public IBuyer[] Buyers { get; private set; }
    public Rectangle[] Walls { get; private set; }
    public bool IsGameOver => (Player?.Health ?? 3) < 1;

    public GameModel()
    {
        Player = new Player(new Vector2(100, 100));
        Walls = new Rectangle[]
        {
            new Rectangle(200, 200, 50, 50),
            new Rectangle(300, 100, 50, 50)
        };
        Door = new Rectangle(400, 300, 30, 70);
        Buyers = new Buyer[]
        {
            new Buyer(new Vector2(100, 100), new Random().Next(2) == 0),
        };
    }

    public void Update()
    {
        Player.Update(Walls, Door);
        foreach (var buyer in Buyers)
        {
            buyer.Update(Walls, Door);
        }
        if (IsGameOver)
        {
            // Здесь можно обработать завершение игры
        }
    }
}
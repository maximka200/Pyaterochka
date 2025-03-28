using Microsoft.Xna.Framework;
using Pyaterochka;

public class GameModel
{
    public Player Player { get; private set; }
    public Rectangle[] Walls { get; private set; }

    public GameModel()
    {
        Player = new Player(new Vector2(100, 100));
        Walls = new Rectangle[]
        {
            new Rectangle(200, 200, 50, 50),
            new Rectangle(300, 100, 50, 50)
        };
    }

    public void Update()
    {
        Player.Update(Walls);
    }
}
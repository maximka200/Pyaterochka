using Microsoft.Xna.Framework;

namespace Pyaterochka.Buyers;

public class Usual : Buyer
{
    public Usual(Vector2 pos, IPlayer player) : base(pos, player, random.Next(3) == 1) {}
}
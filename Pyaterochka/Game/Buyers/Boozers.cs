using Microsoft.Xna.Framework;

namespace Pyaterochka.Buyers;

public class Boozer : Buyer { 
    public Boozer(Vector2 pos, IPlayer player) : base(pos, player, true) {} 
}

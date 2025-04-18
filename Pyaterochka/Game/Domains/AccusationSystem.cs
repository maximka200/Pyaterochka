using Microsoft.Xna.Framework;

namespace Pyaterochka;

public static class AccusationSystem
{
    public static bool Accuse(IPlayer player, IBuyer buyer)
    {
        var distance = Vector2.Distance(player.Position, buyer.Position);
        var accusationRange = 100f;

        if (distance <= accusationRange)
        {
            player.Score++;
            return buyer.IsThief();
        }

        return false;
    }
}
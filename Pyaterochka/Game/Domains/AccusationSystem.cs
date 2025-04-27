using Microsoft.Xna.Framework;

namespace Pyaterochka;

public static class AccusationSystem
{
    public static AccusationResult Accuse(IPlayer player, IBuyer buyer)
    {
        var distance = Vector2.Distance(player.Position, buyer.Position);
        var accusationRange = 100f;

        if (distance <= accusationRange)
        {
            if (buyer.IsThief())
            {
                SoundManager.PlaySoundEffect("successful-accusation");
                player.Score++;
                buyer.Ban();
                return AccusationResult.Success;
            }
            else
            {
                SoundManager.PlaySoundEffect("unsuccessful-accusation");
                player.TakeDamage(1);
                return AccusationResult.Success;
            }
        }

        return AccusationResult.OutOfRange;
    }
}

public enum AccusationResult
{
    Success,
    OutOfRange
}
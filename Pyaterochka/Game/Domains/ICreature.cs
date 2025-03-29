using Microsoft.Xna.Framework;

namespace Pyaterochka;

public interface ICreature
{
    Vector2 Position { get; }
    int HitBox { get; }
    void Update(Rectangle[] walls);
}
namespace Pyaterochka;

public interface IPlayer : ICreature
{
    int Health { get; }
    int Stamina { get; }
    void TakeDamage(int damage);
}
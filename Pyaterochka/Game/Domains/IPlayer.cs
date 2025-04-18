namespace Pyaterochka;

public interface IPlayer : ICreature
{
    int Score { get; set; }
    int Health { get; }
    int Stamina { get; }
    void TakeDamage(int damage);
}
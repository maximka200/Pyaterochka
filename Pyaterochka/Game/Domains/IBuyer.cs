namespace Pyaterochka;

public interface IBuyer : ICreature
{
    public bool IsThief();
    public bool IsBanned { get; }
    public void Ban();
}
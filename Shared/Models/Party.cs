namespace Shared.Models;

public class Party
{
    public const int MaxPartySize = 2;
    private static int _nextPartyId = 1;
    public int Id { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();

    public Party()
    {
        Id = _nextPartyId++;
    }
}
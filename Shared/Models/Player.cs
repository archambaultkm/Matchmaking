namespace Shared.Models;

public class Player
{
    private static int _nextId = 1;
    public int Id { get; set; }
    public int Level { get; set; }
    public string Region { get; set; }
    
    public Player(int level, string region)
    {
        Id = _nextId++; // Assign a unique ID and increment the counter
        Level = level;
        Region = region;
    }
}
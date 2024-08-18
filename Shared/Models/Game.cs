namespace Shared.Models;

public class Game
{
    private Party _radiant;
    private Party _dire;

    public Game(Party radiant, Party dire)
    {
        _radiant = radiant;
        _dire = dire;
    }
}
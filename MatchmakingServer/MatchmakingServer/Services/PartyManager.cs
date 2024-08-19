using Shared.Models;

namespace Matchmaking.MatchmakingServer.Services;

public class PartyManager
{
    private readonly List<Party> _matchedParties = new List<Party>();

    public void AddParty(Party party)
    {
        _matchedParties.Add(party);
    }
    
    public bool HasMatchedParty(Player playerToCheck)
    {
        return _matchedParties.Any(party => party.Players.Any(player => player.Id == playerToCheck.Id));
    }

    public Party GetPlayerParty(Player player)
    {
        return _matchedParties.First(party => party.Players.Contains(player));
    }
}
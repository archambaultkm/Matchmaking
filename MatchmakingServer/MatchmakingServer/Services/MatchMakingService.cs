namespace Matchmaking.MatchmakingServer.Services;

using Shared.Models;

public class MatchMakingService
{
    private readonly List<Player> _queue  = new List<Player>();
    private List<Party> _matchedParties = new List<Party>();
    private const int MinPlayerCount = 2;
    
    public void Enqueue(Player player)
    {
        lock (_queue)
        {
            _queue.Add(player);
            Console.WriteLine($"Player {player.Id} connected. Total players: {_queue.Count}");

            if (_queue.Count >= MinPlayerCount)
            {
                TryMatchPlayers();
            }
        }
    }

    private void TryMatchPlayers()
    {
        lock (_queue)
        {
            foreach (var player in _queue.ToList())
            {
                var match = _queue
                    .Where(p => p != player)
                    .FirstOrDefault(p => IsMatch(player, p));

                if (match != null)
                {
                    // Create a party and add matched players
                    var matchedParty = new Party();
                    matchedParty.Players.Add(player);
                    matchedParty.Players.Add(match);

                    // Remove matched players from the queue
                    _queue.Remove(player);
                    _queue.Remove(match);

                    // Add the matched party to the list of matched parties
                    _matchedParties.Add(matchedParty);
                }
            }
        }
    }

    // Use LINQ to determine if a player has been matched into a party.
    public bool HasMatchedParty(Player playerToCheck)
    {
        return _matchedParties.Any(party => party.Players.Any(player => player.Id == playerToCheck.Id));
    }

    public Party GetPlayerParty(Player player)
    {
        return _matchedParties.First(party => party.Players.Contains(player));
    }

    private static bool IsMatch(Player p1, Player p2)
    {
        // @todo skill level should be 'in range' (defined somewhere) and region should be the same.
        // @todo there can be more leniency on region and range for fewer players.
        
        return true;
    }
}
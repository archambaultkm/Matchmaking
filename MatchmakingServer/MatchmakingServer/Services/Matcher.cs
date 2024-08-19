using Shared.Models;

namespace Matchmaking.MatchmakingServer.Services;

public class Matcher
{
    private const int _levelRange = 10; // allow difference of 10 levels between matched players.
    
    public Party? TryMatchPlayers(List<Player> queue)
    {
        lock (queue)
        {
            foreach (var player in queue.ToList())
            {
                var match = queue
                    .Where(p => p != player)
                    .FirstOrDefault(p => IsMatch(player, p));

                if (match != null)
                {
                    // Create a party and add matched players
                    var matchedParty = new Party();
                    matchedParty.Players.Add(player);
                    matchedParty.Players.Add(match);

                    // Remove matched players from the queue
                    queue.Remove(player);
                    queue.Remove(match);

                    return matchedParty;
                }
            }
        }

        return null;
    }
    
    private static bool IsMatch(Player p1, Player p2)
    {
        // @todo there can be more leniency on region and range when fewer players are queuing.
        return p1.Region == p2.Region && Math.Abs(p1.Level - p2.Level) <= _levelRange;
    }
}
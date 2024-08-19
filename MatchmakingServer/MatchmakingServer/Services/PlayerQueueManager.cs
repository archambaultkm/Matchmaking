using Shared.Models;

namespace Matchmaking.MatchmakingServer.Services;

public class PlayerQueueManager
{
    private readonly List<Player> _queue = new List<Player>();
    private const int _minQueueSize = 2;
    private readonly Matcher _matcher;
    private readonly PartyManager _partyManager;
    
    public PlayerQueueManager(Matcher matcher, PartyManager partyManager)
    {
        _matcher = matcher;
        _partyManager = partyManager;
    }

    public void Enqueue(Player player)
    {
        lock (_queue)
        {
            _queue.Add(player);
            Console.WriteLine($"Player {player.Id} connected. Total players: {_queue.Count}");

            if (_queue.Count >= _minQueueSize)
            {
                var party = _matcher.TryMatchPlayers(_queue);
                if (party != null)
                {
                    _partyManager.AddParty(party);
                    _queue.RemoveAll(p => party.Players.Contains(player));
                }
            }
        }
    }

    public bool HasMatchedParty(Player playerToCheck)
    {
        return _partyManager.HasMatchedParty(playerToCheck);
    }

    public Party GetPlayerParty(Player player)
    {
        return _partyManager.GetPlayerParty(player);
    }
}

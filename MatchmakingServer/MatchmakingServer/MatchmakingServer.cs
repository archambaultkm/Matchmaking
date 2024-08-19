using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Matchmaking.MatchmakingServer.Services;
using Shared.Interfaces;
using Shared.Models;
using Shared.Serialization;

namespace Matchmaking.MatchmakingServer;

public class MatchmakingServer
{
    private readonly TcpListener _listener;
    private readonly PlayerQueueManager _playerQueueManager;
    private readonly IPlayerSerializer _playerSerializer;
    private readonly IPartySerializer _partySerializer;
    private readonly ConcurrentDictionary<int, TcpClient> _connectedClients = new ConcurrentDictionary<int, TcpClient>();
    
    public MatchmakingServer(IPAddress serverIp, int port)
    {
        _playerSerializer = new PlayerSerializer();
        _partySerializer = new PartySerializer();
        _playerQueueManager = new PlayerQueueManager(new Matcher(), new PartyManager());
        _listener = new TcpListener(serverIp, port);
    }
    
    public void Start()
    {
        // Start the TcpListener.
        _listener.Start();
        Console.WriteLine("Server started. Waiting for players...");
        
        // @todo don't do a while true loop, find something better or at least an exit condition.
        while (true)
        {
            try
            {
                // Accept an incoming client connection.
                var client = _listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                // Handle client in a separate thread to allow concurrent connections.
                var clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        // ReSharper disable once FunctionNeverReturns
    }
    
    // Add client to the queue and communicate status back to them.
    private void HandleClient(TcpClient client)
    {
        try
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // Deserialize and add player
            var player = _playerSerializer.Deserialize(data);
            _connectedClients[player.Id] = client;
            // When added to the queue, the manager will attempt to find a match.
            _playerQueueManager.Enqueue(player);

            // If the newest player completed a team, output to server program.
            if (_playerQueueManager.HasMatchedParty(player))
            {
                var party = _playerQueueManager.GetPlayerParty(player);
                BroadcastMatchResult(party);
                Console.WriteLine($"Party {party.Id} is complete.");
            }
            // Server program output that more players are needed.
            else
            {
                string responseMessage = "Waiting for more players to join.";
                var responseData = Encoding.ASCII.GetBytes(responseMessage);
                Console.WriteLine(responseMessage);
                stream.Write(responseData, 0, responseData.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }
    }
    
    // When a party is completed, provide feedback to all players about their team.
    private void BroadcastMatchResult(Party party)
    {
        foreach (var player in party.Players)
        {
            if (_connectedClients.TryGetValue(player.Id, out var client))
            {
                // Send the player their party information.
                var stream = client.GetStream();
                var responseMessage = $"You have been matched into Party {_partySerializer.Serialize(party)}"; 
                var responseData = Encoding.ASCII.GetBytes(responseMessage);
                stream.Write(responseData, 0, responseData.Length);
                
                // Server feedback.
                Console.WriteLine($"Sent match results to player {player.Id}.");

                // Close the connection to the client after sending the response
                client.Close();
                _connectedClients.Remove(player.Id, out _);
            }
        }
    }
}
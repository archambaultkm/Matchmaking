using System;
using System.Net.Sockets;
using System.Text;
using Shared.Models;
using Shared.Serialization;

namespace MatchmakingClient.MatchmakingClient;

public class MatchmakingClient
{
    private readonly Player _player;
    private readonly PlayerSerializer _playerSerializer;
    private readonly TcpClient _client;
    
    public MatchmakingClient(int level)
    {
        _player = new Player(level, "NA");
        _playerSerializer = new PlayerSerializer();
        _client = new TcpClient();
    }
    
    public void Start(string serverIp, int port)
    {
        try
        {
            // Connect to the matchmaking server
            _client.Connect(serverIp, port);
            Console.WriteLine("Connected to server.");

            // Send player data to the server
            SendPlayerData();

            // Receive match results from the server
            ReceiveMatchResults();
        }
        catch (SocketException se)
        {
            Console.WriteLine($"SocketException: {se.Message}\nStack Trace: {se.StackTrace}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }
        finally
        {
            _client.Close();
        }
    }

    private void SendPlayerData()
    {
        var serializedPlayer = _playerSerializer.Serialize(_player);
        var stream = _client.GetStream();
        var data = Encoding.ASCII.GetBytes(serializedPlayer);
        stream.Write(data, 0, data.Length);
        Console.WriteLine("Player data sent to server.");
    }

    private void ReceiveMatchResults()
    {
        var buffer = new byte[1024];
        var stream = _client.GetStream();
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine(response);
        }
    }
}
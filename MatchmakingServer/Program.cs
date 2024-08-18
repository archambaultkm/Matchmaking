using System.Net;

namespace MatchmakingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start the matchmaking server.
            var server = new Matchmaking.MatchmakingServer.MatchmakingServer(IPAddress.Any, 5000);
            server.Start();
        }
    }
}
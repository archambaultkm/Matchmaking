namespace MatchmakingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MatchmakingClient.MatchmakingClient();
            client.Start("127.0.0.1", 5000);
        }
    }
}
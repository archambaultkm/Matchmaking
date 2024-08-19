namespace MatchmakingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var client = new MatchmakingClient.MatchmakingClient(int.Parse(args[0]));
                client.Start("127.0.0.1", 5000);
            }
            catch (FormatException)
            {
                Console.WriteLine("Enter a valid integer for the player level.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("The number is outside the bounds of an integer.");
            }
        }
    }
}
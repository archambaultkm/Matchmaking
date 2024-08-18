using Shared.Models;
using Shared.Interfaces;

namespace Shared.Serialization;

public class PlayerSerializer : IPlayerSerializer
{
    public string Serialize(Player player)
    {
        // Convert each property to a string and join them with a comma
        return $"{player.Id},{player.Level},{player.Region}";
    }

    public Player Deserialize(string data)
    {
        // Split the string by comma
        var parts = data.Split(',');
        
        // @todo validate data

        // Parse each part and create a new Player object
        return new Player(int.Parse(parts[1]), parts[2]);
    }
}
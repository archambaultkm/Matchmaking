using Shared.Models;

namespace Shared.Interfaces;

public interface IPlayerSerializer
{
    string Serialize(Player player);
    Player Deserialize(string data);
}
using Shared.Models;

namespace Shared.Interfaces;

public interface IPartySerializer
{
    string Serialize(Party party);
    Party Deserialize(string data);
}
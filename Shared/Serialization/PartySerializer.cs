using System.Text.Json;
using Shared.Interfaces;
using Shared.Models;

namespace Shared.Serialization;

public class PartySerializer : IPartySerializer
{
    // Serialize a Party object to a JSON string
    public string Serialize(Party party)
    {
        // Serialize the Party object to a JSON string
        return party.Id.ToString();
    }

    // Deserialize a JSON string to a Party object
    public Party Deserialize(string json)
    {
        // Deserialize the JSON string to a Party object
        return JsonSerializer.Deserialize<Party>(json);
    }
}
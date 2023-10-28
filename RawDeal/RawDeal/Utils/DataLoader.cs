using System.Text.Json;

namespace RawDeal;

public abstract class DataLoader
{
    public static List<Superstar> LoadSuperstarData()
    {
        string superstarJsonData = File.ReadAllText(Path.Combine("data", "superstar.json"));
        return JsonSerializer.Deserialize<List<Superstar>>(superstarJsonData);
    }
}
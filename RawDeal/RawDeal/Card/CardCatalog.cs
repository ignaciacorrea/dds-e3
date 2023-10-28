using System.Text.Json;

namespace RawDeal;

public class CardCatalog
{
    private Dictionary <string , CardInfo > _cardInfo;
    
    public CardCatalog() 
    {
        _cardInfo = new Dictionary <string , CardInfo >();
        foreach (var cardInfo in ReadCardInfos())
        {
            _cardInfo[cardInfo.Title] = cardInfo;
        } 
    }
    
    private static CardInfo[] ReadCardInfos() 
    {
        string fileName = Path.Combine("data", "cards.json");
        string jsonString = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize <CardInfo[]>(jsonString);
    }
    
    public Card GetCard(string cardTitle) => new Card(_cardInfo[cardTitle]);
}
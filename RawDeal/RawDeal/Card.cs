namespace RawDeal;

public class Card
{
    private CardInfo _cardInfo;
    
    public Card(CardInfo cardInfo) => _cardInfo = cardInfo;
    public int GetDamage () => _cardInfo.Damage!="#" ? Convert.ToInt32(_cardInfo.Damage) : 0;
    public List<string> GetSubtypes () => _cardInfo.Subtypes;
    public string GetTitle () => _cardInfo.Title;
    public List<string> GetTypes () => _cardInfo.Types;
    public CardInfo GetCardInfo () => _cardInfo;
    public int GetFortitude () => _cardInfo.Fortitude!="#" ? Convert.ToInt32(_cardInfo.Fortitude) : 0;
}
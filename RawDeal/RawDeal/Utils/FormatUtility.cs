using RawDealView.Formatters;

namespace RawDeal;

public abstract class FormatUtility
{
    public static List<string> FormatCardsPlayerCanPlay(List<Card> cards)
    {
        List<string> result = new List<string>();
        foreach (Card card in cards)
        {
            ProcessCard(result, card);
        }
        return result;
    }

    
    private static void ProcessCard(List<string> result, Card card)
    {
        foreach (string type in card.GetTypes())
        {
            if (IsPlayableType(type))
            {
                PlayInfo playInfo = CreatePlayInfo(card, type);
                result.Add(Formatter.PlayToString(playInfo));
            }
        }
    }
    
    private static bool IsPlayableType(string type)
    {
        return type is "MANEUVER" or "ACTION";
    }
    
    private static PlayInfo CreatePlayInfo(Card card, string type)
    {
        return new PlayInfo { CardInfo = card.GetCardInfo(), PlayedAs = type.ToUpper() };
    }
}
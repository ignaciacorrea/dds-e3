using RawDealView.Formatters;

namespace RawDeal;

public abstract class FormatUtility
{
    // clean code!!!!! card.GetCardInfo??? y muchos puntos
    public static List<string> FormatCardsPlayerCanPlay(List<Card> cardsUserCanPlay)
    {
        return cardsUserCanPlay
            .SelectMany(card => card.GetTypes(), (card, type) => new PlayInfo { CardInfo = card.GetCardInfo(), PlayedAs = type.ToUpper() })
            .Where(playInfo => playInfo.PlayedAs is "MANEUVER" or "ACTION")
            .Select(Formatter.PlayToString)
            .ToList();
    }


    public static List<string> FormatCardsToDisplay(List<Card> cardsToDisplay)
    {
        return cardsToDisplay.Select(card => Formatter.CardToString(card.GetCardInfo())).ToList();
    }
}
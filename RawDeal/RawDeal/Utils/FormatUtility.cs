using RawDealView.Formatters;

namespace RawDeal;

public abstract class FormatUtility
{
    // clean code!!!!! card.GetCardInfo??? y muchos puntos
    public static List<string> FormatCardsPlayerCanPlay(IEnumerable<Card> cardsUserCanPlay)
    {
        IEnumerable<PlayInfo> x = cardsUserCanPlay
            .SelectMany(card => card.GetTypes(),
                (card, type) => new PlayInfo { CardInfo = card.GetCardInfo(), PlayedAs = type.ToUpper() });
        return x.Where(playInfo => playInfo.PlayedAs is "MANEUVER" or "ACTION")
            .Select(Formatter.PlayToString)
            .ToList();
    }


    public static List<string> FormatCardsToDisplay(IEnumerable<Card> cardsToDisplay)
    {
        //TODO: Split train wreck
        IEnumerable<string> cards = cardsToDisplay.Select(card => Formatter.CardToString(card.GetCardInfo()));
        return new List<string>(cards);
    }
}
namespace RawDeal;

public abstract class CardDeckInfoProvider
{
    public static int GetDeckLength(List<Card> deck)
    {
        return deck.Count;
    }

    public static Card GetLastCardOfDeck(List<Card> deck)
    {
        return deck.Last();
    }

    public static bool CheckIfDeckIsEmpty(List<Card> deck)
    {
        return deck.Count == 0;
    }
    
    public static bool CheckIfDeckHasAnAmountOfCards(List<Card> deck, int amountOfCards)
    {
        return deck.Count >= amountOfCards;
    }
}
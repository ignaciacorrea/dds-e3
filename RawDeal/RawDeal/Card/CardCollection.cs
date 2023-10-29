using RawDealView.Formatters;

namespace RawDeal;

public class CardCollection
{
    private readonly List<Card> _cards;
    
    public CardCollection()
    {
        _cards = new List<Card>();
    }

    public CardCollection(IEnumerable<Card> cards)
    {
        _cards = new List<Card>(cards);
    }
    
    public void GiveCardToTopOf(CardCollection destination) => GiveCardTo(GetTopCard(), destination, true);
    
    public Card GetTopCard() => _cards.Last();

    public void GiveSpecificCardToTopOf(CardCollection destination, int indexSelectedCard)
    {
        GiveCardTo(GetSpecificCard(indexSelectedCard), destination, true);
    }

    public void GiveSpecificCardToBottomOf(CardCollection destination, int indexSelectedCard)
    {
        GiveCardTo(GetSpecificCard(indexSelectedCard), destination, false);
    }

    public Card GetSpecificCard(int indexSelectedCard) => _cards[indexSelectedCard];
    
    private void GiveCardTo(Card card, CardCollection destination, bool addToTop)
    {
        if (addToTop)
        {
            destination.Add(card);
        }
        else
        {
            destination.Insert(0, card);
        }
        _cards.Remove(card);
    }

    public void Add(Card card) => _cards.Add(card);

    private void Insert(int index, Card card) => _cards.Insert(index, card);
    
    public int GetLength()
    {
        return _cards.Count;
    }

    public bool CheckIfIsEmpty()
    {
        return _cards.Count == 0;
    }
    
    public bool CheckIfHasAnAmountOfCards(int amountOfCards)
    {
        return _cards.Count >= amountOfCards;
    }

    public List<string> FormatCardsToDisplay()
    {
        List<string> formattedCards = new List<string>();
        foreach (Card card in _cards)
        {
            string cardString = Formatter.CardToString(card.GetCardInfo());
            formattedCards.Add(cardString);
        }
        return formattedCards;
    }
}
namespace RawDeal;

public class DeckValidator
{
    private readonly IEnumerable<Card> _playerDeck;
    private readonly Superstar _playerSuperstar;
    private readonly List<Superstar> _listOfSuperstars;
    private const int DeckLength = 60;
    private const int MaxNonSetupCards = 3;

    public DeckValidator(IEnumerable<Card> playerDeck, Superstar playerSuperstar, List<Superstar> listOfSuperstars)
    {
        _playerDeck = playerDeck;
        _playerSuperstar = playerSuperstar;
        _listOfSuperstars = listOfSuperstars;
    }
    
    public bool CheckIfDeckIsValid()
    {
        return (CheckDeckLength() &&
                CheckDeckMaxThreeNonSetupCards() &&
                CheckDeckMaxOneUniqueCard() &&
                CheckDeckOnlyHeelOrFace() &&
                CheckDeckForForeignSuperstarLogo());
    }

    private bool CheckDeckLength()
    {
        return _playerDeck.Count() == DeckLength;
    }
    
    private bool CheckDeckMaxThreeNonSetupCards()
    {
        IEnumerable<Card> cardsExceedingLimit = GetCardsExceedingLimit(MaxNonSetupCards);
        return cardsExceedingLimit.All(card => CheckSubtypeInCard(card, "SetUp"));
    }
    
    private bool CheckDeckMaxOneUniqueCard()
    {
        IEnumerable<Card> cardsExceedingLimit = GetCardsExceedingLimit(1);
        return !cardsExceedingLimit.Any(card => CheckSubtypeInCard(card, "Unique"));
    }
    
    private IEnumerable<Card> GetCardsExceedingLimit(int length)
    {
        var cardsGroupedByTitle = _playerDeck.GroupBy(card => card.GetTitle());
        var groupsExceedingLimit = cardsGroupedByTitle.Where(group => group.Count() > length);
        return groupsExceedingLimit.Select(group => group.First());
    }
    
    private bool CheckDeckOnlyHeelOrFace()
    {
        bool hasHeel = CheckSubtypeInDeck("Heel");
        bool hasFace = CheckSubtypeInDeck("Face");
        return !(hasHeel && hasFace);
    }
    
    private bool CheckSubtypeInDeck(string subtypeToCheck)
    {
        return _playerDeck.Any(card => CheckSubtypeInCard(card, subtypeToCheck));
    }
    
    private static bool CheckSubtypeInCard(Card card, string subtypeToCheck)
    {
        List<string> cardSubtypes = card.GetSubtypes();
        return cardSubtypes.Contains(subtypeToCheck);
    }
    
    private bool CheckDeckForForeignSuperstarLogo()
    {
        List<string> foreignSuperstarsLogos = _listOfSuperstars
            .Where(superstar => superstar.Logo != _playerSuperstar.Logo)
            .Select(superstar => superstar.Logo).ToList();
        return !_playerDeck.Any(card => CheckCardForForeignSuperstarLogo(card, foreignSuperstarsLogos));
    }
    
    private static bool CheckCardForForeignSuperstarLogo(Card card, List<string>foreignSuperstarsLogos)
    {
        List<string> cardSubtypes = card.GetSubtypes();
        return cardSubtypes.Any(foreignSuperstarsLogos.Contains);
    }
}
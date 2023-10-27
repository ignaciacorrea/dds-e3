namespace RawDeal;

public class Player
{
    private readonly Superstar _superstar;
    private CardCollection _hand;
    private readonly CardCollection _arsenal;
    private CardCollection _ringside;
    private CardCollection _ringArea;
    private int _fortitudeRating;
    private bool _usedAbility;
    private bool _superstarAbilityIsEnabled;
    
    public Player(Superstar superstar, List<Card> arsenal)
    {
        InitializeLists();
        _superstar = superstar;
        _arsenal = new CardCollection(arsenal);
        _fortitudeRating = 0;
        _usedAbility = false;
        StealStartingHand();
    }

    private void InitializeLists()
    {
        _hand = new CardCollection(new List<Card>());
        _ringside = new CardCollection(new List<Card>());
        _ringArea = new CardCollection(new List<Card>());
    }
    
    public string GetSuperstarName() {return _superstar.Name;}

    public string GetSuperstarAbility() {return _superstar.SuperstarAbility;}
    
    public int GetSuperstarValue() {return _superstar.SuperstarValue;}
    
    public List<Card> GetHand() {return _hand.GetCards();}
    
    public List<Card> GetRingside() {return _ringside.GetCards();}
    
    public List<Card> GetRingArea() {return _ringArea.GetCards();}
    
    public List<Card> GetArsenal() {return _arsenal.GetCards();}

    public int GetFortitudeRating() {return _fortitudeRating;}

    public bool GetUsedAbility() {return _usedAbility;}
    
    public bool GetIfSuperstarAbilityIsEnable() {return _superstarAbilityIsEnabled;}
    
    public void ManageIfSuperstarAbilityIsEnabled(bool value) {_superstarAbilityIsEnabled = value;}
    
    private void StealStartingHand()
    {
        for (int i = 0; i < _superstar.HandSize; i++)
        {
            DrawCardFromArsenal();
        }
    }

    public void DrawCardFromArsenal()
    {
        _arsenal.GiveCardToTopOf(_hand);
    }

    public void DiscardCardToRingside(int indexSelectedCard)
    {
        _hand.GiveSpecificCardToTopOf(_ringside, indexSelectedCard);
    }
    
    public void DiscardCardToArsenal(int indexSelectedCard)
    {
        _hand.GiveSpecificCardToBottomOf(_arsenal, indexSelectedCard);
    }
    
    public void DiscardCardToRingArea(int indexSelectedCard)
    {
        Card cardAddedRingArea = _hand.GetSpecificCard(indexSelectedCard);
        _fortitudeRating += cardAddedRingArea.GetDamage();   
        _hand.GiveSpecificCardToTopOf(_ringArea, indexSelectedCard);
    }

    public void RecoverCardToHandFromRingside(int indexSelectedCard)
    {
        _ringside.GiveSpecificCardToTopOf(_hand, indexSelectedCard);
    }
    
    public void RecoverCardToArsenalFromRingside(int indexSelectedCard)
    {
        _ringside.GiveSpecificCardToTopOf(_arsenal, indexSelectedCard);
    }
    
    public void ReceiveDamage()
    {
        _arsenal.GiveCardToTopOf(_ringside);
    }
}
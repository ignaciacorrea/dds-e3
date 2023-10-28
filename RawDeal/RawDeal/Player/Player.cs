namespace RawDeal;

public class Player
{
    public Superstar Superstar;
    public CardCollection Hand;
    public CardCollection Arsenal;
    public CardCollection Ringside;
    public CardCollection RingArea;
    public int FortitudeRating;
    public SuperstarAbility SuperstarAbility;

    public Player(Superstar superstar, IEnumerable<Card> arsenal)
    {
        Superstar = superstar;
        Arsenal = new CardCollection(arsenal);
        Hand = new CardCollection();
        Ringside = new CardCollection();
        RingArea = new CardCollection();
    }
    public string GetSuperstarName() {return Superstar.Name;}
    public string GetStringSuperstarAbility() {return Superstar.SuperstarAbility;}
    public int GetSuperstarValue() {return Superstar.SuperstarValue;}
    public List<Card> GetHand() {return Hand.GetCards();}
    public List<Card> GetArsenal() {return Arsenal.GetCards();}
    public List<Card> GetRingside() {return Ringside.GetCards();}
    public List<Card> GetRingArea() {return RingArea.GetCards();}
}
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
    public int GetFortitudeRating() {return FortitudeRating;}
}
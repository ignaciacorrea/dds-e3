using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class ShowCardController
{
    private readonly Player _player;
    private readonly Player _opponent;
    private readonly View _view;
    
    public ShowCardController(Player player, Player opponent, View view)
    {
        _player = player;
        _opponent = opponent;
        _view = view;
    }
    
    public void Execute()
    {
        CardSet deckUserWantsToSee = _view.AskUserWhatSetOfCardsHeWantsToSee();
        CardCollection cardsToDisplay = GetCardsForDeck(deckUserWantsToSee);
        _view.ShowCards(cardsToDisplay.FormatCardsToDisplay());
    }
    
    private CardCollection GetCardsForDeck(CardSet cardSet) =>
        cardSet switch
        {
            CardSet.Hand => _player.Hand,
            CardSet.RingArea => _player.RingArea,
            CardSet.RingsidePile => _player.Ringside,
            CardSet.OpponentsRingArea => _opponent.RingArea,
            CardSet.OpponentsRingsidePile => _opponent.Ringside,
            _ => new CardCollection()
        };
}
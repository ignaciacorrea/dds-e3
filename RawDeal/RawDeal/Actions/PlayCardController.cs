using RawDealView;

namespace RawDeal;

public class PlayCardController
{
    private readonly Player _player;
    private readonly Player _opponent;
    private readonly View _view;
    
    public PlayCardController(Player player, Player opponent, View view)
    {
        _player = player;
        _opponent = opponent;
        _view = view;
    }
    
    private void PlayCard()
    {
        List<Card> playableCards = GetPlayableCards();
        List<String> formattedPlayableCards = FormatUtility.FormatCardsPlayerCanPlay(playableCards);
        int indexSelectedCard = _view.AskUserToSelectAPlay(formattedPlayableCards);
        
        String formattedSelectedCard = formattedPlayableCards[indexSelectedCard];
        CardInfo selectedCardInfo = playableCards[indexSelectedCard].GetCardInfo();
        if (indexSelectedCard != -1)
        {
            PlaySelectedCard(cardsAndTheirIndexInTheHandPlayerCanPlay.Keys.ElementAt(id), 
                selectedCardInfo, 
                formattedSelectedCard);
        }
    }


    private List<Card> GetPlayableCards()
    {
        List<Card> playableCards = new List<Card>();
        for (int index = 0; index < _player.Hand.GetLength(); index++)
        {
            Card card = _player.Hand.GetSpecificCard(index);
            if (card.GetFortitude() <= _player.GetFortitudeRating())
            {
                playableCards.Add(card);
            }
        }
        return playableCards;
    }


    private void PlaySelectedCard(int indexSelectedCard, CardInfo selectedCardInfo, String formattedSelectedCard)
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(_player.GetSuperstarName(), formattedSelectedCard);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        
        int totalDamage = int.Parse(selectedCardInfo.Damage);
        if (_player.GetSuperstarName() == "MANKIND") totalDamage -= 1;
        _view.SayThatSuperstarWillTakeSomeDamage(_player.GetSuperstarName(), totalDamage);
        MakeDamage(totalDamage);
        _currentPlayerController.DiscardCardToRingArea(indexSelectedCard);
    }
}
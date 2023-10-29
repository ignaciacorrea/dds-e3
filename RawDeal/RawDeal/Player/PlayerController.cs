using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class PlayerController
{
    private Player _player;
    private Player _opponent;
    private PlayerController _opponentController;
    private SuperstarAbility _superstarAbility;
    private ActionController _actionController;
    private readonly View _view;
    
    public PlayerController(Player player, Player opponent, View view)
    {
        _player = player;
        _opponent = opponent;
        _opponentController = new PlayerController(_opponent, _player, view);
        _superstarAbility = _player.SuperstarAbility;
        _actionController = new ActionController(this, _opponentController, view);
        _view = view;
    }

    public void StartTurn()
    {
        _view.SayThatATurnBegins(_player.GetSuperstarName());
        _superstarAbility.ApplyBeforeDrawing(_opponent);
        DrawCardFromArsenal();
    }
    
    public NextPlay AskWhatToDo()
    {
        bool abilityCanBeUsed= _superstarAbility.CheckIfAbilityCanBeUsed();
        NextPlay playerActionRequest = abilityCanBeUsed ? _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible() : _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        return playerActionRequest;
    }
    
    
    public void MakeDamage(int totalDamage)
    {
        for (int currentDamage = 1; currentDamage <= totalDamage; currentDamage++)
        {
            if (CheckPinVictory()) break;
            Card lastCardOfDeck = _opponent.Arsenal.GetTopCard();
            CardInfo infoLastCardOfDeck = lastCardOfDeck.GetCardInfo();
            string stringLastCardOfDeck = Formatter.CardToString(infoLastCardOfDeck); 
            _view.ShowCardOverturnByTakingDamage(stringLastCardOfDeck, currentDamage, totalDamage);
            _opponentController.ReceiveDamage();
        }
    }
    
    
    private bool CheckPinVictory()
    {
        if (!_opponent.Arsenal.CheckIfIsEmpty()) return false;
        _view.CongratulateWinner(_player.GetSuperstarName());
        _gameIsOn = false;
        return true;
    }
    
    
    public ShowCardController BuildShowCardController()
    {
        return new ShowCardController(_player, _opponent, _view);
    }
    
    public void StealStartingHand()
    {
        for (int i = 0; i < _player.Superstar.HandSize; i++)
        {
            DrawCardFromArsenal();
        }
    }
    
    public void DrawCardFromArsenal()
    {
        _player.Arsenal.GiveCardToTopOf(_player.Hand);
    }

    public void DiscardCardToRingside(int indexSelectedCard)
    {
        _player.Hand.GiveSpecificCardToTopOf(_player.Ringside, indexSelectedCard);
    }
    
    public void DiscardCardToArsenal(int indexSelectedCard)
    {
        _player.Hand.GiveSpecificCardToBottomOf(_player.Arsenal, indexSelectedCard);
    }
    
    public void DiscardCardToRingArea(int indexSelectedCard)
    {
        Card cardAddedRingArea = _player.Hand.GetSpecificCard(indexSelectedCard);
        _player.FortitudeRating += cardAddedRingArea.GetDamage();   
        _player.Hand.GiveSpecificCardToTopOf(_player.RingArea, indexSelectedCard);
    }

    public void RecoverCardToHandFromRingside(int indexSelectedCard)
    {
        _player.Ringside.GiveSpecificCardToTopOf(_player.Hand, indexSelectedCard);
    }
    
    public void RecoverCardToArsenalFromRingside(int indexSelectedCard)
    {
        _player.Ringside.GiveSpecificCardToTopOf(_player.Arsenal, indexSelectedCard);
    }
    
    public void ReceiveDamage()
    {
        _player.Arsenal.GiveCardToTopOf(_player.Ringside);
    }
}
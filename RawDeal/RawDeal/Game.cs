using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private Player _currentPlayer;
    private Player _otherPlayer;
    private bool _gameIsOn = true;
    
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
    }

    
    public void Play()
    {
        try
        {
            PlayGame();
        }
        catch (Exception ex)
        {
            _view.SayThatDeckIsInvalid();
        }
    }


    private void PlayGame()
    {
        CreatePlayers();
        SetCurrentPlayerBasedOnTheSuperstarValue();
        StartTurn();
        
        while(_gameIsOn)
        {
            ShowGameInfo();
            NextPlay playerActionRequest = AskWhatToDo();
            HandleOption(playerActionRequest);
        }
    }
    
    
    private void CreatePlayers()
    {
        PlayerCreator playerCreator = new PlayerCreator(_view, _deckFolder);
        _currentPlayer = playerCreator.SetUpPlayer();
        _otherPlayer = playerCreator.SetUpPlayer();
    }


    private void SetCurrentPlayerBasedOnTheSuperstarValue()
    {
        if (_currentPlayer.GetSuperstarValue() < _otherPlayer.GetSuperstarValue())
        {
            SwapPlayers();
        }
    }
    
    
    private void StartTurn()
    {
        _view.SayThatATurnBegins(_currentPlayer.GetSuperstarName());
        
        // --- Manejar habilidades --- //
        if (_currentPlayer.GetSuperstarName() == "THE ROCK" && !CardDeckInfoProvider.CheckIfDeckIsEmpty(_currentPlayer.GetRingside())) HandleTheRockAbility();
        if (_currentPlayer.GetSuperstarName() == "KANE") HandleKaneAbility();
        if (_currentPlayer.GetSuperstarName() == "MANKIND" && CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_currentPlayer.GetArsenal(), 2)) _currentPlayer.DrawCardFromArsenal();;
        // --- --- --- --- --- --- --- //
        
        _currentPlayer.DrawCardFromArsenal();
        ActivateSuperstarsAbilities();
    }


    // if dentro de switch??? numero
    private void ActivateSuperstarsAbilities()
    {
        switch (_currentPlayer.GetSuperstarName())
        {
            case "CHRIS JERICHO":
                if (!CardDeckInfoProvider.CheckIfDeckIsEmpty(_currentPlayer.GetHand())) _currentPlayer.ManageIfSuperstarAbilityIsEnabled(true);
                break;
            case "THE UNDERTAKER":
                if (CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_currentPlayer.GetHand(), 2)) _currentPlayer.ManageIfSuperstarAbilityIsEnabled(true);
                break;
            case "STONE COLD STEVE AUSTIN":
                _currentPlayer.ManageIfSuperstarAbilityIsEnabled(true);
                break;
        }
    }


    private void HandleTheRockAbility()
    {
        if (!_view.DoesPlayerWantToUseHisAbility(_currentPlayer.GetSuperstarName())) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_currentPlayer.GetSuperstarName(), _currentPlayer.GetSuperstarAbility());
        int indexSelectedCard = _view.AskPlayerToSelectCardsToRecover(_currentPlayer.GetSuperstarName(), 1, FormatUtility.FormatCardsToDisplay(_currentPlayer.GetRingside()));
        _currentPlayer.RecoverCardToArsenalFromRingside(indexSelectedCard);
    }


    private void HandleKaneAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(_currentPlayer.GetSuperstarName(), _currentPlayer.GetSuperstarAbility());
        _view.SayThatSuperstarWillTakeSomeDamage(_otherPlayer.GetSuperstarName(), 1);
        MakeDamage(1);
    }


    private void ShowGameInfo()
    {
        PlayerInfo currentPlayerInfo = new PlayerInfo(_currentPlayer.GetSuperstarName(), _currentPlayer.GetFortitudeRating(), CardDeckInfoProvider.GetDeckLength(_currentPlayer.GetHand()), CardDeckInfoProvider.GetDeckLength(_currentPlayer.GetArsenal()));
        PlayerInfo otherPlayerInfo = new PlayerInfo(_otherPlayer.GetSuperstarName(), _otherPlayer.GetFortitudeRating(), CardDeckInfoProvider.GetDeckLength(_otherPlayer.GetHand()), CardDeckInfoProvider.GetDeckLength(_otherPlayer.GetArsenal()));
        _view.ShowGameInfo(currentPlayerInfo, otherPlayerInfo);
    }


    private NextPlay AskWhatToDo()
    {
        CheckSuperstarAbilityEnabled();
        NextPlay playerActionRequest = _currentPlayer.GetIfSuperstarAbilityIsEnable() ? _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible() : _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        return playerActionRequest;
    }


    // numero
    private void CheckSuperstarAbilityEnabled()
    {
        if (_currentPlayer.GetSuperstarName() == "CHRIS JERICHO" && CardDeckInfoProvider.CheckIfDeckIsEmpty(_currentPlayer.GetHand())) _currentPlayer.ManageIfSuperstarAbilityIsEnabled(false);
        if (_currentPlayer.GetSuperstarName() == "THE UNDERTAKER" && !CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_currentPlayer.GetHand(), 2)) _currentPlayer.ManageIfSuperstarAbilityIsEnabled(false);
    }


    private void HandleOption(NextPlay playerRequest)
    {
        switch (playerRequest)
        {
            case NextPlay.ShowCards:
                ShowCardsToUser();
                break;
            case NextPlay.PlayCard:
                PlayCard();
                break;
            case NextPlay.UseAbility:
                _currentPlayer.ManageIfSuperstarAbilityIsEnabled(false);
                HandleSuperstarsAbilities();
                break;
            case NextPlay.EndTurn:
                EndTurn();
                break;
            case NextPlay.GiveUp:
                _view.CongratulateWinner(_otherPlayer.GetSuperstarName());
                _gameIsOn = false;
                break;
        }
    }


    private void HandleSuperstarsAbilities()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(_currentPlayer.GetSuperstarName(), _currentPlayer.GetSuperstarAbility());
        _currentPlayer.ManageIfSuperstarAbilityIsEnabled(false);
        switch (_currentPlayer.GetSuperstarName())
        {
            case "CHRIS JERICHO":
                HandleChrisJerichoAbility();
                break;
            case "THE UNDERTAKER":
                HandleTheUndertakerAbility();
                break;
            case "STONE COLD STEVE AUSTIN":
                HandleStoneColdSteveAustinAbility();
                break;
        }
    }


    private void HandleChrisJerichoAbility()
    {
        PromptPlayerToDiscardCardsToRingside(1);
        SwapPlayers();
        PromptPlayerToDiscardCardsToRingside(1);
        SwapPlayers();
    }


    private void HandleStoneColdSteveAustinAbility()
    {
        _currentPlayer.DrawCardFromArsenal();
        PromptPlayerToDiscardCardsToArsenal();
    }


    // numero 2 y 1
    private void HandleTheUndertakerAbility()
    {
        PromptPlayerToDiscardCardsToRingside(2);
        int indexSelectedCardTake = _view.AskPlayerToSelectCardsToPutInHisHand(_currentPlayer.GetSuperstarName(), 1, FormatUtility.FormatCardsToDisplay(_currentPlayer.GetRingside()));
        _currentPlayer.RecoverCardToHandFromRingside(indexSelectedCardTake);
    }
    
    
    private void PromptPlayerToDiscardCardsToRingside(int totalCardsToDiscard)
    {
        for (int numberOfCardToDiscard = totalCardsToDiscard; numberOfCardToDiscard > 0; numberOfCardToDiscard--)
        {
            int indexSelectedCard = _view.AskPlayerToSelectACardToDiscard(FormatUtility.FormatCardsToDisplay(_currentPlayer.GetHand()), _currentPlayer.GetSuperstarName(), _currentPlayer.GetSuperstarName(), numberOfCardToDiscard);
            _currentPlayer.DiscardCardToRingside(indexSelectedCard);
        }
    }
    
    
    private void PromptPlayerToDiscardCardsToArsenal()
    {
        _view.SayThatPlayerDrawCards(_currentPlayer.GetSuperstarName(), 1);
        int indexSelectedCard = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(_currentPlayer.GetSuperstarName(), FormatUtility.FormatCardsToDisplay(_currentPlayer.GetHand()));
        _currentPlayer.DiscardCardToArsenal(indexSelectedCard);
    }
    
    
    private void EndTurn()
    {
        if(CheckCountOutVictory()) return;
        SwapPlayers();
        if(CheckCountOutVictory()) return;
        StartTurn();
    }
    

    private bool CheckCountOutVictory()
    {
        if (!CardDeckInfoProvider.CheckIfDeckIsEmpty(_currentPlayer.GetArsenal())) return false;
        _view.CongratulateWinner(_otherPlayer.GetSuperstarName());
        _gameIsOn = false;
        return true;
    }


    private void SwapPlayers()
    {
        Player temporaryPlayer = _currentPlayer;
        _currentPlayer = _otherPlayer;
        _otherPlayer = temporaryPlayer;
    }


    private void ShowCardsToUser()
    {
        CardSet deckUserWantsToSee = _view.AskUserWhatSetOfCardsHeWantsToSee();
        List<Card> cardsToDisplay = GetCardsForDeck(deckUserWantsToSee);
        _view.ShowCards(FormatUtility.FormatCardsToDisplay(cardsToDisplay));
    }
    
    
    private List<Card> GetCardsForDeck(CardSet cardset) =>
        cardset switch
        {
            CardSet.Hand => _currentPlayer.GetHand(),
            CardSet.RingArea => _currentPlayer.GetRingArea(),
            CardSet.RingsidePile => _currentPlayer.GetRingside(),
            CardSet.OpponentsRingArea => _otherPlayer.GetRingArea(),
            CardSet.OpponentsRingsidePile => _otherPlayer.GetRingside(),
            _ => new List<Card>()
        };


    // clean code!!!!!
    private void PlayCard()
    {
        Dictionary<int, Card> cardsAndTheirIndexInTheHandPlayerCanPlay =
            _currentPlayer.GetHand()
                .Select((card, index) => new { Index = index, Card = card })
                .Where(item => item.Card.GetFortitude() <= _currentPlayer.GetFortitudeRating())
                .ToDictionary(item => item.Index, item => item.Card);
        List<Card> cardsPlayerCanPlay = cardsAndTheirIndexInTheHandPlayerCanPlay.Values.ToList();
        List<String> formattedCardsPlayerCanPlay = FormatUtility.FormatCardsPlayerCanPlay(cardsPlayerCanPlay);
        int id = _view.AskUserToSelectAPlay(formattedCardsPlayerCanPlay);
        if (id != -1) PlaySelectedCard(cardsAndTheirIndexInTheHandPlayerCanPlay.Keys.ElementAt(id), cardsPlayerCanPlay[id].GetCardInfo(), formattedCardsPlayerCanPlay[id]);
    }


    private void PlaySelectedCard(int indexSelectedCard, CardInfo selectedCardInfo, String formattedSelectedCard)
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(_currentPlayer.GetSuperstarName(), formattedSelectedCard);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        int totalDamage = int.Parse(selectedCardInfo.Damage);
        if (_otherPlayer.GetSuperstarName() == "MANKIND") totalDamage -= 1;
        _view.SayThatSuperstarWillTakeSomeDamage(_otherPlayer.GetSuperstarName(), totalDamage);
        MakeDamage(totalDamage);
        _currentPlayer.DiscardCardToRingArea(indexSelectedCard);
    }


    private void MakeDamage(int totalDamage)
    {
        for (int currentDamage = 1; currentDamage <= totalDamage; currentDamage++)
        {
            if (CheckPinVictory()) break;
            Card lastCardOfDeck = CardDeckInfoProvider.GetLastCardOfDeck(_otherPlayer.GetArsenal());
            CardInfo infoLastCardOfDeck = lastCardOfDeck.GetCardInfo();
            string stringLastCardOfDeck = Formatter.CardToString(infoLastCardOfDeck); 
            _view.ShowCardOverturnByTakingDamage(stringLastCardOfDeck, currentDamage, totalDamage);
            _otherPlayer.ReceiveDamage();
        }
    }
    
    
    private bool CheckPinVictory()
    {
        if (!CardDeckInfoProvider.CheckIfDeckIsEmpty(_otherPlayer.GetArsenal())) return false;
        _view.CongratulateWinner(_currentPlayer.GetSuperstarName());
        _gameIsOn = false;
        return true;
    }
}
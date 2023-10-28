using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private Player _currentPlayer;
    private Player _opponentPlayer;
    private PlayerController _currentPlayerController;
    private PlayerController _opponentPlayerController;
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
        catch (InvalidDeckException ex)
        {
            _view.SayThatDeckIsInvalid();
        }
    }


    private void PlayGame()
    {
        CreatePlayers();
        SetCurrentPlayerBasedOnTheSuperstarValue();
        StartTurn();
        LoopGame();
    }


    private void CreatePlayers()
    {
        PlayerCreator playerCreator = new PlayerCreator(_view, _deckFolder);
        _currentPlayer = playerCreator.SetUpPlayer();
        _opponentPlayer = playerCreator.SetUpPlayer();
        _currentPlayerController = new PlayerController(_currentPlayer, _opponentPlayer, _view);
        _opponentPlayerController = new PlayerController(_opponentPlayer, _currentPlayer, _view);
    }


    private void SetCurrentPlayerBasedOnTheSuperstarValue()
    {
        if (_currentPlayer.GetSuperstarValue() < _opponentPlayer.GetSuperstarValue())
        {
            SwapPlayers();
        }
    }
    
    
    private void LoopGame()
    {
        while(_gameIsOn)
        {
            ShowGameInfo();
            NextPlay playerActionRequest = AskWhatToDo();
            HandleOption(playerActionRequest);
        }
    }
    
    
    private void ShowGameInfo()
    {
        PlayerInfo currentPlayerInfo = new PlayerInfo(_currentPlayerController.GetSuperstarName(), _currentPlayerController.GetFortitudeRating(), CardDeckInfoProvider.GetDeckLength(_currentPlayerController.GetHand()), CardDeckInfoProvider.GetDeckLength(_currentPlayerController.GetArsenal()));
        PlayerInfo opponentPlayerInfo = new PlayerInfo(_opponentPlayerController.GetSuperstarName(), _opponentPlayerController.GetFortitudeRating(), CardDeckInfoProvider.GetDeckLength(_opponentPlayerController.GetHand()), CardDeckInfoProvider.GetDeckLength(_opponentPlayerController.GetArsenal()));
        _view.ShowGameInfo(currentPlayerInfo, opponentPlayerInfo);
    }


    private NextPlay AskWhatToDo()
    {
        CheckSuperstarAbilityEnabled();
        NextPlay playerActionRequest = _currentPlayerController.GetIfSuperstarAbilityIsEnable() ? _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible() : _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        return playerActionRequest;
    }


    // TODO: Move NextPlay handler to another class
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
                _currentPlayerController.ManageIfSuperstarAbilityIsEnabled(false);
                HandleSuperstarsAbilities();
                break;
            case NextPlay.EndTurn:
                EndTurn();
                break;
            case NextPlay.GiveUp:
                _view.CongratulateWinner(_opponentPlayerController.GetSuperstarName());
                _gameIsOn = false;
                break;
        }
    }


    // if dentro de switch??? numero
    private void ActivateSuperstarsAbilities()
    {
        switch (_currentPlayerController.GetSuperstarName())
        {
            case "CHRIS JERICHO":
                if (!CardDeckInfoProvider.CheckIfDeckIsEmpty(_currentPlayerController.GetHand())) _currentPlayerController.ManageIfSuperstarAbilityIsEnabled(true);
                break;
            case "THE UNDERTAKER":
                if (CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_currentPlayerController.GetHand(), 2)) _currentPlayerController.ManageIfSuperstarAbilityIsEnabled(true);
                break;
            case "STONE COLD STEVE AUSTIN":
                _currentPlayerController.ManageIfSuperstarAbilityIsEnabled(true);
                break;
        }
    }


    private void HandleTheRockAbility()
    {
        if (!_view.DoesPlayerWantToUseHisAbility(_currentPlayerController.GetSuperstarName())) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_currentPlayerController.GetSuperstarName(), _currentPlayerController.GetSuperstarAbility());
        int indexSelectedCard = _view.AskPlayerToSelectCardsToRecover(_currentPlayerController.GetSuperstarName(), 1, FormatUtility.FormatCardsToDisplay(_currentPlayerController.GetRingside()));
        _currentPlayerController.RecoverCardToArsenalFromRingside(indexSelectedCard);
    }


    private void HandleKaneAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(_currentPlayerController.GetSuperstarName(), _currentPlayerController.GetSuperstarAbility());
        _view.SayThatSuperstarWillTakeSomeDamage(_opponentPlayerController.GetSuperstarName(), 1);
        MakeDamage(1);
    }


    // numero
    private void CheckSuperstarAbilityEnabled()
    {
        if (_currentPlayerController.GetSuperstarName() == "CHRIS JERICHO" && CardDeckInfoProvider.CheckIfDeckIsEmpty(_currentPlayerController.GetHand())) _currentPlayerController.ManageIfSuperstarAbilityIsEnabled(false);
        if (_currentPlayerController.GetSuperstarName() == "THE UNDERTAKER" && !CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_currentPlayerController.GetHand(), 2)) _currentPlayerController.ManageIfSuperstarAbilityIsEnabled(false);
    }

    private void HandleSuperstarsAbilities()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(_currentPlayerController.GetSuperstarName(), _currentPlayerController.GetSuperstarAbility());
        _currentPlayerController.ManageIfSuperstarAbilityIsEnabled(false);
        switch (_currentPlayerController.GetSuperstarName())
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
        _currentPlayerController.DrawCardFromArsenal();
        PromptPlayerToDiscardCardsToArsenal();
    }


    // numero 2 y 1
    private void HandleTheUndertakerAbility()
    {
        PromptPlayerToDiscardCardsToRingside(2);
        int indexSelectedCardTake = _view.AskPlayerToSelectCardsToPutInHisHand(_currentPlayerController.GetSuperstarName(), 1, FormatUtility.FormatCardsToDisplay(_currentPlayerController.GetRingside()));
        _currentPlayerController.RecoverCardToHandFromRingside(indexSelectedCardTake);
    }
    
    
    private void PromptPlayerToDiscardCardsToRingside(int totalCardsToDiscard)
    {
        for (int numberOfCardToDiscard = totalCardsToDiscard; numberOfCardToDiscard > 0; numberOfCardToDiscard--)
        {
            int indexSelectedCard = _view.AskPlayerToSelectACardToDiscard(FormatUtility.FormatCardsToDisplay(_currentPlayerController.GetHand()), _currentPlayerController.GetSuperstarName(), _currentPlayerController.GetSuperstarName(), numberOfCardToDiscard);
            _currentPlayerController.DiscardCardToRingside(indexSelectedCard);
        }
    }
    
    
    private void PromptPlayerToDiscardCardsToArsenal()
    {
        _view.SayThatPlayerDrawCards(_currentPlayerController.GetSuperstarName(), 1);
        int indexSelectedCard = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(_currentPlayerController.GetSuperstarName(), FormatUtility.FormatCardsToDisplay(_currentPlayerController.GetHand()));
        _currentPlayerController.DiscardCardToArsenal(indexSelectedCard);
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
        if (!CardDeckInfoProvider.CheckIfDeckIsEmpty(_currentPlayerController.GetArsenal())) return false;
        _view.CongratulateWinner(_opponentPlayerController.GetSuperstarName());
        _gameIsOn = false;
        return true;
    }


    private void SwapPlayers()
    {
        PlayerController temporaryPlayerController = _currentPlayerController;
        _currentPlayerController = _opponentPlayerController;
        _opponentPlayerController = temporaryPlayerController;
    }


    private void ShowCardsToUser()
    {
        // var showCardController = _currentPlayer.BuildShowCardController();
        // showCardController.Execute();
        
        CardSet deckUserWantsToSee = _view.AskUserWhatSetOfCardsHeWantsToSee();
        List<Card> cardsToDisplay = GetCardsForDeck(deckUserWantsToSee);
        _view.ShowCards(FormatUtility.FormatCardsToDisplay(cardsToDisplay));
    }
    
    // TODO: Move show card to another class
    private List<Card> GetCardsForDeck(CardSet cardset) =>
        cardset switch
        {
            CardSet.Hand => _currentPlayerController.GetHand(),
            CardSet.RingArea => _currentPlayerController.GetRingArea(),
            CardSet.RingsidePile => _currentPlayerController.GetRingside(),
            CardSet.OpponentsRingArea => _opponentPlayerController.GetRingArea(),
            CardSet.OpponentsRingsidePile => _opponentPlayerController.GetRingside(),
            _ => new List<Card>()
        };


    // clean code!!!!!
    private void PlayCard()
    {
        Dictionary<int, Card> cardsAndTheirIndexInTheHandPlayerCanPlay =
            _currentPlayerController.GetHand()
                .Select((card, index) => new { Index = index, Card = card })
                .Where(item => item.Card.GetFortitude() <= _currentPlayerController.GetFortitudeRating())
                .ToDictionary(item => item.Index, item => item.Card);
        List<Card> cardsPlayerCanPlay = cardsAndTheirIndexInTheHandPlayerCanPlay.Values.ToList();
        List<String> formattedCardsPlayerCanPlay = FormatUtility.FormatCardsPlayerCanPlay(cardsPlayerCanPlay);
        int id = _view.AskUserToSelectAPlay(formattedCardsPlayerCanPlay);
        if (id != -1) PlaySelectedCard(cardsAndTheirIndexInTheHandPlayerCanPlay.Keys.ElementAt(id), cardsPlayerCanPlay[id].GetCardInfo(), formattedCardsPlayerCanPlay[id]);
    }


    private void PlaySelectedCard(int indexSelectedCard, CardInfo selectedCardInfo, String formattedSelectedCard)
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(_currentPlayerController.GetSuperstarName(), formattedSelectedCard);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        int totalDamage = int.Parse(selectedCardInfo.Damage);
        if (_opponentPlayerController.GetSuperstarName() == "MANKIND") totalDamage -= 1;
        _view.SayThatSuperstarWillTakeSomeDamage(_opponentPlayerController.GetSuperstarName(), totalDamage);
        MakeDamage(totalDamage);
        _currentPlayerController.DiscardCardToRingArea(indexSelectedCard);
    }


    private void MakeDamage(int totalDamage)
    {
        for (int currentDamage = 1; currentDamage <= totalDamage; currentDamage++)
        {
            if (CheckPinVictory()) break;
            Card lastCardOfDeck = CardDeckInfoProvider.GetLastCardOfDeck(_opponentPlayerController.GetArsenal());
            CardInfo infoLastCardOfDeck = lastCardOfDeck.GetCardInfo();
            string stringLastCardOfDeck = Formatter.CardToString(infoLastCardOfDeck); 
            _view.ShowCardOverturnByTakingDamage(stringLastCardOfDeck, currentDamage, totalDamage);
            _opponentPlayerController.ReceiveDamage();
        }
    }
    
    
    private bool CheckPinVictory()
    {
        if (!CardDeckInfoProvider.CheckIfDeckIsEmpty(_opponentPlayerController.GetArsenal())) return false;
        _view.CongratulateWinner(_currentPlayerController.GetSuperstarName());
        _gameIsOn = false;
        return true;
    }
}
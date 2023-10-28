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
    private bool _gameIsOn;
    
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        _gameIsOn = true;
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
        _currentPlayerController.StartTurn();
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
            _currentPlayerController.AskWhatToDo();
        }
    }
    
    
    private void ShowGameInfo()
    {
        PlayerInfo currentPlayerInfo = GetPlayerInfo(_currentPlayer);
        PlayerInfo opponentPlayerInfo = GetPlayerInfo(_opponentPlayer);
        _view.ShowGameInfo(currentPlayerInfo, opponentPlayerInfo);
    }

    
    private static PlayerInfo GetPlayerInfo(Player player)
    {
        return new PlayerInfo(player.GetSuperstarName(), player.GetFortitudeRating(),
            CardDeckInfoProvider.GetDeckLength(player.GetHand()),
            CardDeckInfoProvider.GetDeckLength(player.GetArsenal()));
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
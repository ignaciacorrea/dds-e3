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
            SetUpGame();
            PlayGame();
        }
        catch (InvalidDeckException ex)
        {
            _view.SayThatDeckIsInvalid();
        }
    }


    private void SetUpGame()
    {
        CreatePlayers();
        CreatePlayersControllers();
        SetCurrentPlayerBasedOnTheSuperstarValue();
    }


    private void PlayGame()
    {
        _currentPlayerController.StealStartingHand();
        _opponentPlayerController.StealStartingHand();
        _currentPlayerController.StartTurn();
        LoopGame();
    }


    private void CreatePlayers()
    {
        PlayerCreator playerCreator = new PlayerCreator(_view, _deckFolder);
        _currentPlayer = playerCreator.SetUpPlayer();
        _opponentPlayer = playerCreator.SetUpPlayer();
    }


    private void CreatePlayersControllers()
    {
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
            HandleAction();
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
        return new PlayerInfo(player.GetSuperstarName(), player.GetFortitudeRating(), player.Hand.GetLength(), player.Arsenal.GetLength());
    }


    
    private void HandleAction()
    {
        ActionController actionController = new ActionController(_currentPlayerController, _opponentPlayerController);
        actionController.Execute();
    }
    
    
    private bool CheckCountOutVictory()
    {
        if (!_currentPlayer.Arsenal.CheckIfIsEmpty()) return false;
        _view.CongratulateWinner(_opponentPlayer.GetSuperstarName());
        _gameIsOn = false;
        return true;
    }


    private void SwapPlayers()
    {
        PlayerController temporaryPlayerController = _currentPlayerController;
        _currentPlayerController = _opponentPlayerController;
        _opponentPlayerController = temporaryPlayerController;
    }
    
}
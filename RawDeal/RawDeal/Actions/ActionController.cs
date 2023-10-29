using RawDealView.Options;

namespace RawDeal;

public class ActionController
{
    private readonly PlayerController _playerController;
    private readonly PlayerController _opponentController;
    
    public ActionController(PlayerController player, PlayerController opponent)
    {
        _playerController = player;
        _opponentController = opponent;
    }

    public void Execute()
    {
        NextPlay playerRequest = _playerController.AskWhatToDo();
        HandleOption(playerRequest);
    }
    
    private void HandleOption(NextPlay playerRequest)
    {
        switch (playerRequest)
        {
            case NextPlay.ShowCards:
                ShowCards();
                break;
            case NextPlay.PlayCard:
                PlayCard();
                break;
            // case NextPlay.UseAbility:
            //     UseAbility();
            //     break;
            case NextPlay.EndTurn:
                EndTurn();
                break;
            // case NextPlay.GiveUp:
            //     _view.CongratulateWinner(_opponentPlayerController.GetSuperstarName());
            //     _gameIsOn = false;
            //     break;
        }
    }


    private void ShowCards()
    {
        ShowCardController showCardController = _playerController.BuildShowCardController();
        showCardController.Execute();
    }
    
    
    public void PlayCard()
    {
        PlayCardController playCardController = _playerController.BuildPlayCardController();
        playCardController.Execute();
    }


    private void UseAbility()
    {
        _playerController._superstarAbility.ApplyAfterDrawing(_opponentController);
    }
    
    private void EndTurn()
    {
        if(CheckCountOutVictory()) return;
        SwapPlayers();
    }
    
}
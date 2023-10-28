using RawDealView;

namespace RawDeal;

public class TheRockAbility : SuperstarAbility
{
    private readonly View _view;
    private Player _player;
    private PlayerController _playerController;
    
    public TheRockAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyBeforeDrawing(Player opponent)
    {
        MakeControllers(opponent);
        if (!CardDeckInfoProvider.CheckIfDeckIsEmpty(_player.GetRingside()))
            HandleTheRockAbility();
    }
    
    private void HandleTheRockAbility()
    {
        if (!_view.DoesPlayerWantToUseHisAbility(_player.GetSuperstarName())) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        int indexSelectedCard = _view.AskPlayerToSelectCardsToRecover(_player.GetSuperstarName(), 1, FormatUtility.FormatCardsToDisplay(_player.GetRingside()));
        _playerController.RecoverCardToArsenalFromRingside(indexSelectedCard);
    }

    protected override void MakeControllers(Player opponent)
    {
        _playerController = new PlayerController(_player, opponent, _view);
    }
}
using RawDealView;

namespace RawDeal;

public class KaneAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;
    private PlayerController _playerController;
    
    public KaneAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyBeforeDrawing(Player opponent)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        _view.SayThatSuperstarWillTakeSomeDamage(opponent.GetSuperstarName(), 1);
        MakeControllers(opponent);
        _playerController.MakeDamage(1);
    }
    
    protected override void MakeControllers(Player opponent)
    {
        _playerController = new PlayerController(_player, opponent, _view);
    }
}
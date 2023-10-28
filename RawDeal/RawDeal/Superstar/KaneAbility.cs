using RawDealView;

namespace RawDeal;

public class KaneAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;
    
    public KaneAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyBeforeDrawing(Player opponent)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        _view.SayThatSuperstarWillTakeSomeDamage(opponent.GetSuperstarName(), 1);
        PlayerController playerController = new PlayerController(_player, opponent, _view);
        playerController.MakeDamage(1);
    }
}
using RawDealView;

namespace RawDeal;

public class MankindAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;
    private PlayerController _playerController;
    private const int CardsInArsenalNeededToUseAbility = 2;
    
    public MankindAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyBeforeDrawing(Player opponent)
    {
        if (CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_player.GetArsenal(), CardsInArsenalNeededToUseAbility))
            _playerController.DrawCardFromArsenal();
    }
    
    protected override void MakeControllers(Player opponent)
    {
        _playerController = new PlayerController(_player, opponent, _view);
    }
}
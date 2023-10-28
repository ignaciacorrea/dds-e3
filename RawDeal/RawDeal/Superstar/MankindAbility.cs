using RawDealView;

namespace RawDeal;

public class MankindAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;
    private const int AmountCardsNeededInArsenal = 2;
    
    public MankindAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyBeforeDrawing(Player opponent)
    {
        if(!CheckIfAbilityCanBeUsed()) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        PlayerController playerController = new PlayerController(_player, opponent, _view);
        playerController.DrawCardFromArsenal();
    }
    
    public override bool CheckIfAbilityCanBeUsed()
    {
        bool playerHasEnoughCardsInArsenal =  CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_player.GetArsenal(), AmountCardsNeededInArsenal);
        return playerHasEnoughCardsInArsenal;
    }
}
using RawDealView;

namespace RawDeal;

public class TheRockAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;
    private const int AmountCardsRecover = 1;
    
    public TheRockAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyBeforeDrawing(Player opponent)
    {
        if (!CheckIfAbilityCanBeUsed()) return;
        if (!_view.DoesPlayerWantToUseHisAbility(_player.GetSuperstarName())) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        HandleAbility(opponent);
    }

    public override bool CheckIfAbilityCanBeUsed()
    {
        bool isRingsideEmpty = _player.Ringside.CheckIfIsEmpty();
        bool abilityCanBeUsed = !isRingsideEmpty;
        return abilityCanBeUsed;
    }
    
    private void HandleAbility(Player opponent)
    {
        List<string> formattedCardsToDisplay = _player.Ringside.FormatCardsToDisplay();
        int indexSelectedCard = _view.AskPlayerToSelectCardsToRecover(_player.GetSuperstarName(), AmountCardsRecover, formattedCardsToDisplay);
        PlayerController playerController = new PlayerController(_player, opponent, _view);
        playerController.RecoverCardToArsenalFromRingside(indexSelectedCard);
    }
}
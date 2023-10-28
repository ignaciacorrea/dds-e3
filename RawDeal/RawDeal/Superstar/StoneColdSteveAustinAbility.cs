using RawDealView;

namespace RawDeal;

public class StoneColdSteveAustinAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;

    public StoneColdSteveAustinAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }

    public override void ApplyAfterDrawing(Player opponent)
    {
        if (!CheckIfAbilityCanBeUsed()) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        PlayerController playerController = new PlayerController(_player, opponent, _view);
        HandleAbility(playerController);
    }

    public override bool CheckIfAbilityCanBeUsed()
    {
        // solo puede usar la habilidad una vez en su turno
        // if (_playerController.GetSuperstarAbilityUsed()) return false;
        return true;
    }

    private void HandleAbility(PlayerController playerController)
    {
        _view.SayThatPlayerDrawCards(_player.GetSuperstarName(), 1);
        playerController.DrawCardFromArsenal();
        List<string> formattedCardsToDisplay = FormatUtility.FormatCardsToDisplay(_player.GetHand());
        int indexSelectedCard = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(_player.GetSuperstarName(), formattedCardsToDisplay);
        playerController.DiscardCardToArsenal(indexSelectedCard);
    }
}
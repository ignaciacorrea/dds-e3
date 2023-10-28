using RawDealView;

namespace RawDeal;

public class ChrisJerichoAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;
    private const int AmountCardsDiscard = 1;
    
    public ChrisJerichoAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyAfterDrawing(Player opponent)
    {
        if (!CheckIfAbilityCanBeUsed()) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        PromptPlayerToDiscardCardsToRingside(_player, opponent);
        PromptPlayerToDiscardCardsToRingside(opponent, _player);
    }

    public override bool CheckIfAbilityCanBeUsed()
    {
        // solo puede usar la habilidad una vez en su turno
        // if (_playerController.GetSuperstarAbilityUsed()) return false;
        bool isHandEmpty = CardDeckInfoProvider.CheckIfDeckIsEmpty(_player.GetHand());
        return !isHandEmpty;
    }
    
    private void PromptPlayerToDiscardCardsToRingside(Player player, Player opponent)
    {
        PlayerController playerController = new PlayerController(player, opponent, _view);
        List<string> formattedCardsToDisplay = FormatUtility.FormatCardsToDisplay(player.GetHand());
        int indexSelectedCard = _view.AskPlayerToSelectACardToDiscard(formattedCardsToDisplay, player.GetSuperstarName(), player.GetSuperstarName(), AmountCardsDiscard);
        playerController.DiscardCardToRingside(indexSelectedCard);
    }
}
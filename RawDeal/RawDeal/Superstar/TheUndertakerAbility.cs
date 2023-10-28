using RawDealView;

namespace RawDeal;

public class TheUndertakerAbility : SuperstarAbility
{
    private readonly View _view;
    private readonly Player _player;
    private const int AmountCardsDiscard = 2;
    private const int AmountCardsRecover = 1;
    
    public TheUndertakerAbility(View view, Player player)
    {
        _view = view;
        _player = player;
    }
    
    public override void ApplyAfterDrawing(Player opponent)
    {
        if (!CheckIfAbilityCanBeUsed()) return;
        _view.SayThatPlayerIsGoingToUseHisAbility(_player.GetSuperstarName(), _player.GetStringSuperstarAbility());
        PlayerController playerController = new PlayerController(_player, opponent, _view);
        PromptPlayerToDiscardCardsToRingside(playerController);
        PromptPlayerToRecoverCardFromRingside(playerController);
        
    }

    public override bool CheckIfAbilityCanBeUsed()
    {
        // solo puede usar la habilidad una vez en su turno
        // if (_playerController.GetSuperstarAbilityUsed()) return false;
        bool playerHasEnoughCardsInArsenal = CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_player.GetHand(), AmountCardsDiscard);
        return playerHasEnoughCardsInArsenal;
    }
    
    private void PromptPlayerToDiscardCardsToRingside(PlayerController playerController)
    {
        for (int numberOfCardToDiscard = AmountCardsDiscard; numberOfCardToDiscard > 0; numberOfCardToDiscard--)
        {
            List<string> formattedCardsToDisplay = FormatUtility.FormatCardsToDisplay(_player.GetHand());
            int indexSelectedCard = _view.AskPlayerToSelectACardToDiscard(formattedCardsToDisplay, _player.GetSuperstarName(), _player.GetSuperstarName(), numberOfCardToDiscard);
            playerController.DiscardCardToRingside(indexSelectedCard);
        }
    }
    
    private void PromptPlayerToRecoverCardFromRingside(PlayerController playerController)
    {
        List<string> formattedCardsToDisplay = FormatUtility.FormatCardsToDisplay(_player.GetRingside());
        int indexSelectedCard = _view.AskPlayerToSelectCardsToPutInHisHand(_player.GetSuperstarName(), AmountCardsRecover, formattedCardsToDisplay);
        playerController.RecoverCardToHandFromRingside(indexSelectedCard);
    }
}
using RawDealView;

namespace RawDeal;

public class TheUndertakerAbility : SuperstarAbility
{
    public TheUndertakerAbility(Player player, Player opponent, View view) : base(player, opponent, view)
    {
        IsNecessaryToAsk = true;
    }

    public override void Use()
    {
        // ...
    }
    
    public override bool CheckIfAbilityCanBeUsed()
    {
        return _player.GetUsedAbility() == false && CardDeckInfoProvider.CheckIfDeckHasAnAmountOfCards(_player.GetHand(), 2);
    }
}
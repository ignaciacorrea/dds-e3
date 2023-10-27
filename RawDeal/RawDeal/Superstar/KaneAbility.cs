using RawDealView;

namespace RawDeal;

public class KaneAbility : SuperstarAbility
{
    public KaneAbility(Player player, Player opponent, View view) : base(player, opponent, view)
    {
        IsNecessaryToAsk = false;
    }

    public override void Use()
    {
        // ...
    }
    
    public override bool CheckIfAbilityCanBeUsed()
    {
        return _player.GetUsedAbility() == false && !CardDeckInfoProvider.CheckIfDeckIsEmpty(_opponent.GetArsenal());
    }
}
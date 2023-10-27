using RawDealView;

namespace RawDeal;

public class StoneColdSteveAustinAbility : SuperstarAbility
{
    public StoneColdSteveAustinAbility(Player player, Player opponent, View view) : base(player, opponent, view)
    {
        IsNecessaryToAsk = true;
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
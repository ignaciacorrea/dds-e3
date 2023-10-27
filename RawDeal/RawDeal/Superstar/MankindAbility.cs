using RawDealView;

namespace RawDeal;

public class MankindAbility : SuperstarAbility
{
    public MankindAbility(Player player, Player opponent, View view) : base(player, opponent, view)
    {
        IsNecessaryToAsk = false;
    }

    public override void Use()
    {
        // _player.CardsDrawnPerTurn = 2;
        // _player.DamageReduction = 1;
    }
    
    public override bool CheckIfAbilityCanBeUsed()
    {
        return true;
    }
}
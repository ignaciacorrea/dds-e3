using RawDealView;

namespace RawDeal;

public class SuperstarAbility
{
    public virtual void ApplyBeforeDrawing(Player opponent) { }
    public virtual void ApplyAfterDrawing(Player opponent) { }
    public virtual bool CheckIfAbilityCanBeUsed() {return false;}
}
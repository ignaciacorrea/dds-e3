using RawDealView;

namespace RawDeal;

public abstract class SuperstarAbility
{
    protected readonly Player _player;
    protected readonly Player _opponent;
    protected readonly View _view;
    public bool IsNecessaryToAsk;
    
    public SuperstarAbility(Player player, Player opponent, View view)
    {
        _player = player;
        _opponent = opponent;
        _view = view;
    }

    public abstract void Use();
    
    public abstract bool CheckIfAbilityCanBeUsed();
}
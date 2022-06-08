namespace ModifierSystem
{
    public interface ICheckComponent
    {
        ICooldownComponent CooldownComponent { get; }
        ICostComponent CostComponent { get; }
        IChanceComponent ChanceComponent { get; }

        void Effect();
        bool Check();

        bool EffectComponentIsOfType<T>() where T : IEffectComponent;
    }
}
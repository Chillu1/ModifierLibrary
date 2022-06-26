namespace ModifierSystem
{
    public interface ICheckComponent : IDisplay
    {
        ICooldownComponent CooldownComponent { get; }
        ICostComponent CostComponent { get; }
        IChanceComponent ChanceComponent { get; }

        void Effect();
        void EffectTime();
        bool Check();
        void Apply();

        bool EffectComponentIsOfType<T>() where T : IEffectComponent;
    }
}
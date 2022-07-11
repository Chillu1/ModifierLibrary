namespace ModifierSystem
{
    public interface ICheckComponent : IDisplayable
    {
        ICooldownComponent CooldownComponent { get; }
        ICostComponent CostComponent { get; }
        IChanceComponent ChanceComponent { get; }
        string Info { get; }

        void Effect();
        void EffectTime();
        bool Check();
        void Apply();

        bool EffectComponentIsOfType<T>() where T : IEffectComponent;
    }
}
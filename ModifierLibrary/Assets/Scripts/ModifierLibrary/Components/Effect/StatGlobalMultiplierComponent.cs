using UnitLibrary;

namespace ModifierLibrary
{
    public sealed class StatGlobalMultiplierComponent : EffectComponent
    {
        private PoolStatType StatType { get; }
        private double Multiplier { get; }

        public StatGlobalMultiplierComponent(PoolStatType type, double multiplier, ConditionCheckData conditionCheckData = null,
            bool isRevertible = false) : base(conditionCheckData, isRevertible)
        {
            StatType = type;
            Multiplier = multiplier;

            Info = $"Stat: {StatType} {Multiplier}";
        }

        protected override void Effect(Unit receiver, Unit acter)
        {
            receiver.SetGlobalRegenMultiplier(StatType, Multiplier);
        }

        protected override void RevertEffect(Unit receiver, Unit acter)
        {
            receiver.SetGlobalRegenMultiplier(StatType, 1d);
        }
    }
}
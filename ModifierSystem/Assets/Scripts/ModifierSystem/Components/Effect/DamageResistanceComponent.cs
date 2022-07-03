using BaseProject;

namespace ModifierSystem
{
    public sealed class DamageResistanceComponent : EffectComponent
    {
        private DamageType DamageType { get; }
        private double Value { get; }

        public DamageResistanceComponent(DamageType damageType, double value, ConditionCheckData conditionCheckData = null,
            bool isRevertible = false) : base(conditionCheckData, isRevertible)
        {
            DamageType = damageType;
            Value = value;

            Info = $"Damage Resistance: {damageType} {value}\n";
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.DamageTypeDamageResistances.ChangeValue(DamageType, Value);
        }

        protected override void RevertEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.DamageTypeDamageResistances.ChangeValue(DamageType, -Value);
        }
    }
}
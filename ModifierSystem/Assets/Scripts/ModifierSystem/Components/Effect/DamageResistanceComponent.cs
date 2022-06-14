using BaseProject;

namespace ModifierSystem
{
    public sealed class DamageResistanceComponent : EffectComponent
    {
        private DamageType DamageType { get; }
        private float Value { get; }

        public DamageResistanceComponent(DamageType damageType, float value, ConditionCheckData conditionCheckData = null)
            : base(conditionCheckData)
        {
            DamageType = damageType;
            Value = value;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.DamageTypeDamageResistances.ChangeValue(DamageType, Value);
        }

        protected override void RemoveEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.DamageTypeDamageResistances.ChangeValue(DamageType, -Value);
        }
    }
}
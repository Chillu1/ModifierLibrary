using BaseProject;

namespace ModifierSystem
{
    public sealed class DamageResistanceComponent : EffectComponent
    {
        private Properties EffectProperties { get; }
        private DamageType DamageType { get; }
        private double Value { get; }

        public DamageResistanceComponent(Properties effectProperties, IBaseEffectProperties baseProperties = null) : base(baseProperties)
        {
            EffectProperties = effectProperties;
        }
        
        public DamageResistanceComponent(DamageType damageType, double value, ConditionCheckData conditionCheckData = null,
            bool isRevertible = false) : base(conditionCheckData, isRevertible)
        {
            DamageType = damageType;
            Value = value;

            Info = $"Damage Resistance: {damageType} {value}\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            if (EffectProperties.DamageType != DamageType.None)
            {
                receiver.DamageTypeDamageResistances.ChangeValue(EffectProperties.DamageType, EffectProperties.Value);
                return;
            }

            receiver.DamageTypeDamageResistances.ChangeValue(DamageType, Value);
        }

        protected override void RevertEffect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            receiver.DamageTypeDamageResistances.ChangeValue(DamageType, -Value);
        }

        public struct Properties : IEffectProperties
        {
            public DamageType DamageType { get; }
            public double Value { get; }

            public Properties(DamageType damageType, double value)
            {
                DamageType = damageType;
                Value = value;
            }
        }
    }

    public interface IEffectProperties
    {
    }
}
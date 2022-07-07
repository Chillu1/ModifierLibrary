using BaseProject;

namespace ModifierSystem
{
    public sealed class ElementResistanceComponent : EffectComponent
    {
        private ElementType ElementType { get; }
        private double Value { get; }

        public ElementResistanceComponent(ElementType elementType, double value, ConditionCheckData conditionCheckData = null,
            bool isRevertible = false) : base(conditionCheckData, isRevertible)
        {
            ElementType = elementType;
            Value = value;

            Info = $"Resistance: {elementType} {value}\n";
        }

        protected override void Effect(Unit receiver, Unit acter)
        {
            receiver.ElementalDamageResistances.ChangeValue(ElementType, Value);
        }

        protected override void RevertEffect(Unit receiver, Unit acter)
        {
            receiver.ElementalDamageResistances.ChangeValue(ElementType, -Value);
        }
    }
}
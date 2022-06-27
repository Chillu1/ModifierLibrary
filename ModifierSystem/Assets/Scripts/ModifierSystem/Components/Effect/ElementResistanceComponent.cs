using BaseProject;

namespace ModifierSystem
{
    public sealed class ElementResistanceComponent : EffectComponent
    {
        private ElementType ElementType { get; }
        private float Value { get; }

        public ElementResistanceComponent(ElementType elementType, float value, ConditionCheckData conditionCheckData = null,
            bool isRevertible = false) : base(conditionCheckData, isRevertible)
        {
            ElementType = elementType;
            Value = value;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.ElementalDamageResistances.ChangeValue(ElementType, Value);
        }

        protected override void RevertEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.ElementalDamageResistances.ChangeValue(ElementType, -Value);
        }
    }
}
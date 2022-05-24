using BaseProject;

namespace ModifierSystem
{
    public sealed class ApplierEffectComponent : EffectComponent, IStackEffectComponent
    {
        private Modifier Modifier { get; }
        private AddModifierParameters Parameters { get; }
        public bool IsStackEffect { get; }

        public ApplierEffectComponent(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default,
            bool isStackEffect = false, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Modifier = modifier;
            Parameters = parameters;
            IsStackEffect = isStackEffect;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            var clonedModifier = (Modifier)Modifier.Clone();
            clonedModifier.CopyEvents(Modifier);
            ((Being)receiver).AddModifier(clonedModifier, Parameters);
        }

        public void StackEffect(int stacks, double value)
        {
            if (IsStackEffect)
                SimpleEffect();
        }
    }
}
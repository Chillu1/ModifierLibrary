using BaseProject;

namespace ModifierSystem
{
    public sealed class ApplierComponent : EffectComponent, IStackEffectComponent
    {
        private IModifier Modifier { get; }
        private AddModifierParameters Parameters { get; }
        public bool IsStackEffect { get; }

        public ApplierComponent(IModifier modifier, ITargetComponent targetComponent, AddModifierParameters parameters = AddModifierParameters.Default,
            bool isStackEffect = false, ConditionCheckData conditionCheckData = null) : base(targetComponent, conditionCheckData)
        {
            Modifier = modifier;
            Parameters = parameters;
            IsStackEffect = isStackEffect;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter, bool triggerEvents)
        {
            var clonedModifier = (IModifier)Modifier.Clone();
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
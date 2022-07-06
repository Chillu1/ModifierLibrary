using BaseProject;

namespace ModifierSystem
{
    public sealed class ApplierEffectComponent : EffectComponent, IStackEffectComponent
    {
        private Modifier Modifier { get; }
        public bool IsStackEffect { get; }

        public ApplierEffectComponent(Modifier modifier, bool isStackEffect = false, ConditionCheckData conditionCheckData = null) : base(
            conditionCheckData)
        {
            Modifier = modifier;
            IsStackEffect = isStackEffect;

            Info = $"Applies: {Modifier}\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            ((Unit)receiver).AddModifier((Modifier)Modifier.Clone(), (Unit)acter);
        }

        public void StackEffect(int stacks, double value)
        {
            if (IsStackEffect)
                SimpleEffect();
        }
    }
}
using BaseProject;

namespace ModifierSystem
{
    public sealed class ApplierEffectComponent : EffectComponent, IStackEffectComponent
    {
        private Properties EffectProperties { get; }
        private Modifier Modifier { get; }
        private bool IsStackEffect { get; }

        public ApplierEffectComponent(Properties effectProperties, IBaseEffectProperties baseProperties = null) : base(baseProperties)
        {
            EffectProperties = effectProperties;
        }

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

        public struct Properties : IEffectProperties
        {
            public Modifier Modifier { get; }
            public bool IsStackEffect { get; }

            public Properties(Modifier modifier, bool isStackEffect)
            {
                Modifier = modifier;
                IsStackEffect = isStackEffect;
            }
        }
    }
}
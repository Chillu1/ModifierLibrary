using JetBrains.Annotations;

namespace ModifierSystem
{
    public class StackComponentProperties
    {
        [NotNull] public IStackEffectComponent StackEffectComponent { get; }
        public WhenStackEffect WhenStackEffect = WhenStackEffect.Always;
        public double Value = 0;
        public int OnXStacks = -1;
        public int MaxStacks = 10;

        public StackComponentProperties([NotNull] IStackEffectComponent stackEffectComponent)
        {
            StackEffectComponent = stackEffectComponent;
        }
    }
}
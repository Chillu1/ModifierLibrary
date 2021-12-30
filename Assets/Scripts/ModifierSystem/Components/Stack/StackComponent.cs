using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class StackComponent : Component, IStackComponent
    {
        [NotNull] public IStackEffectComponent StackEffectComponent { get; }
        public WhenStackEffect WhenStackEffect { get; }
        public double Value { get; }
        public int OnXStacks { get; }

        private int Stacks { get; set; }
        private int MaxStacks { get; set; }
        private bool Finished { get; set; }

        public StackComponent(StackComponentProperties properties)
        {
            StackEffectComponent = properties.StackEffectComponent;
            WhenStackEffect = properties.WhenStackEffect;
            Value = properties.Value;
            OnXStacks = properties.OnXStacks;
            MaxStacks = properties.MaxStacks;

            //Validate();//TODO
        }

        public void Stack()
        {
            if (Finished || Stacks + 1 >= MaxStacks)
                return;
            //Log.Verbose($"Stacks: {Stacks}/{MaxStacks}", "modifiers");

            Stacks++;
            switch (WhenStackEffect)
            {
                case WhenStackEffect.Always:
                    TriggerStackEffect();
                    break;
                case WhenStackEffect.OnXStacks:
                    if (Stacks == OnXStacks)
                    {
                        TriggerStackEffect();
                        Finished = true;
                        //TODO Remove? Trigger TimeComponent Remove timer?
                    }
                    break;
                case WhenStackEffect.EveryXStacks:
                    if (Stacks == OnXStacks)
                    {
                        TriggerStackEffect();
                        ResetStacks();
                    }
                    break;
                case WhenStackEffect.None:
                    Log.Error($"StackEffectType {WhenStackEffect.None} illegal");
                    return;
            }

            void TriggerStackEffect()
            {
                StackEffectComponent.Effect(Stacks, Value);
            }
        }

        /// <summary>
        ///     Can be caused by duration, timing out, etc
        /// </summary>
        public void RemoveStack()
        {
            if (Finished || Stacks <= 0)
                return;

            Stacks--;

            switch (WhenStackEffect)
            {
                case WhenStackEffect.ZeroStacks:
                    if (Stacks == 0)
                        StackEffectComponent.Effect(Stacks, Value);
                    break;
                case WhenStackEffect.None:
                    Log.Error($"StackEffectType {WhenStackEffect.None} illegal");
                    return;
            }
        }

        public void ResetStacks()
        {
            Stacks = 0;
        }
    }

    public enum GenericValueBasedStackEffects
    {
        None = 0,
        Effect,
        Add,
        AddStacksBased,
        Multiply,
        MultiplyStacksBased,
    }
}
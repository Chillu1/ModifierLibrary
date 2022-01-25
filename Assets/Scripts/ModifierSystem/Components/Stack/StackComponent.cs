using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class StackComponent : Component, IStackComponent
    {
        public IStackEffectComponent StackEffectComponent { get; }
        public WhenStackEffect WhenStackEffect { get; }
        public double Value { get; }
        public int OnXStacks { get; }

        private int Stacks { get; set; }
        private int MaxStacks { get; set; }
        private bool Finished { get; set; }

        public StackComponent(IStackEffectComponent effect, StackComponentProperties properties)
        {
            StackEffectComponent = effect;
            WhenStackEffect = properties.WhenStackEffect;
            Value = properties.Value;
            OnXStacks = properties.OnXStacks;
            MaxStacks = properties.MaxStacks;

            Validate();
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
                StackEffectComponent.StackEffect(Stacks, Value);
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
                        StackEffectComponent.StackEffect(Stacks, Value);
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

        private bool Validate()
        {
            //TODO Has to have Value if the effect is ValueBased, not sure how to check though
            bool valid = true;

            switch (WhenStackEffect)
            {
                case WhenStackEffect.Always:
                    break;
                case WhenStackEffect.OnXStacks:
                    if (OnXStacks == -1)
                    {
                        Log.Error("OnXStacks with a undefined OnXStacks value");
                        valid = false;
                    }
                    break;
                case WhenStackEffect.EveryXStacks:
                    if (OnXStacks == -1)
                    {
                        Log.Error("EveryXStacks with a undefined OnXStacks value");
                        valid = false;
                    }
                    break;
                case WhenStackEffect.ZeroStacks:
                    break;
                default:
                    Log.Error($"StackEffectType {WhenStackEffect.None} illegal");
                    valid = false;
                    break;
            }

            return valid;
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
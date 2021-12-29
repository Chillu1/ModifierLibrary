using BaseProject;

namespace ModifierSystem
{
    public class StackComponent : Component, IStackComponent
    {
        //Cooldown?
        public IStackEffectComponent StackEffectComponent { get; }
        public WhenStackEffect WhenStackEffect { get; }
        public double Value { get; }
        public int OnXStacks { get; }

        private int Stacks { get; set; }
        private int MaxStacks { get; set; }

        public StackComponent(IStackEffectComponent stackEffectComponent, WhenStackEffect whenStackEffect = WhenStackEffect.Always,
            double value = 0, int onXStacks = -1, int maxStacks = 10)
        {
            StackEffectComponent = stackEffectComponent;
            WhenStackEffect = whenStackEffect;
            Value = value;
            OnXStacks = onXStacks;
            MaxStacks = maxStacks;

            //Validate();//TODO
        }

        public void Stack()
        {
            if (Stacks + 1 >= MaxStacks)
                return;
            //Log.Verbose($"Stacks: {Stacks}/{MaxStacks}", "modifiers");

            Stacks++;
            switch (WhenStackEffect)
            {
                case WhenStackEffect.Always:
                    StackEffectComponent.StackEffect(Stacks, Value);
                    break;
                case WhenStackEffect.OnXStacks:
                    if (Stacks == OnXStacks)
                    {
                        StackEffectComponent.StackEffect(Stacks, Value);
                        ResetStacks();
                    }
                    break;
                case WhenStackEffect.EveryXStacks:
                    if (Stacks % OnXStacks == 0)
                        StackEffectComponent.StackEffect(Stacks, Value);
                    break;
                case WhenStackEffect.None:
                    Log.Error($"StackEffectType {WhenStackEffect.None} illegal");
                    return;
            }
        }

        /// <summary>
        ///     Can be caused by duration, timing out, etc
        /// </summary>
        public void RemoveStack()
        {
            if (Stacks <= 0)
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
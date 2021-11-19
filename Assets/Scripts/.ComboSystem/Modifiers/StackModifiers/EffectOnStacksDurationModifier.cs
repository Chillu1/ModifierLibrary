namespace ComboSystem
{
    public abstract class EffectOnStacksDurationModifier<TEffectOnStacks> : DurationModifier<TEffectOnStacks> where TEffectOnStacks : EffectOnStacksDurationModifierData
    {
        public int AmountOfStacks { get; protected set; } = 1;

        //Not forced only stackable, because duration modifier can also be refreshable
        public EffectOnStacksDurationModifier(string id, TEffectOnStacks data, ModifierProperties properties = ModifierProperties.Stackable)
            : base(id, data, properties)
        {
        }


        public override void Stack()
        {
            base.Stack();
            AmountOfStacks++;
            if (AmountOfStacks >= Data.AmountOfStacksForEffect)
            {
                Apply();
                AmountOfStacks -= Data.AmountOfStacksForEffect;//Maybe we dont want to remove the stacks in the future, but work around it in a different way
            }
        }

        public override string ToString()
        {
            return base.ToString() + ". AmountOfStacks: "+AmountOfStacks;
        }
    }
}
namespace ComboSystem
{
    public abstract class EffectOnStacksModifier<TEffectOnStacks> : Modifier<TEffectOnStacks> where TEffectOnStacks : EffectOnStacksModifierData
    {
        public int AmountOfStacks { get; protected set; } = 1;

        public EffectOnStacksModifier(string id, TEffectOnStacks data) : base(id, data, ModifierProperties.Stackable)
        {
        }

        public override void Stack()
        {
            AmountOfStacks++;
            if (AmountOfStacks >= Data.AmountOfStacksForEffect)
            {
                Apply();
                AmountOfStacks -= Data.AmountOfStacksForEffect;//Maybe we dont want to remove the stacks in the future, but work around it in a different way
            }

            base.Stack();
        }

        public override string ToString()
        {
            return base.ToString() + ". AmountOfStacks: "+AmountOfStacks;
        }
    }
}
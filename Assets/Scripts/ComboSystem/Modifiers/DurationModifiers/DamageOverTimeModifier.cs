namespace ComboSystem
{
    public class DamageOverTimeModifier : EffectOverTimeModifier<DamageOverTimeData>
    {
        public DamageOverTimeModifier(string id, DamageOverTimeData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override bool Apply()
        {
            if (!ApplyIsValid())
                return false;
            Target!.DealDamage(Data.DamageData);
            return true;
        }

        public override void Stack()
        {
            base.Stack();
            if (ModifierProperties.HasFlag(ModifierProperties.Stackable))
            {

            }
        }
    }
}
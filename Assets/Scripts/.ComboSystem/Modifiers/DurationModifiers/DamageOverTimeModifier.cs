using BaseProject;

namespace ComboSystem
{
    public class DamageOverTimeModifier : EffectOverTimeModifier<DamageOverTimeData>
    {
        public DamageOverTimeModifier(string id, DamageOverTimeData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override void Effect()
        {
            Target!.DealDamage(Data.DamageData);
        }

        public override void Stack()
        {
            base.Stack();
            if (ModifierProperties.HasFlag(ModifierProperties.Stackable))
            {
                //TODO
            }
        }
    }
}
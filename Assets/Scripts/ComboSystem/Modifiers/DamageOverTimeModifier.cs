namespace ComboSystem
{
    public class DamageOverTimeModifier : EffectOverTimeModifier<DamageOverTimeData>
    {
        public DamageOverTimeModifier(string id, DamageOverTimeData damageOverTimeData, ModifierProperties properties = default) : base(id, damageOverTimeData, properties)
        {
            Data = damageOverTimeData;
        }

        protected override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target!.DealDamage(Data.DamageData);
            return true;
        }
    }
}
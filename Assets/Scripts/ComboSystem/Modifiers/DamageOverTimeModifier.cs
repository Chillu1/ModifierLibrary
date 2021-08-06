namespace ComboSystem
{
    public class DamageOverTimeModifier : EffectOverTimeModifier<DamageOverTimeData>
    {
        public DamageOverTimeModifier(DamageOverTimeData damageOverTimeData) : base(damageOverTimeData)
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
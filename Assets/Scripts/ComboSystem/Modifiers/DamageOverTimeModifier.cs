namespace ComboSystem
{
    public class DamageOverTimeModifier : EffectOverTimeModifier<DamageOverTimeData>
    {
        public DamageOverTimeModifier(DamageOverTimeData damageOverTimeData) : base(damageOverTimeData)
        {
            Data = damageOverTimeData;
        }

        protected override void Apply()
        {
            Target.DealDamage(Data.DamageData);
        }
    }
}
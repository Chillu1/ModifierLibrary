namespace ComboSystem
{
    public class DamageOverTimeData : EffectOverTimeData
    {
        public DamageData[] DamageData { get; protected set; }

        public DamageOverTimeData(DamageData[] damageData, float everyXSecond, float duration)
            : base(EffectType.Damage, everyXSecond, duration)
        {
            DamageData = damageData;
        }
    }
}
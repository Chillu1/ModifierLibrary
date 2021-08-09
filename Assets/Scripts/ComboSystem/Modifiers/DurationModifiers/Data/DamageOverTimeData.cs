namespace ComboSystem
{
    public class DamageOverTimeData : EffectOverTimeData, IDamageData
    {
        public DamageData[] DamageData { get; protected set; }

        public DamageOverTimeData(DamageData[] damageData, float everyXSecond, float duration)
            : base(EffectType.Damage, everyXSecond, duration)
        {
            DamageData = damageData;
        }
    }
}
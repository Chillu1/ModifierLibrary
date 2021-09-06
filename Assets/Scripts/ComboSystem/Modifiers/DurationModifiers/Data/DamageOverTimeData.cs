using BaseProject;

namespace ComboSystem
{
    public class DamageOverTimeData : EffectOverTimeData
    {
        public Damages DamageData { get; protected set; }

        public DamageOverTimeData(Damages damageData, float everyXSecond, float duration)
            : base(EffectType.Damage, everyXSecond, duration)
        {
            DamageData = damageData;
        }
    }
}
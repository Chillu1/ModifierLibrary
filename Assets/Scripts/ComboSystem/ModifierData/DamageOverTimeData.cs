namespace ComboSystem
{
    public class DamageOverTimeData : EffectOverTimeData
    {
        public int Damage { get; protected set; }

        public DamageOverTimeData(int damage, float everyXSecond, float duration) : base(EffectType.Damage, everyXSecond, duration)
        {
            Damage = damage;
        }
    }
}
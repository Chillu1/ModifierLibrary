namespace ComboSystem
{
    public class DamageData//TODO RENAME
    {
        public float Damage;
        public DamageType DamageType;
    }

    public class Damages : IDamageData//TODO RENAME
    {
        public DamageData[] DamageData { get; set; }

        public Damages(DamageData damageData)
        {
            DamageData = new[] { damageData };
        }
        public Damages(float damage, DamageType damageType)
        {
            DamageData = new[] { new DamageData() { Damage = damage, DamageType = damageType } };
        }
        public Damages(DamageData[] damageData)
        {
            DamageData = damageData;
        }
    }
}
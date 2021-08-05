namespace ComboSystem
{
    public interface ICharacter
    {
        void RecalculateStats();
        void DealDamage(DamageData[] damageData);
    }
}
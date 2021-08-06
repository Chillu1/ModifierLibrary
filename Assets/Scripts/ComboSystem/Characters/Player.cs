namespace ComboSystem
{
    public class Player : Character
    {
        public Player()
        {
            Name = nameof(Player);
            MaxHealth = 10;
            Health = 10;
            MovementSpeed = 5;
        }

        public override void RecalculateStats()
        {
        }

        public override void DealDamage(DamageData[] damageData)
        {
            Health -= damageData[0].Damage;
        }

        public override bool IsValidTarget(Modifier modifier)
        {
            return true;
        }
    }
}
namespace ComboSystem
{
    public class Slime : Character
    {
        public Slime()
        {
            Name = nameof(Slime);
        }

        public override void RecalculateStats()
        {
        }

        public override void DealDamage(DamageData[] damageData)
        {
        }

        public override bool IsValidTarget(Modifier modifier)
        {
            return true;
        }
    }
}
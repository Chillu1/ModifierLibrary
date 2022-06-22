using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     LifeSteal before reduction (ignores resistances)
    /// </summary>
    public class LifeStealComponent : EffectComponent
    {
        //TODO Might be smart to make lifeSteal mechanic part of actual baseProject.Being class instead
        private DamageData[] Damage { get; }
        private double SummedDamage { get; }
        private double Percentage { get; }

        public LifeStealComponent(DamageData[] damage, double percentage, ConditionCheckData conditionCheckData = null) : base(
            conditionCheckData)
        {
            Damage = damage;
            Percentage = percentage;
            SummedDamage = Damage.Sum(d => d.Damage);
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.Stats.Health.Heal(SummedDamage * Percentage);
        }
    }
}
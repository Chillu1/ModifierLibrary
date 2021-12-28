using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     LifeSteal before reduction (ignores resistances)
    /// </summary>
    public class LifeStealComponent : EffectComponent, IConditionalEffectComponent
    {
        //TODO Might be smart to make lifeSteal mechanic part of actual baseProject.Being class instead
        private DamageData[] Damage { get; }
        private double Percentage { get; }

        public LifeStealComponent(DamageData[] damage, double percentage)
        {
            Damage = damage;
            Percentage = percentage;
        }

        public override void Effect()
        {
            //TODO TargetComponent
        }

        public void Effect(BaseBeing owner, BaseBeing healer)
        {
            owner.Heal(Damage.Sum(d => d.Damage) * Percentage, healer);
        }
    }
}
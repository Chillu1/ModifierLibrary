using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     LifeSteal before reduction (ignores resistances)
    /// </summary>
    public class LifeStealComponent : EffectComponent, IConditionEffectComponent
    {
        //TODO Might be smart to make lifeSteal mechanic part of actual baseProject.Being class instead
        private DamageData[] Damage { get; }
        private double SummedDamage { get; }
        private double Percentage { get; }

        private readonly ITargetComponent _targetComponent;

        public LifeStealComponent(DamageData[] damage, double percentage, ITargetComponent targetComponent)
        {
            Damage = damage;
            Percentage = percentage;
            _targetComponent = targetComponent;

            SummedDamage = Damage.Sum(d => d.Damage);
        }

        public override void Effect()
        {
            _targetComponent.Target.Stats.Health.Heal(SummedDamage * Percentage);
        }

        public void Effect(BaseBeing owner, BaseBeing healer)
        {
            _targetComponent.HandleTarget(owner, healer,
                (receiverLocal, acterLocal) => receiverLocal.Heal(SummedDamage * Percentage, acterLocal));
        }
    }
}
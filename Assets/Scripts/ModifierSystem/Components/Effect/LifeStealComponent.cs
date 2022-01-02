using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     LifeSteal before reduction (ignores resistances)
    /// </summary>
    public class LifeStealComponent : IEffectComponent, IConditionEffectComponent
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

        public void SimpleEffect()
        {
            _targetComponent.Target.Stats.Health.Heal(SummedDamage * Percentage);
        }

        public void ConditionEffect(BaseBeing receiver, BaseBeing healer)
        {
            _targetComponent.HandleTarget(receiver, healer,
                (receiverLocal, acterLocal) => receiverLocal.Heal(SummedDamage * Percentage, acterLocal));
        }
    }
}
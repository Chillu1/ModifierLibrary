using System;
using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     LifeSteal before reduction (ignores resistances)
    /// </summary>
    public class LifeStealComponent : IEffectComponent
    {
        //TODO Might be smart to make lifeSteal mechanic part of actual baseProject.Being class instead
        private DamageData[] Damage { get; }
        private double Percentage { get; }

        //Target is the being who gets lifeSteal buff
        private Func<IBeing> _getTarget;

        public LifeStealComponent(DamageData[] damage, double percentage, ITargetComponent targetComponent)
        {
            Damage = damage;
            Percentage = percentage;
            _getTarget = () => targetComponent.GetTarget();
        }

        public void Effect()
        {
            _getTarget().BaseBeing.AttackEvent += OnLifeStealEvent;
        }

        public void CleanUp()
        {
            _getTarget().BaseBeing.AttackEvent -= OnLifeStealEvent;
        }

        private void OnLifeStealEvent(BaseBeing target, BaseBeing attacked)
        {
            target.Heal(Damage.Sum(d => d.Damage) * Percentage);
        }
    }
}
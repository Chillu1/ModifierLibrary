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
        private readonly ITargetComponent _targetComponent;

        public LifeStealComponent(DamageData[] damage, double percentage, ITargetComponent targetComponent)
        {
            Damage = damage;
            Percentage = percentage;
            _targetComponent = targetComponent;
        }

        public void Effect()
        {
            _targetComponent.Target.AttackEvent += OnLifeStealEvent;
        }

        public void CleanUp()
        {
            _targetComponent.Target.AttackEvent -= OnLifeStealEvent;
        }

        private void OnLifeStealEvent(BaseBeing target, BaseBeing attacked)
        {
            target.Heal(Damage.Sum(d => d.Damage) * Percentage);
        }
    }
}
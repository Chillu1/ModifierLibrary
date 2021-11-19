using System;
using BaseProject;

namespace ComboSystemComposition
{
    public class DamageComponent : IEffectComponent
    {
        public DamageData[] Damage { get; private set; }
        private Func<IBeing> _getTarget;

        public DamageComponent(DamageData[] damage, ITargetComponent targetComponent)
        {
            Damage = damage;
            _getTarget = () => targetComponent.GetTarget();
        }

        public void Effect()
        {
            _getTarget().DealDamage(Damage);
        }
    }
}
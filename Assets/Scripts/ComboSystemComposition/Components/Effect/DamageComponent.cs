using System;

namespace ComboSystemComposition
{
    public class DamageComponent : IEffectComponent
    {
        //TODO Damage type to proper, elementalType, etc
        public double Damage { get; private set; }
        private Func<IBeing> _getTarget;

        public DamageComponent(double damage, ITargetComponent targetComponent)
        {
            Damage = damage;
            _getTarget = () => targetComponent.GetTarget();
        }

        public void Effect()
        {
            _getTarget.Invoke().DealDamage(Damage);
        }
    }
}
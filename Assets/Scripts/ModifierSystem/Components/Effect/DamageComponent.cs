using System;
using BaseProject;

namespace ModifierSystem
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
            //Log.Info(_getTarget().BaseBeing.Id);
            _getTarget().DealDamage(Damage);
        }

        public void IncreaseDamage(double damage)
        {
            Damage[0].BaseDamage+= damage;
        }
    }
}
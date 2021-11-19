using System;

namespace ComboSystemComposition
{
    public class HealComponent : IEffectComponent
    {
        public double Heal { get; private set; }
        private Func<IBeing> _getTarget;

        public HealComponent(double heal, ITargetComponent targetComponent)
        {
            Heal = heal;
            _getTarget = () => targetComponent.GetTarget();
        }

        public void Effect()
        {
            _getTarget().BaseBeing.Heal(Heal);
        }
    }
}
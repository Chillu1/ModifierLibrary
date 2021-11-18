using System;

namespace ComboSystemComposition
{
    public class StatComponent : IEffectComponent
    {
        //TODO Stat type to proper, health, damage, etc
        public double Health { get; private set; }
        private Func<IBeing> _getTarget;

        public StatComponent(double health, ITargetComponent targetComponent)
        {
            Health = health;
            _getTarget = () => targetComponent.GetTarget();
        }

        public void Effect()
        {
            _getTarget.Invoke().ChangeStat(Health);
        }
    }
}
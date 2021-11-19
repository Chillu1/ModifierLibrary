using System;
using BaseProject;

namespace ModifierSystem
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
            _getTarget.Invoke().BaseBeing.ChangeStat(StatType.Health, Health);//TODO Hardcoded health
        }
    }
}
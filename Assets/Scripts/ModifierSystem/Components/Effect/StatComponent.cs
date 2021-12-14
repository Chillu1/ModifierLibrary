using System;
using BaseProject;

namespace ModifierSystem
{
    public class StatComponent : IEffectComponent
    {
        //TODO Stat type to proper, health, damage, etc
        public double Health { get; private set; }
        private readonly ITargetComponent _targetComponent;

        public StatComponent(double health, ITargetComponent targetComponent)
        {
            Health = health;
            _targetComponent = targetComponent;
        }

        public void Effect()
        {
            _targetComponent.Target.BaseBeing.ChangeStat(StatType.Health, Health);//TODO Hardcoded health
        }
    }
}
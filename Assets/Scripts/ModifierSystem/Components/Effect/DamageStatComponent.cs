using BaseProject;

namespace ModifierSystem
{
    public class DamageStatComponent : EffectComponent, IConditionalEffectComponent
    {
        private readonly DamageData[] _damageData;
        private readonly ITargetComponent _targetComponent;

        public DamageStatComponent(DamageData[] damageData, ITargetComponent targetComponent)
        {
            _damageData = damageData;
            _targetComponent = targetComponent;
        }

        public override void Effect()
        {
            _targetComponent.Target.ChangeDamageStat(_damageData);
        }

        public void Effect(BaseBeing target, BaseBeing attacked)
        {
            target.Stats.ChangeDamageStat(_damageData);
        }
    }
}
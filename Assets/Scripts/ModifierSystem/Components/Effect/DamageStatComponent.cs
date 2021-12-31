using BaseProject;

namespace ModifierSystem
{
    public class DamageStatComponent : IEffectComponent, IConditionEffectComponent
    {
        private DamageData[] DamageData { get; }
        private readonly ITargetComponent _targetComponent;

        public DamageStatComponent(DamageData[] damageData, ITargetComponent targetComponent)
        {
            DamageData = damageData;
            _targetComponent = targetComponent;
        }

        public void Effect()
        {
            _targetComponent.Target.ChangeDamageStat(DamageData);
        }

        public void Effect(BaseBeing target, BaseBeing attacked)
        {
            _targetComponent.HandleTarget(target, attacked,
                (receiverLocal, acterLocal) => ((Being)receiverLocal).ChangeDamageStat(DamageData));
        }
    }
}
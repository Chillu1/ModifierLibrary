using BaseProject;

namespace ModifierSystem
{
    public class DamageStatComponent : EffectComponent, IConditionEffectComponent
    {
        private DamageData[] DamageData { get; }
        private readonly ITargetComponent _targetComponent;

        public DamageStatComponent(DamageData[] damageData, ITargetComponent targetComponent)
        {
            DamageData = damageData;
            _targetComponent = targetComponent;
        }

        public override void Effect()
        {
            _targetComponent.Target.ChangeDamageStat(DamageData);
        }

        public void Effect(BaseBeing target, BaseBeing attacked)
        {
            _targetComponent.HandleTarget(target, attacked,
                (receiverLocal, acterLocal) => receiverLocal.Stats.ChangeDamageStat(DamageData));
        }
    }
}
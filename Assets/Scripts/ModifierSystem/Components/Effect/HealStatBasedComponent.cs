using BaseProject;

namespace ModifierSystem
{
    public class HealStatBasedComponent : IEffectComponent, IConditionEffectComponent
    {
        private readonly ITargetComponent _targetComponent;

        public HealStatBasedComponent(ITargetComponent targetComponent)
        {
            _targetComponent = targetComponent;
        }

        public void SimpleEffect()
        {
            _targetComponent.Target.Heal(_targetComponent.Owner);
            //_targetComponent.Target.Heal(_targetComponent.Owner.Stats.HealStat, _targetComponent.Owner.BaseBeing);
        }

        public void ConditionEffect(BaseBeing receiver, BaseBeing healer)
        {
            _targetComponent.HandleTarget(receiver, healer,
                (receiverLocal, acterLocal) => receiverLocal.Heal(receiverLocal, acterLocal, false));
        }
    }
}
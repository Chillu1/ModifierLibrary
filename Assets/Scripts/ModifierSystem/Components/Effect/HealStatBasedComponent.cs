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

        public void Effect()
        {
            _targetComponent.Target.Heal(_targetComponent.Owner);
            //_targetComponent.Target.Heal(_targetComponent.Owner.Stats.HealStat, _targetComponent.Owner.BaseBeing);
        }

        public void Effect(BaseBeing owner, BaseBeing healer)
        {
            _targetComponent.HandleTarget(owner, healer,
                (receiverLocal, acterLocal) => receiverLocal.Heal(receiverLocal, acterLocal, false));
        }
    }
}
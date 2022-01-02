using BaseProject;

namespace ModifierSystem
{
    public abstract class EffectComponent : IEffectComponent, IConditionEffectComponent
    {
        private ConditionBeingStatus Status { get; }
        private readonly ITargetComponent _targetComponent;

        protected EffectComponent(ITargetComponent targetComponent, ConditionBeingStatus status = ConditionBeingStatus.None)
        {
            _targetComponent = targetComponent;
            Status = status;
        }

        protected abstract void ActualEffect(BaseBeing receiver, BaseBeing acter, bool triggerEvents);

        public void SimpleEffect()
        {
            Effect(_targetComponent.Target, _targetComponent.Owner, true);
        }

        private void Effect(BaseBeing receiver, BaseBeing acter, bool triggerEvents)
        {
            if (Status != ConditionBeingStatus.None)
            {
                var beingCondition = ConditionGenerator.GenerateBeingCondition(Status);
                if (!beingCondition(receiver, acter))
                    return;
            }

            ActualEffect(receiver, acter, triggerEvents);
        }

        public void ConditionEffect(BaseBeing receiver, BaseBeing acter)
        {
            _targetComponent.HandleTarget(receiver, acter, EventEffect);

            void EventEffect(BaseBeing receiverLocal, BaseBeing acterLocal)
            {
                Effect(receiverLocal, acterLocal, false);
            }
        }
    }
}
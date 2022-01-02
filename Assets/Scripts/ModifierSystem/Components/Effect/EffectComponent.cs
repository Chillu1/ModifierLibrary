using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public abstract class EffectComponent : IEffectComponent, IConditionEffectComponent
    {
        [CanBeNull] private ConditionCheckData ConditionCheckData { get; }
        private readonly ITargetComponent _targetComponent;

        protected EffectComponent(ITargetComponent targetComponent, ConditionCheckData conditionCheckData = null)
        {
            _targetComponent = targetComponent;
            ConditionCheckData = conditionCheckData;
        }

        protected abstract void ActualEffect(BaseBeing receiver, BaseBeing acter, bool triggerEvents);

        /// <summary>
        ///     No conditions, just effect
        /// </summary>
        public void SimpleEffect()
        {
            Effect(_targetComponent.Target, _targetComponent.Owner, true);
        }

        /// <summary>
        ///     Main effect helper, checks for CheckCondition
        /// </summary>
        private void Effect(BaseBeing receiver, BaseBeing acter, bool triggerEvents)
        {
            if (ConditionCheckData != null)
            {
                var beingCondition = ConditionGenerator.GenerateBeingCondition(ConditionCheckData);
                if (!beingCondition(receiver, acter))
                    return;
            }

            ActualEffect(receiver, acter, triggerEvents);
        }

        /// <summary>
        ///     Being Event based condition effect
        /// </summary>
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
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public abstract class EffectComponent : IEffectComponent, IConditionEffectComponent
    {
        [CanBeNull] private ConditionCheckData ConditionCheckData { get; }
        private bool IsRevertible { get; }
        private ITargetComponent _targetComponent;

        public string Info { get; protected set; }

        protected EffectComponent(ConditionCheckData conditionCheckData = null, bool isRevertible = false)
        {
            ConditionCheckData = conditionCheckData;
            IsRevertible = isRevertible;

            Info = "Unset effect data " + "\n";
        }

        public void Setup(ITargetComponent targetComponent)
        {
            _targetComponent = targetComponent;
        }

        protected abstract void ActualEffect(BaseBeing receiver, BaseBeing acter);

        /// <summary>
        ///     No conditions, just effect
        /// </summary>
        public void SimpleEffect()
        {
            Effect(_targetComponent.Target, _targetComponent.Owner);
        }

        /// <summary>
        ///     Main effect helper, checks for CheckCondition
        /// </summary>
        private void Effect(BaseBeing receiver, BaseBeing acter)
        {
            if (ConditionCheckData != null)
            {
                var beingCondition = ConditionGenerator.GenerateBeingCondition(ConditionCheckData);
                if (!beingCondition(receiver, acter))
                    return;
            }

            ActualEffect(receiver, acter);
        }

        /// <summary>
        ///     Being Event based condition effect
        /// </summary>
        public void ConditionEffect(BaseBeing receiver, BaseBeing acter)
        {
            _targetComponent.HandleTarget(receiver, acter, Effect);
        }

        public void RevertEffect()
        {
            if (IsRevertible)
                RevertEffect(_targetComponent.Target, _targetComponent.Owner);
        }

        protected virtual void RevertEffect(BaseBeing receiver, BaseBeing acter)
        {
        }
    }
}
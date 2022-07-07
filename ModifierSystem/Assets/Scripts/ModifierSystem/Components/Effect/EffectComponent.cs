using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public abstract class EffectComponent : IEffectComponent, IConditionEffectComponent
    {
        [CanBeNull] private ConditionCheckData ConditionCheckData { get; }
        private bool IsRevertible { get; }
        private ITargetComponent _targetComponent;
        protected Unit ApplierOwner => _targetComponent.ApplierOwner;//TODO TEMP, to make Taunt work... Feeding original applier

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

        protected abstract void Effect(BaseProject.Unit receiver, BaseProject.Unit acter);

        /// <summary>
        ///     No conditions, just effect
        /// </summary>
        public void SimpleEffect()
        {
            TryEffect(_targetComponent.Target, _targetComponent.Owner);
        }

        /// <summary>
        ///     Main effect helper, checks for CheckCondition
        /// </summary>
        private void TryEffect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            if (ConditionCheckData != null)
            {
                var unitCondition = ConditionGenerator.GenerateUnitCondition(ConditionCheckData);
                if (!unitCondition(receiver, acter))
                    return;
            }

            Effect(receiver, acter);
        }

        /// <summary>
        ///     Unit Event based condition effect
        /// </summary>
        public void ConditionEffect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            _targetComponent.HandleTarget(receiver, acter, TryEffect);
        }

        public void RevertEffect()
        {
            if (IsRevertible)
                RevertEffect(_targetComponent.Target, _targetComponent.Owner);
        }

        protected virtual void RevertEffect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            Log.Error("RevertEffect not overridden, but we're trying to use IsRevertible logic");
        }
    }
}
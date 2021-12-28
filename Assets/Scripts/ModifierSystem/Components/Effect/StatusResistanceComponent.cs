using BaseProject;

namespace ModifierSystem
{
    public class StatusResistanceComponent : IEffectComponent, IConditionEffectComponent
    {
        private StatusTag[] StatusTags { get; }
        private double[] Values { get; }

        private readonly ITargetComponent _targetComponent;

        public StatusResistanceComponent(StatusTag[] tags, double[] values, ITargetComponent targetComponent)
        {
            if (tags.Length != values.Length)
                Log.Error("Status tags wrong length for tags/values", "modifiers");
            StatusTags = tags;
            Values = values;
            _targetComponent = targetComponent;
        }

        public void Effect()
        {
            _targetComponent.Target.StatusResistances.ChangeValue(StatusTags, Values);
        }

        public void Effect(BaseBeing owner, BaseBeing acter)
        {
            _targetComponent.HandleTarget(owner, acter,
                (receiverLocal, acterLocal) => receiverLocal.StatusResistances.ChangeValue(StatusTags, Values));
        }
    }
}
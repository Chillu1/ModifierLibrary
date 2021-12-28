using BaseProject;

namespace ModifierSystem
{
    public class StatusResistanceComponent : EffectComponent
    {
        private StatusTag[] StatusTags { get; }
        private double[] Values { get; }

        private ITargetComponent TargetComponent { get; }

        public StatusResistanceComponent(StatusTag[] tags, double[] values, ITargetComponent targetComponent)
        {
            if (tags.Length != values.Length)
                Log.Error("Status tags wrong length for tags/values", "modifiers");
            StatusTags = tags;
            Values = values;
            TargetComponent = targetComponent;
        }

        public override void Effect()
        {
            for (int i = 0; i < StatusTags.Length; i++)
            {
                TargetComponent.Target.StatusResistances.ChangeValue(StatusTags[i], Values[i]);
            }
        }
    }
}
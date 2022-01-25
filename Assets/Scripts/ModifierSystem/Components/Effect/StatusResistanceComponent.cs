using BaseProject;

namespace ModifierSystem
{
    public class StatusResistanceComponent : EffectComponent
    {
        private StatusTag[] StatusTags { get; }
        private double[] Values { get; }

        public StatusResistanceComponent(StatusTag[] tags, double[] values, ITargetComponent targetComponent) : base(targetComponent)
        {
            if (tags.Length != values.Length)
                Log.Error("Status tags wrong length for tags/values", "modifiers");
            StatusTags = tags;
            Values = values;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            ((Being)receiver).StatusResistances.ChangeValue(StatusTags, Values);
        }
    }
}
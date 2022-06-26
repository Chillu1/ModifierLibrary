using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    public class StatusResistanceComponent : EffectComponent
    {
        private StatusTag[] StatusTags { get; }
        private double[] Values { get; }

        public StatusResistanceComponent(StatusTag[] statusTags, double[] values, ConditionCheckData conditionCheckData = null) : base(
            conditionCheckData)
        {
            if (statusTags.Length != values.Length)
                Log.Error("Status tags wrong length for tags/values", "modifiers");
            StatusTags = statusTags;
            Values = values;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            ((Being)receiver).StatusResistances.ChangeValue(StatusTags, Values);
        }

        protected override void RemoveEffect(BaseBeing receiver, BaseBeing acter)
        {
            ((Being)receiver).StatusResistances.ChangeValue(StatusTags, Values.Select(v => -v).ToArray());
        }
    }
}
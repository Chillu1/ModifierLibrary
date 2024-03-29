using System.Linq;
using UnitLibrary;

namespace ModifierLibrary
{
	public sealed class StatusResistanceComponent : EffectComponent
	{
		private StatusTag[] StatusTags { get; }
		private double[] Values { get; }

		public StatusResistanceComponent(StatusTag[] statusTags, double[] values, ConditionCheckData conditionCheckData = null,
			bool isRevertible = false) : base(conditionCheckData, isRevertible)
		{
			if (statusTags.Length != values.Length)
				Log.Error("Status tags wrong length for tags/values", "modifiers");
			StatusTags = statusTags;
			Values = values;

			Info = $"StatusResistance: {string.Join<StatusTag>(", ", StatusTags)}{string.Join(", ", Values)}\n";
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			receiver.StatusResistances.ChangeValue(StatusTags, Values);
		}

		protected override void RevertEffect(Unit receiver, Unit acter)
		{
			receiver.StatusResistances.ChangeValue(StatusTags, Values.Select(v => -v).ToArray());
		}
	}
}
using System.Linq;
using UnitLibrary;

namespace ModifierLibrary
{
	public sealed class StatMultiplierComponent : EffectComponent
	{
		private (StatType type, double multiplier)[] Stats { get; }

		public StatMultiplierComponent(StatType type, double multiplier, ConditionCheckData conditionCheckData = null,
			bool isRevertible = false) : base(conditionCheckData, isRevertible)
		{
			Stats = new[] { (type, multiplier) };

			Info = $"Stat: {Stats[0].type} {Stats[0].multiplier}";
		}

		public StatMultiplierComponent((StatType type, double multiplier)[] stats, ConditionCheckData conditionCheckData = null,
			bool isRevertible = false) : base(conditionCheckData, isRevertible)
		{
			Stats = stats;

			Info = $"Stats: {string.Join(", ", Stats.Select(t => $"{t.type} {t.multiplier}"))}";
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			receiver.ChangeStatMultiplier(Stats);
		}

		protected override void RevertEffect(Unit receiver, Unit acter)
		{
			var negativeStats = new (StatType type, double multiplier)[Stats.Length];
			for (int i = 0; i < Stats.Length; i++)
			{
				var damageData = Stats[i];
				negativeStats[i] = (damageData.type, -damageData.multiplier);
			}

			receiver.ChangeStatMultiplier(negativeStats);
		}
	}
}
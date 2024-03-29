using UnitLibrary;

namespace ModifierLibrary
{
	public sealed class DamageResistanceComponent : EffectComponent
	{
		private DamageType DamageType { get; }
		private double Value { get; }

		public DamageResistanceComponent(DamageType damageType, double value, ConditionCheckData conditionCheckData = null,
			bool isRevertible = false) : base(conditionCheckData, isRevertible)
		{
			DamageType = damageType;
			Value = value;

			Info = $"Damage Resistance: {damageType} {value}\n";
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			receiver.DamageTypeDamageResistances.Change(DamageType, Value);
		}

		protected override void RevertEffect(Unit receiver, Unit acter)
		{
			receiver.DamageTypeDamageResistances.Change(DamageType, -Value);
		}
	}
}
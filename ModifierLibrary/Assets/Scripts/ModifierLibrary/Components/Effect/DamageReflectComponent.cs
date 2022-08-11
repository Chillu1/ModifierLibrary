using System.Linq;
using UnitLibrary;

namespace ModifierLibrary
{
	public sealed class DamageReflectComponent : EffectComponent
	{
		private double Percentage { get; }

		public DamageReflectComponent(double percentage, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
		{
			Percentage = percentage;

			Info = $"Damage Reflect: {percentage * 100d}%\n";
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			DamageData[] reflectDamage = receiver.Stats.Damage;
			foreach (var data in reflectDamage)
				data.BaseDamage *= Percentage;
			receiver.DealDamage(reflectDamage, acter, AttackType.Reflect);
		}
	}
}
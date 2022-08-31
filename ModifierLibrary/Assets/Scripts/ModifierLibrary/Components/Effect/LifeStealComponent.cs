using System.Linq;
using UnitLibrary;

namespace ModifierLibrary
{
	/// <summary>
	///     LifeSteal before reduction (ignores resistances)
	/// </summary>
	public sealed class LifeStealComponent : EffectComponent
	{
		//TODO Might be smart to make lifeSteal mechanic part of actual UnitLibrary.Unit class instead
		//private DamageData[] Damage { get; }
		private double SummedDamage { get; }
		private double Percentage { get; }

		public LifeStealComponent(DamageData[] damage, double percentage, ConditionCheckData conditionCheckData = null) : base(
			conditionCheckData)
		{
			//Damage = damage;
			Percentage = percentage;
			SummedDamage = damage.Sum(d => d.Damage);

			Info = $"LifeSteal: {SummedDamage} damage, {Percentage * 100d}%\n";
		}

		protected override void Effect(Unit receiver, Unit acter/*, object data*/)
		{
			//if data = struct DamageDealtData, receiver.Heal(data * Percentage), acter). But tbh prob just better to implement this in UnitLibrary.Unit class
			receiver.Heal(SummedDamage * Percentage, acter);
		}
	}
}
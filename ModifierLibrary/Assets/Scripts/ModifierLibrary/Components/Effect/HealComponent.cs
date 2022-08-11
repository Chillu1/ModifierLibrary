namespace ModifierLibrary
{
	public sealed class HealComponent : EffectComponent
	{
		private double Heal { get; }

		public HealComponent(double heal, ConditionCheckData conditionCheckData = null, bool isRevertible = false)
			: base(conditionCheckData, isRevertible)
		{
			Heal = heal;

			Info = $"Heal: {Heal}\n";
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			receiver.Heal(Heal, acter);
		}

		protected override void RevertEffect(Unit receiver, Unit acter)
		{
			receiver.Heal(-Heal, acter);
		}
	}
}
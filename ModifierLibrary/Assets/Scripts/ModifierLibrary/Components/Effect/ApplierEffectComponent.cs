namespace ModifierLibrary
{
	public sealed class ApplierEffectComponent : EffectComponent, IStackEffectComponent
	{
		private Modifier Modifier { get; }
		private bool IsStackEffect { get; }

		public ApplierEffectComponent(Modifier modifier, bool isStackEffect = false, ConditionCheckData conditionCheckData = null) :
			base(conditionCheckData)
		{
			Modifier = modifier;
			IsStackEffect = isStackEffect;

			string info = Modifier.Info != null
				? $"Applies: {Modifier.Info.DisplayName}\n{Modifier.Info.EffectInfo}{Modifier.Info.BasicCheckInfo}"
				: "Applies: " + modifier;
			Info = info;
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			receiver.AddModifier((Modifier)Modifier.Clone(), acter);
		}

		public void StackEffect(int stacks, double value)
		{
			if (IsStackEffect)
				SimpleEffect();
		}
	}
}
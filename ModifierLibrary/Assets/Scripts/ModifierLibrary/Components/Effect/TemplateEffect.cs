namespace ModifierLibrary
{
	public sealed class TemplateEffect : EffectComponent
	{
		public double Foo { get; }

		public TemplateEffect(double foo, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
		{
			Foo = foo;

			Info = $"Template: {Foo}\n";
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			//receiver.Function(Foo, acter);
		}
	}
}
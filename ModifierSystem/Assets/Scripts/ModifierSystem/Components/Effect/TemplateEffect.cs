using BaseProject;

namespace ModifierSystem
{
    public sealed class TemplateEffect : EffectComponent
    {
        public double Foo { get; }

        public TemplateEffect(double foo, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Foo = foo;

            Info = $"Template: {Foo}\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            //receiver.Function(Foo, acter);
        }
    }

    public sealed class TestEffect : EffectComponent
    {
        public TestEffect(DamageType damageType, double value/*, ConditionCheckData data = null*/) : base(null, false)
        {
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
        }
    }
}
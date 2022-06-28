using BaseProject;

namespace ModifierSystem
{
    public class TemplateEffect : EffectComponent
    {
        public double Foo { get; }

        public TemplateEffect(double foo, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Foo = foo;

            Info = $"Template: {Foo}\n";
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            //receiver.Function(Foo, acter);
        }
    }
}
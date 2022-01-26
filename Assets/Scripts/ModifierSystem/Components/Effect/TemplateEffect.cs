using BaseProject;

namespace ModifierSystem
{
    public class TemplateEffect : EffectComponent
    {
        public double Foo { get; }

        public TemplateEffect(double foo, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Foo = foo;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            //receiver.Function(Foo, acter);
        }
    }
}
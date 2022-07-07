using BaseProject;

namespace ModifierSystem
{
    public sealed class AttackComponent : EffectComponent
    {
        public AttackComponent(IBaseEffectProperties baseProperties = null) : base(baseProperties)
        {
            Info = "Attack\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            Unit.Attack((Unit)receiver, (Unit)acter); //TODO Not sure about this
        }
    }
}
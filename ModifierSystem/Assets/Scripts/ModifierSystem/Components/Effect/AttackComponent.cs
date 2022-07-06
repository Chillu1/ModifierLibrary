using BaseProject;

namespace ModifierSystem
{
    public class AttackComponent : EffectComponent
    {
        public AttackComponent(ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Info = "Attack\n";
        }

        protected override void Effect(BaseBeing receiver, BaseBeing acter)
        {
            Being.Attack((Being)receiver, (Being)acter); //TODO Not sure about this
        }
    }
}
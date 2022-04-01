using BaseProject;

namespace ModifierSystem
{
    public class AttackComponent : EffectComponent
    {
        public AttackComponent(ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            Being.Attack((Being)receiver, (Being)acter); //TODO Not sure about this
        }
    }
}
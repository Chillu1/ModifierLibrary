using BaseProject;

namespace ModifierSystem
{
    public class AttackComponent : EffectComponent
    {
        public AttackComponent(ITargetComponent targetComponent, ConditionCheckData conditionCheckData = null)
            : base(targetComponent, conditionCheckData)
        {
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            acter.Attack(receiver);//TODO Not sure about this
        }
    }
}
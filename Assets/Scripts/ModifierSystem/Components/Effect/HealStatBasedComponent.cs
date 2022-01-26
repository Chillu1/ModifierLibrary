using BaseProject;

namespace ModifierSystem
{
    public class HealStatBasedComponent : EffectComponent
    {
        public HealStatBasedComponent(ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.Heal(receiver, acter);
        }
    }
}
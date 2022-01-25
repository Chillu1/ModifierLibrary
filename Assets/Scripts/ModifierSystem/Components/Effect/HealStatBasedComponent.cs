using BaseProject;

namespace ModifierSystem
{
    public class HealStatBasedComponent : EffectComponent
    {
        public HealStatBasedComponent(ITargetComponent targetComponent) : base(targetComponent)
        {
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.Heal(receiver, acter);
        }
    }
}
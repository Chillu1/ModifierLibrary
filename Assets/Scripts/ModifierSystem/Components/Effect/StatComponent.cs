using BaseProject;

namespace ModifierSystem
{
    public class StatComponent : EffectComponent
    {
        private Stat[] Stats { get; }

        public StatComponent(Stat[] stats, ITargetComponent targetComponent) : base(targetComponent)
        {
            Stats = stats;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter, bool triggerEvents)
        {
            ((Being)receiver).ChangeStat(Stats);
        }
    }
}
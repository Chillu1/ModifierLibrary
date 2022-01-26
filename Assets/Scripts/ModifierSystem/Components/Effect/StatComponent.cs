using BaseProject;

namespace ModifierSystem
{
    public class StatComponent : EffectComponent
    {
        private Stat[] Stats { get; }

        public StatComponent(Stat[] stats, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Stats = stats;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            ((Being)receiver).ChangeStat(Stats);
        }
    }
}
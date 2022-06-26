using System.Linq;
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

        //protected override void RemoveEffect(BaseBeing receiver, BaseBeing acter)
        //{
        //    ((Being)receiver).ChangeStat(Stats.Select(s => s.BaseValue = -s.BaseValue));
        //}
    }
}
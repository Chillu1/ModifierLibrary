using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    public sealed class StatMultiplierComponent : EffectComponent
    {
        private (StatType type, double multiplier)[] Stats { get; }

        public StatMultiplierComponent(StatType type, double multiplier, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Stats = new[] { (type, multiplier) };

            Info = $"Stat: {Stats[0].type} {Stats[0].multiplier}";
        }

        public StatMultiplierComponent((StatType type, double multiplier)[] stats, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Stats = stats;

            Info = $"Stats: {string.Join(", ", Stats.Select(t => $"{t.type} {t.multiplier}"))}";
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            ((Being)receiver).ChangeStatMultiplier(Stats);
        }

        //protected override void RevertEffect(BaseBeing receiver, BaseBeing acter)
        //{
        //    ((Being)receiver).ChangeStat(Stats.Select(t => t.value = -t.value).Cast<(StatType type, double value)>().ToArray());
        //}
    }
}
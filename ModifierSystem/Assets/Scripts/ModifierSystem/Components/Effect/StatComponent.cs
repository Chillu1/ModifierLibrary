using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    public sealed class StatComponent : EffectComponent
    {
        private (StatType type, double value)[] Stats { get; }

        public StatComponent(StatType type, double value, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Stats = new[] { (type, value) };

            Info = $"Stat: {Stats[0].type} {Stats[0].value}";
        }

        public StatComponent((StatType type, double value)[] stats, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Stats = stats;

            Info = $"Stats: {string.Join(", ", Stats.Select(t => $"{t.type} {t.value}"))}";
        }

        protected override void Effect(Unit receiver, Unit acter)
        {
            receiver.ChangeStat(Stats);
        }

        //protected override void RevertEffect(Unit receiver, Unit acter)
        //{
        //    ((Unit)receiver).ChangeStat(Stats.Select(t => t.value = -t.value).Cast<(StatType type, double value)>().ToArray());
        //}
    }
}
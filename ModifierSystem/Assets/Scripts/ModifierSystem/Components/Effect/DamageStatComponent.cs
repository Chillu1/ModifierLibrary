using System.Linq;
using BaseProject;

namespace ModifierSystem
{
    public class DamageStatComponent : EffectComponent
    {
        private DamageData[] DamageData { get; }

        public DamageStatComponent(DamageData[] damageData, ConditionCheckData conditionCheckData = null, bool isRevertible = false) :
            base(conditionCheckData, isRevertible)
        {
            DamageData = damageData;

            Info = $"DamageStat: {string.Join<DamageData>(", ", damageData)}\n";
        }

        protected override void Effect(BaseBeing receiver, BaseBeing acter)
        {
            ((Being)receiver).ChangeDamageStat(DamageData);
        }

        protected override void RevertEffect(BaseBeing receiver, BaseBeing acter)
        {
            ((Being)receiver).ChangeDamageStat(DamageData.Select(d =>
            {
                d.BaseDamage = -d.BaseDamage;
                return d;
            }).ToArray());
        }
    }
}
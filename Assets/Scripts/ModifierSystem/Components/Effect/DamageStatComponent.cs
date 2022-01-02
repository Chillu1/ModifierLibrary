using BaseProject;

namespace ModifierSystem
{
    public class DamageStatComponent : EffectComponent
    {
        private DamageData[] DamageData { get; }

        public DamageStatComponent(DamageData[] damageData, ITargetComponent targetComponent,
            ConditionCheckData conditionCheckData = null) : base(targetComponent, conditionCheckData)
        {
            DamageData = damageData;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter, bool triggerEvents)
        {
            ((Being)receiver).ChangeDamageStat(DamageData);
        }
    }
}
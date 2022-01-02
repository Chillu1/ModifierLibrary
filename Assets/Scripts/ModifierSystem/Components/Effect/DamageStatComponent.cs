using BaseProject;

namespace ModifierSystem
{
    public class DamageStatComponent : IEffectComponent, IConditionEffectComponent
    {
        private DamageData[] DamageData { get; }
        private ConditionBeingStatus Status { get; }
        private readonly ITargetComponent _targetComponent;

        public DamageStatComponent(DamageData[] damageData, ITargetComponent targetComponent,
            ConditionBeingStatus status = ConditionBeingStatus.None)
        {
            DamageData = damageData;
            _targetComponent = targetComponent;
            Status = status;
        }

        private void DamageStatEffect(BaseBeing receiver, BaseBeing acter)
        {
            if(Status != ConditionBeingStatus.None)
            {
                var beingCondition = ConditionGenerator.GenerateBeingCondition(Status);
                if (!beingCondition(receiver, acter))
                    return;
            }

            ((Being)receiver).ChangeDamageStat(DamageData);
        }

        public void SimpleEffect()
        {
            DamageStatEffect(_targetComponent.Target, null);
        }

        public void ConditionEffect(BaseBeing receiver, BaseBeing acter)
        {
            _targetComponent.HandleTarget(receiver, acter, DamageStatEffect);
        }
    }
}
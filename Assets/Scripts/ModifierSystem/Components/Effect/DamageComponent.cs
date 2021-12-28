using BaseProject;

namespace ModifierSystem
{
    public class DamageComponent : EffectComponent, IConditionEffectComponent, IMetaEffect
    {
        private DamageData[] Damage { get; }
        private readonly ITargetComponent _targetComponent;

        public DamageComponent(DamageData[] damage, ITargetComponent targetComponent)
        {
            Damage = damage;
            _targetComponent = targetComponent;
        }

        public override void Effect()
        {
            //Log.Info(_getTarget().BaseBeing.Id);
            _targetComponent.Target.DealDamage(Damage, _targetComponent.Owner);
        }

        public void Effect(BaseBeing owner, BaseBeing acter)
        {
            _targetComponent.HandleTarget(owner, acter,
                (receiverLocal, acterLocal) => receiverLocal.DealDamage(Damage, acterLocal));
        }

        public void MetaEffect(ChangeType changeType, double value)
        {
            switch (changeType)
            {
                case ChangeType.None:
                    Log.Error("Wrong ChangeType");
                    break;
                case ChangeType.AdditiveIncrease:
                    Damage[0].BaseDamage += value;
                    break;
                case ChangeType.Multiply:
                    Damage[0].Multiplier += value;
                    break;
                case ChangeType.EveryXStack:
                    break;
            }
        }
    }
}
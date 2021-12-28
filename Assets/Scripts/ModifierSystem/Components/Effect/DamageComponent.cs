using BaseProject;

namespace ModifierSystem
{
    public class DamageComponent : EffectComponent, IConditionalEffectComponent, IMetaEffect
    {
        private DamageData[] Damage { get; }
        private ITargetComponent TargetComponent { get; }

        public DamageComponent(DamageData[] damage, ITargetComponent targetComponent)
        {
            Damage = damage;
            TargetComponent = targetComponent;
        }

        public override void Effect()
        {
            //Log.Info(_getTarget().BaseBeing.Id);
            TargetComponent.Target.DealDamage(Damage, TargetComponent.Owner);
        }

        public void Effect(BaseBeing owner, BaseBeing acter)
        {
            switch (TargetComponent.ConditionalTarget)
            {
                case ConditionalTarget.Self:
                    owner.DealDamage(Damage, acter);
                    break;
                case ConditionalTarget.Acter:
                    acter.DealDamage(Damage, owner);
                    break;
            }
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
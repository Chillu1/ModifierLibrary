using BaseProject;

namespace ModifierSystem
{
    public sealed class DamageComponent : EffectComponent, IStackEffectComponent
    {
        private DamageData[] Damage { get; }
        private DamageComponentStackEffect StackEffectType { get; }

        public DamageComponent(DamageData[] damage, ITargetComponent targetComponent,
            DamageComponentStackEffect stackEffectType = DamageComponentStackEffect.None,
            ConditionBeingStatus status = ConditionBeingStatus.None) : base(targetComponent, status)
        {
            Damage = damage;
            StackEffectType = stackEffectType;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter, bool triggerEvents)
        {
            receiver.DealDamage(Damage, acter, triggerEvents);
        }

        public void StackEffect(int stacks, double value)
        {
            switch (StackEffectType)
            {
                case DamageComponentStackEffect.Effect:
                    SimpleEffect();
                    break;
                case DamageComponentStackEffect.Add:
                    Damage[0].BaseDamage += value;
                    break;
                case DamageComponentStackEffect.AddStacksBased:
                    Damage[0].BaseDamage += value * stacks;
                    break;
                case DamageComponentStackEffect.Multiply:
                    Damage[0].Multiplier += value;
                    break;
                case DamageComponentStackEffect.MultiplyStacksBased:
                    Damage[0].Multiplier += value * stacks;
                    break;
                case DamageComponentStackEffect.OnXStacksAddElemental:
                    //TODO
                    //Damage[0].ElementData.?
                    break;
                default:
                    Log.Error($"StackEffectType {StackEffectType} unsupported for {GetType()}");
                    return;
            }
        }

        public enum DamageComponentStackEffect
        {
            None = 0,
            Effect,
            Add, //TODO Add to all damages?
            AddStacksBased,
            Multiply, //TODO Multiply all damages?
            MultiplyStacksBased,

            //DamageComponent specific
            OnXStacksAddElemental,
        }
    }
}
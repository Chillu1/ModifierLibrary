using BaseProject;

namespace ModifierSystem
{
    public class DamageComponent : IEffectComponent, IConditionEffectComponent, IStackEffectComponent
    {
        private DamageData[] Damage { get; }
        private DamageComponentStackEffect StackEffect { get; }
        private readonly ITargetComponent _targetComponent;

        public DamageComponent(DamageData[] damage, ITargetComponent targetComponent,
            DamageComponentStackEffect stackEffect = DamageComponentStackEffect.None)
        {
            Damage = damage;
            _targetComponent = targetComponent;
            StackEffect = stackEffect;
        }

        public void Effect()
        {
            _targetComponent.Target.DealDamage(Damage, _targetComponent.Owner);
        }

        public void Effect(BaseBeing owner, BaseBeing acter)
        {
            _targetComponent.HandleTarget(owner, acter,
                (receiverLocal, acterLocal) => receiverLocal.DealDamage(Damage, acterLocal));
        }

        public void Effect(int stacks, double value)
        {
            switch (StackEffect)
            {
                case DamageComponentStackEffect.Effect:
                    Effect();
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
                    Log.Error($"StackEffectType {StackEffect} unsupported for {GetType()}");
                    return;
            }
        }

        public enum DamageComponentStackEffect
        {
            None = 0,
            Effect,
            Add,//TODO Add to all damages?
            AddStacksBased,
            Multiply,//TODO Multiply all damages?
            MultiplyStacksBased,

            //DamageComponent specific
            OnXStacksAddElemental,
        }
    }
}
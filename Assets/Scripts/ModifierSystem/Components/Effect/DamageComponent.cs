using System;
using BaseProject;

namespace ModifierSystem
{
    public class DamageComponent : IEffectComponent, IConditionEffectComponent, IStackEffectComponent
    {
        private DamageData[] Damage { get; }
        private DamageComponentStackEffects StackEffects { get; }
        private readonly ITargetComponent _targetComponent;

        public DamageComponent(DamageData[] damage, ITargetComponent targetComponent,
            DamageComponentStackEffects stackEffects = DamageComponentStackEffects.None)
        {
            Damage = damage;
            _targetComponent = targetComponent;
            StackEffects = stackEffects;
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

        public void StackEffect(int stacks, double value)
        {
            switch (StackEffects)
            {
                case DamageComponentStackEffects.Effect:
                    Effect();
                    break;
                case DamageComponentStackEffects.Add:
                    Damage[0].BaseDamage += value;
                    break;
                case DamageComponentStackEffects.AddStacksBased:
                    Damage[0].BaseDamage += value * stacks;
                    break;
                case DamageComponentStackEffects.Multiply:
                    Damage[0].Multiplier += value;
                    break;
                case DamageComponentStackEffects.MultiplyStacksBased:
                    Damage[0].Multiplier += value * stacks;
                    break;
                case DamageComponentStackEffects.OnXStacksAddElemental:
                    //TODO
                    //Damage[0].ElementData.?
                    break;
                default:
                    Log.Error($"StackEffectType {StackEffects} unsupported for {GetType()}");
                    return;
            }
        }

        public enum DamageComponentStackEffects
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
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
                    StackDamageEffect(data => data.BaseDamage += value);
                    break;
                case DamageComponentStackEffects.AddStacksBased:
                    StackDamageEffect(data => data.BaseDamage += value*stacks);
                    break;
                case DamageComponentStackEffects.Multiply:
                    StackDamageEffect(data => data.Multiplier += value);
                    break;
                case DamageComponentStackEffects.MultiplyStacksBased:
                    StackDamageEffect(data => data.Multiplier += value * stacks);
                    break;
                case DamageComponentStackEffects.OnXStacksAddElemental:
                    //TODO
                    //Damage[0].ElementData.?
                    break;
                default:
                    Log.Error($"StackEffectType {StackEffects} unsupported for {GetType()}");
                    return;
            }

            void StackDamageEffect(Action<DamageData> action)
            {
                foreach (var data in Damage)
                    action(data);
            }
        }

        public enum DamageComponentStackEffects
        {
            None = 0,
            Effect,
            Add,
            AddStacksBased,
            Multiply,
            MultiplyStacksBased,

            OnXStacksAddElemental,
        }
    }
}
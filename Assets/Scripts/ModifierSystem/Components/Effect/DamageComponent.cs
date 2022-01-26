using System;
using BaseProject;

namespace ModifierSystem
{
    public sealed class DamageComponent : EffectComponent, IStackEffectComponent
    {
        private DamageData[] Damage { get; }
        private DamageComponentStackEffect StackEffectType { get; }

        public DamageComponent(DamageData[] damage, DamageComponentStackEffect stackEffectType = DamageComponentStackEffect.None,
            ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Damage = damage;
            StackEffectType = stackEffectType;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.DealDamage(Damage, acter);
        }

        public void StackEffect(int stacks, double value)
        {
            if (StackEffectType.HasFlag(DamageComponentStackEffect.Add))
                Damage[0].BaseDamage += value;
            if (StackEffectType.HasFlag(DamageComponentStackEffect.AddStacksBased))
                Damage[0].BaseDamage += value * stacks;
            if (StackEffectType.HasFlag(DamageComponentStackEffect.Multiply))
                Damage[0].Multiplier += value;
            if (StackEffectType.HasFlag(DamageComponentStackEffect.SetMultiplierStacksBased))
                Damage[0].Multiplier = stacks;
            if (StackEffectType.HasFlag(DamageComponentStackEffect.MultiplyStacksBased))
                Damage[0].Multiplier += value * stacks;
            if (StackEffectType.HasFlag(DamageComponentStackEffect.OnXStacksAddElemental))
            {
                //TODO
                //Damage[0].ElementData.?
            }

            //Effect at the end, after all the other possible calcs
            if (StackEffectType.HasFlag(DamageComponentStackEffect.Effect))
                SimpleEffect();
        }

        [Flags]
        public enum DamageComponentStackEffect
        {
            None = 0,
            Effect = 1,
            Add = 2, //TODO Add to all damages?
            AddStacksBased = 4,
            Multiply = 8, //TODO Multiply all damages?
            MultiplyStacksBased = 16,
            SetMultiplierStacksBased = 32, //TODO Multiply all damages?

            //DamageComponent specific
            OnXStacksAddElemental = 64,
        }
    }
}
using System;
using BaseProject;

namespace ModifierSystem
{
    public sealed class DamageComponent : EffectComponent, IStackEffectComponent
    {
        private DamageData[] Damage { get; }
        private StackEffectType StackType { get; }

        public DamageComponent(DamageData[] damage, StackEffectType stackType = StackEffectType.None,
            ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Damage = damage;
            StackType = stackType;

            Info = "Damage: " + string.Join<DamageData>(" ", Damage) + "\n";
        }

        protected override void Effect(Unit receiver, Unit acter)
        {
            receiver.DealDamage(Damage, acter);
        }

        public void StackEffect(int stacks, double value)
        {
            //Debug.Log("StackEffect");
            if (StackType.HasFlag(StackEffectType.Add))
                Damage[0].BaseDamage += value;
            if (StackType.HasFlag(StackEffectType.AddStacksBased))
                Damage[0].BaseDamage += value * stacks;
            if (StackType.HasFlag(StackEffectType.Multiply))
                Damage[0].Multiplier += value;
            if (StackType.HasFlag(StackEffectType.SetMultiplierStacksBased))
                Damage[0].Multiplier = stacks;
            if (StackType.HasFlag(StackEffectType.MultiplyStacksBased))
                Damage[0].Multiplier += value * stacks;
            if (StackType.HasFlag(StackEffectType.OnXStacksAddElemental))
            {
                //TODO
                //Damage[0].ElementData.?
            }

            //Effect at the end, after all the other possible calcs
            if (StackType.HasFlag(StackEffectType.Effect))
                SimpleEffect();
        }

        [Flags]
        public enum StackEffectType
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
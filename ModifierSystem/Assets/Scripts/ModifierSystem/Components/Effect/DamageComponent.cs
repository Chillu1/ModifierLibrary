using System;
using System.Linq;
using BaseProject;
using UnityEngine;

namespace ModifierSystem
{
    public sealed class DamageComponent : EffectComponent, IStackEffectComponent
    {
        private Properties EffectProperties { get; }
        private DamageData[] Damage { get; }
        private StackEffectType StackType { get; }

        public DamageComponent(Properties effectProperties, IBaseEffectProperties baseProperties = null) : base(baseProperties)
        {
            EffectProperties = effectProperties;
            
            Info = "Damage: " + string.Join<DamageData>(" ", EffectProperties.Damage) + "\n";
        }

        public DamageComponent(DamageData[] damage, StackEffectType stackType = StackEffectType.None,
            ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Damage = damage;
            StackType = stackType;

            Info = "Damage: " + string.Join<DamageData>(" ", Damage) + "\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            receiver.DealDamage(EffectProperties.Damage, acter);
        }

        public void StackEffect(int stacks, double value)
        {
            //Debug.Log("StackEffect");
            if (EffectProperties.StackType.HasFlag(StackEffectType.Add))
                EffectProperties.Damage[0].BaseDamage += value;
            if (EffectProperties.StackType.HasFlag(StackEffectType.AddStacksBased))
                EffectProperties.Damage[0].BaseDamage += value * stacks;
            if (EffectProperties.StackType.HasFlag(StackEffectType.Multiply))
                EffectProperties.Damage[0].Multiplier += value;
            if (EffectProperties.StackType.HasFlag(StackEffectType.SetMultiplierStacksBased))
                EffectProperties.Damage[0].Multiplier = stacks;
            if (EffectProperties.StackType.HasFlag(StackEffectType.MultiplyStacksBased))
                EffectProperties.Damage[0].Multiplier += value * stacks;
            if (EffectProperties.StackType.HasFlag(StackEffectType.OnXStacksAddElemental))
            {
                //TODO
                //Damage[0].ElementData.?
            }

            //Effect at the end, after all the other possible calcs
            if (EffectProperties.StackType.HasFlag(StackEffectType.Effect))
                SimpleEffect();
        }

        public struct Properties : IEffectProperties
        {
            public DamageData[] Damage { get; }
            public StackEffectType StackType { get; }

            public Properties(DamageData[] damage, StackEffectType stackType = StackEffectType.None)
            {
                Damage = damage;
                StackType = stackType;
            }
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
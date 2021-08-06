using System;

namespace ComboSystem
{
    public abstract class Character
    {
        //TODO Make stats classes instead? Have them sort out their calculations instead of here...
        public float Health { get; protected set; }
        public float MaxHealth { get; protected set; }
        public float MovementSpeed { get; protected set; }
        public ModifierController ModifierController { get; }

        protected Character()
        {
            ModifierController = new ModifierController();
        }

        public virtual void ChangeStat(StatType statType, float value)
        {
            switch (statType)
            {
                case StatType.None:
                    break;
                case StatType.Attack:
                    break;
                case StatType.Defense:
                    break;
                case StatType.MovementSpeed:
                    MovementSpeed += value;
                    break;
                case StatType.Health:
                    float percentageHealth = Health / MaxHealth;
                    MaxHealth += value;
                    RecalculateHealth(percentageHealth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        protected virtual void RecalculateHealth(float percentageHealth)
        {
            Health = MaxHealth * percentageHealth;
        }

        public virtual void ChangeStatPercentage(StatType statType, float percentage)
        {
            switch (statType)
            {
                case StatType.None:
                    break;
                case StatType.Attack:
                    break;
                case StatType.Defense:
                    break;
                case StatType.MovementSpeed:
                    MovementSpeed += MovementSpeed * percentage;
                    break;
                case StatType.Health:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public abstract void RecalculateStats();
        public abstract void DealDamage(DamageData[] damageData);
        public virtual void AddModifier(Modifier modifier)
        {
            ModifierController.AddModifier(modifier);
        }

        public abstract bool IsValidTarget(Modifier modifier);
    }

    public class Player : Character
    {
        public override void RecalculateStats()
        {
        }

        public override void DealDamage(DamageData[] damageData)
        {
        }

        public override bool IsValidTarget(Modifier modifier)
        {
            return true;
        }
    }

    public class Slime : Character
    {
        public override void RecalculateStats()
        {
        }

        public override void DealDamage(DamageData[] damageData)
        {
        }

        public override bool IsValidTarget(Modifier modifier)
        {
            return true;
        }
    }
}
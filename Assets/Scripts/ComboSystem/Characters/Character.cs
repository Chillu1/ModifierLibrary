using System;

namespace ComboSystem
{
    public abstract class Character
    {
        public string Name { get; protected set; }

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

        public virtual void RecalculateStats(){}

        /// <summary>
        ///     Deal damage to this character
        /// </summary>
        public virtual void DealDamage(DamageData[] damageData)
        {
            Health -= damageData[0].Damage;
        }

        public virtual void Attack(Character target)
        {
            ApplyModifiers(target);
            //target.DealDamage();//TODO
        }

        public virtual void ApplyModifiers(Character target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            foreach (var modifierApplier in modifierAppliers)
                modifierApplier.ApplyModifierToTarget(target);
        }
        public virtual void AddModifier(Modifier modifier, bool ownerIsTarget = true)
        {
            if(ownerIsTarget)
                modifier.SetTarget(this);
            ModifierController.TryAddModifier(modifier);
        }

        public virtual bool IsValidTarget(Modifier modifier)
        {
            return true;
        }
    }
}
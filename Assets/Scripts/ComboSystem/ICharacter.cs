using System;

namespace ComboSystem
{
    public interface ICharacter//TODO Change to abstract class, cuz enemies & player things will use same behaviour
    {
        void ChangeStat(StatType statType, float value);
        void ChangeStatPercentage(StatType statType, float percentage);
        void RecalculateStats();
        void DealDamage(DamageData[] damageData);
    }

    public class Player : ICharacter
    {
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public float MovementSpeed { get; private set; }

        public void ChangeStat(StatType statType, float value)
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

        public void ChangeStatPercentage(StatType statType, float percentage)
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

        public void RecalculateStats()
        {
        }

        public void DealDamage(DamageData[] damageData)
        {
        }

        private void RecalculateHealth(float percentageHealth)
        {
            Health = MaxHealth * percentageHealth;
        }
    }
}
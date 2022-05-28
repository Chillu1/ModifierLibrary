using System;
using BaseProject;

namespace ModifierSystem
{
    public class CostComponent : Component, ICostComponent
    {
        private CostType Type { get; }
        private float Amount { get; }

        private Being _owner;

        public CostComponent(CostType type, float amount)
        {
            Type = type;
            Amount = amount;
        }

        public void SetupOwner(Being owner)
        {
            _owner = owner;
        }

        public bool ContainsCost()
        {
            switch (Type)
            {
                case CostType.Mana:
                    return true;
                case CostType.Health:
                    bool success = _owner.Stats.HasStat(StatType.Health, Amount, ComparisonCheck.Greater);
                    //Log.Verbose("We have enough health stat: " + success, "modifiers");
                    return success;
                default:
                    Log.Error($"CostComponent: {Type} is not a valid CostType", "modifiers");
                    break;
            }

            return false;
        }

        public void ApplyCost()
        {
            switch (Type)
            {
                case CostType.Mana:
                    break;
                case CostType.Health:
                    //TODO TEMP DamageData
                    //Log.Verbose("Applying health cost: " + Amount, "modifiers");
                    _owner.DealDamage(new[] { new DamageData(Amount, DamageType.Physical) }, _owner, AttackType.Internal);
                    break;
                default:
                    Log.Error($"CostComponent: {Type} is not a valid CostType", "modifiers");
                    break;
            }
        }
    }
}
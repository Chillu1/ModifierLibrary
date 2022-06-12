using BaseProject;

namespace ModifierSystem
{
    public class ApplierModifierGenerationProperties : IModifierGenerationProperties
    {
        public Modifier AppliedModifier { get; }
        public ApplierType ApplierType { get; }
        public LegalTarget LegalTarget { get; }

        public CostType CostType { get; private set; }
        public float CostAmount { get; private set; }

        public float Cooldown { get; private set; } = -1;
        public bool AutomaticCast { get; private set; }
        public double Chance { get; private set; } = -1;

        public ApplierModifierGenerationProperties(Modifier appliedModifier, ApplierType applierType,
            LegalTarget legalTarget = LegalTarget.Beings)
        {
            AppliedModifier = appliedModifier;
            ApplierType = applierType;
            LegalTarget = legalTarget;
        }

        public void SetCost(CostType costType, float costAmount)
        {
            CostType = costType;
            CostAmount = costAmount;
        }

        public void SetCooldown(float cooldown)
        {
            Cooldown = cooldown;
        }

        public void SetAutomaticCast()
        {
            AutomaticCast = true;
        }

        public void SetChance(double chance)
        {
            Chance = chance;
        }
    }
}
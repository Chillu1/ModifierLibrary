using BaseProject;

namespace ModifierSystem
{
    public class ComboModifierGenerationProperties : ModifierGenerationProperties
    {
        public ComboRecipes Recipes { get; private set; }
        public float Cooldown { get; private set; }

        public IMultiplier Effect { get; private set; }


        public ComboModifierGenerationProperties(string name, LegalTarget legalTarget = LegalTarget.Self) : base(name, legalTarget)
        {
        }

        public void AddRecipes(ComboRecipes comboRecipes)
        {
            Recipes = comboRecipes;
        }

        public void SetCooldown(float cooldown)
        {
            Cooldown = cooldown;
        }

        public void AddDynamicEffect(IMultiplier effect)
        {
            Effect = effect;
        }
    }
}
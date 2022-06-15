using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Used for making generic, often created modifiers
    /// </summary>
    public static class TemplateModifierGenerator
    {
        public static Modifier PermanentElementResistance(string id, ElementType elementType, float value)
        {
            var properties = new ModifierGenerationProperties(id);
            properties.AddEffect(new ElementResistanceComponent(elementType, value));
            properties.SetEffectOnInit();

            return ModifierGenerator.GenerateModifier(properties);
        }
        public static Modifier PermanentDamageResistance(string id, DamageType damageType, float value)
        {
            var properties = new ModifierGenerationProperties(id);
            properties.AddEffect(new DamageResistanceComponent(damageType, value));
            properties.SetEffectOnInit();

            return ModifierGenerator.GenerateModifier(properties);
        }
        public static Modifier PermanentStatusResistance(string id, StatusTag[] statusTags, double[] values)
        {
            var properties = new ModifierGenerationProperties(id);
            properties.AddEffect(new StatusResistanceComponent(statusTags, values));
            properties.SetEffectOnInit();

            return ModifierGenerator.GenerateModifier(properties);
        }

        /// <summary>
        ///     FireBall, FireAttack, etc
        /// </summary>
        public static (Modifier modifier, Modifier applier) PermanentActApplierDamageLinger(string id, ApplierType applierType
            , DamageData[] damage, float manaUsage, float cooldown)
        {
            var properties = new ModifierGenerationProperties(id);
            properties.AddEffect(new DamageComponent(damage), damage);
            properties.SetEffectOnInit();
            properties.SetRemovable();

            var modifier = ModifierGenerator.GenerateModifier(properties);

            var applierProperties = new ApplierModifierGenerationProperties(modifier);
            applierProperties.SetApplier(applierType);
            applierProperties.SetCost(CostType.Mana, manaUsage);
            applierProperties.SetCooldown(cooldown);

            var applierModifier = ModifierGenerator.GenerateApplierModifier(applierProperties);

            return (modifier, applierModifier);
        }
    }
}
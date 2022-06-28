using BaseProject;

namespace ModifierSystem
{
    public struct Cost
    {
        public CostType Type;
        public float Value;

        public Cost(CostType type, float value)
        {
            Type = type;
            Value = value;
        }
    }

    public class CheckProperties//TODO Rename
    {
        public Cost Cost;
        public float Cooldown;
    }

    public class CheckChanceProperties : CheckProperties//TODO Rename
    {
        public double Chance;
    }

    public class DurationProperties
    {
        public float Duration;
    }

    public class OverTimeProperties : DurationProperties
    {
        public float Interval;
    }

    /// <summary>
    ///     Used for making generic, often created modifiers
    /// </summary>
    public static class TemplateModifierGenerator
    {
        public static Modifier PermanentElementResistance(string id, ElementType elementType, float value)
        {
            var properties = new ModifierGenerationProperties(id,
                new ModifierInfo(elementType + " Resistance", elementType + " attacks and spells have reduced effectiveness",
                    elementType + "Resistance"));
            properties.AddEffect(new ElementResistanceComponent(elementType, value));
            properties.SetEffectOnInit();

            return ModifierGenerator.GenerateModifier(properties);
        }
        public static Modifier PermanentDamageResistance(string id, ModifierInfo info, DamageType damageType, float value)
        {
            var properties = new ModifierGenerationProperties(id,
                new ModifierInfo(damageType + " Resistance", damageType + " attacks and spells have reduced effectiveness",
                    damageType + "Resistance"));
            properties.AddEffect(new DamageResistanceComponent(damageType, value));
            properties.SetEffectOnInit();

            return ModifierGenerator.GenerateModifier(properties);
        }
        public static Modifier PermanentStatusResistance(string id, ModifierInfo info, StatusTag[] statusTags, double[] values)
        {
            var properties = new ModifierGenerationProperties(id, info);
            properties.AddEffect(new StatusResistanceComponent(statusTags, values));
            properties.SetEffectOnInit();

            return ModifierGenerator.GenerateModifier(properties);
        }

        /// <summary>
        ///     IceBolt, FireAttack, etc
        /// </summary>
        public static (Modifier modifier, Modifier applier) PermanentActApplierDamageLinger(string id,
            ModifierInfo effectInfo, ModifierInfo applierInfo,
            ApplierType applierType, DamageData[] damage, CheckProperties check)
        {
            var properties = new ModifierGenerationProperties(id, effectInfo);
            properties.AddEffect(new DamageComponent(damage), damage);
            properties.SetEffectOnInit();
            properties.SetRemovable();

            var modifier = ModifierGenerator.GenerateModifier(properties);

            var applierProperties = new ApplierModifierGenerationProperties(modifier, applierInfo);
            applierProperties.SetApplier(applierType);
            if(check.Cost.Type != CostType.None)
                applierProperties.SetCost(check.Cost.Type, check.Cost.Value);
            if(check.Cooldown != 0)
                applierProperties.SetCooldown(check.Cooldown);

            var applierModifier = ModifierGenerator.GenerateApplierModifier(applierProperties);

            return (modifier, applierModifier);
        }

        public static (Modifier modifier, Modifier applier) PermanentActApplierDamageDoT(string id,
            ModifierInfo effectInfo, ModifierInfo applierInfo,
            ApplierType applierType, DamageData[] damage, CheckProperties check, OverTimeProperties overTime)
        {
            var properties = new ModifierGenerationProperties(id, effectInfo);
            properties.AddEffect(new DamageComponent(damage), damage);
            properties.SetEffectOnInit();
            properties.SetEffectOnTime(overTime.Interval, true);
            properties.SetRemovable(overTime.Duration);

            var modifier = ModifierGenerator.GenerateModifier(properties);

            var applierProperties = new ApplierModifierGenerationProperties(modifier, applierInfo);
            applierProperties.SetApplier(applierType);
            if(check.Cost.Type != CostType.None)
                applierProperties.SetCost(check.Cost.Type, check.Cost.Value);
            if(check.Cooldown != 0)
                applierProperties.SetCooldown(check.Cooldown);

            var applierModifier = ModifierGenerator.GenerateApplierModifier(applierProperties);

            return (modifier, applierModifier);
        }

        public static (Modifier modifier, Modifier applier) PermanentActApplierDamageInitDoT(string id,
            ModifierInfo effectInfo, ModifierInfo applierInfo,
            ApplierType applierType, DamageData[] initDamage, DamageData[] dotDamage, CheckProperties check, OverTimeProperties overTime)
        {
            var properties = new ModifierGenerationProperties(id, effectInfo);
            properties.AddEffect(new DamageComponent(initDamage), initDamage);
            properties.SetEffectOnInit();
            properties.AddEffect(new DamageComponent(dotDamage), dotDamage);
            properties.SetEffectOnTime(overTime.Interval, true);
            properties.SetRemovable(overTime.Duration);

            var modifier = ModifierGenerator.GenerateModifier(properties);

            var applierProperties = new ApplierModifierGenerationProperties(modifier, applierInfo);
            applierProperties.SetApplier(applierType);
            if(check.Cost.Type != CostType.None)
                applierProperties.SetCost(check.Cost.Type, check.Cost.Value);
            if(check.Cooldown != 0)
                applierProperties.SetCooldown(check.Cooldown);

            var applierModifier = ModifierGenerator.GenerateApplierModifier(applierProperties);

            return (modifier, applierModifier);
        }
    }
}
using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ComboModifierPrototypes : ModifierPrototypes<ComboModifier>, IComboModifierPrototypes
    {
        //PermanentMods might be able to be stripped/removed later, does it matter?
        private const float PermanentComboModifierCooldown = 60;
        private static IComboModifierPrototypes _instance;

        public ComboModifierPrototypes(bool includeTest = false)
        {
            _instance = this;
            if (includeTest && false)//TODOPRIO
                SetupTestComboModifiers();
        }

        public static void SetUnitTestInstance(IComboModifierPrototypes instance)
        {
            _instance = instance;
        }

        public ComboModifier AddModifier(ComboModifierGenerationProperties properties)
        {
            var comboModifier = ModifierGenerator.GenerateComboModifier(properties);
            AddModifier(comboModifier);
            return comboModifier;
        }

        public static HashSet<ComboModifier> CheckForComboRecipes(HashSet<string> modifierIds,
            Dictionary<string, float> comboModifierCooldowns, ElementController elementController, Stats stats)
        {
            HashSet<ComboModifier> modifierToAdd = new HashSet<ComboModifier>();
            if (_instance == null)
            {
                Log.Warning("ComboModifier instance is null, this is bad, unless this is a unit test", "modifiers");
                return modifierToAdd;
            }

            foreach (var comboModifier in _instance.Values)
            {
                if (comboModifierCooldowns.ContainsKey(comboModifier.Id)) //Skip if there's a cooldown on the comboModifier
                    continue;
                if (comboModifier.CheckRecipes(modifierIds, elementController, stats))
                    modifierToAdd.Add(_instance.Get(comboModifier.Id));
            }

            return modifierToAdd;
        }

        private void SetupTestComboModifiers()
        {
            {
                //Aspect of the cat
                var properties = new ComboModifierGenerationProperties("ComboAspectOfTheCatTest", null);
                properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { "MovementSpeedOfCatTest", "AttackSpeedOfCatTest" })));
                properties.SetCooldown(1);

                properties.AddEffect(new StatComponent(StatType.MovementSpeed, 10));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                AddModifier(properties);
            }
            {
                //Poison & bleed = infection
                var damageData = new[]
                    { new DamageData(10, DamageType.Physical, new ElementData(ElementType.Bleed | ElementType.Poison, 30, 50)) };
                var properties = new ComboModifierGenerationProperties("ComboInfectionTest", null);
                properties.AddDynamicEffect(damageData[0]);
                properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[]
                    { new ElementalRecipe(ElementType.Poison, 5), new ElementalRecipe(ElementType.Bleed, 5) })));
                properties.SetCooldown(1);

                properties.AddEffect(
                    new DamageComponent(
                        damageData) /*, damageData*/); //TODO What to do with infection & such combined status res enums?
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                AddModifier(properties);
            }
            {
                //10k health = giant (physical res)
                var properties = new ComboModifierGenerationProperties("ComboGiantTest", null);
                properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { new Stat(StatType.Health, 10000) })));
                properties.SetCooldown(PermanentComboModifierCooldown);

                properties.AddEffect(new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }));
                properties.SetEffectOnInit();

                AddModifier(properties);
            }
            {
                //10k health = temporary giant (physical res)
                var properties = new ComboModifierGenerationProperties("ComboTimedGiantTest", null);
                properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { new Stat(StatType.Health, 10000) })));
                properties.SetCooldown(PermanentComboModifierCooldown);
                properties.SetRemovable(10);

                properties.AddEffect(new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }));
                properties.SetEffectOnInit();

                AddModifier(properties);
            }
        }
    }
}
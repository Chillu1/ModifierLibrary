using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ComboModifierPrototypes : IComboModifierPrototypes
    {
        private static IComboModifierPrototypes _instance;
        public ModifierPrototypesBase<ComboModifier> ModifierPrototypes { get; }

        public ComboModifierPrototypes()
        {
            _instance = this;
            ModifierPrototypes = new ModifierPrototypesBase<ComboModifier>();
            SetupModifierPrototypes();
        }

        public static void SetUnitTestInstance(IComboModifierPrototypes instance)
        {
            _instance = instance;
        }

        private void SetupModifierPrototypes()
        {
            //Scope brackets so it's impossible to use a wrong component/modifier
            {
                //Aspect of the cat
                var aspectOfTheCatModifier = new Modifier("AspectOfTheCat");
                var aspectOfTheCatTarget = new TargetComponent(LegalTarget.Self);
                var aspectOfTheCatEffect = new StatComponent(12, aspectOfTheCatTarget);
                var aspectOfTheCatApply = new ApplyComponent(aspectOfTheCatEffect, aspectOfTheCatTarget);
                aspectOfTheCatModifier.AddComponent(new InitComponent(aspectOfTheCatApply));
                aspectOfTheCatModifier.AddComponent(aspectOfTheCatTarget);
                aspectOfTheCatModifier.AddComponent(new TimeComponent(new RemoveComponent(aspectOfTheCatModifier), 10));
                var aspectOfTheCatComboModifier = new ComboModifier(aspectOfTheCatModifier,
                    new ComboRecipes(new ComboRecipe(new[] { "Dexterity", "Speed" })),
                    1);
                ModifierPrototypes.AddModifier(aspectOfTheCatComboModifier);
            }
            {
                //Poison & bleed = infection
                var infectionModifier = new Modifier("Infection");
                var infectionTarget = new TargetComponent(LegalTarget.Self);
                var infectionEffect = new DamageComponent(new[]
                        { new DamageData(20, DamageType.Physical, new ElementData(ElementalType.Bleed | ElementalType.Poison, 30, 50)) },
                        infectionTarget);
                var infectionApply = new ApplyComponent(infectionEffect, infectionTarget);
                infectionModifier.AddComponent(new InitComponent(infectionApply));
                infectionModifier.AddComponent(infectionTarget);
                infectionModifier.AddComponent(new TimeComponent(infectionEffect, 2, true));
                infectionModifier.AddComponent(new TimeComponent(new RemoveComponent(infectionModifier), 10));
                var infectionComboModifier = new ComboModifier(infectionModifier,
                    new ComboRecipes(new ComboRecipe(
                        new[]{new ElementalRecipe(ElementalType.Poison, 5), new ElementalRecipe(ElementalType.Bleed, 5)})),
                    1);
                ModifierPrototypes.AddModifier(infectionComboModifier);
            }
        }

        [CanBeNull]
        public ComboModifier GetItem(string key)
        {
            return ModifierPrototypes.GetItem(key);
        }

        public static HashSet<ComboModifier> CheckForComboRecipes(HashSet<string> modifierIds, ElementController elementController)
        {
            HashSet<ComboModifier> modifierToAdd = new HashSet<ComboModifier>();
            if (_instance == null)
            {
                Log.Warning("ComboModifier instance is null, this is bad, unless this is a unit test");
                return modifierToAdd;
            }

            foreach (var comboModifier in _instance.ModifierPrototypes.Values)
            {
                if (comboModifier.CheckRecipes(modifierIds, elementController))
                    modifierToAdd.Add(_instance.GetItem(comboModifier.Id));
            }

            return modifierToAdd;
        }
    }
}
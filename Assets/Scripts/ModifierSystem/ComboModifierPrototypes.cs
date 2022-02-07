using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ComboModifierPrototypes : IComboModifierPrototypes
    {
        private static IComboModifierPrototypes _instance;
        public ModifierPrototypesBase ModifierPrototypes { get; }

        public ComboModifierPrototypes()
        {
            _instance = this;
            ModifierPrototypes = new ModifierPrototypesBase();
            SetupModifierPrototypes();
        }

        public static void SetUnitTestInstance(IComboModifierPrototypes instance)
        {
            _instance = instance;
        }

        private void SetupModifierPrototypes()
        {
            //Scope brackets so it's impossible to use a wrong component/modifier
            /*{
                //Aspect of the cat
                var modifier = new Modifier("AspectOfTheCat");
                var target = new TargetComponent(LegalTarget.Self);
                var effect = new StatComponent(new []{new Stat(StatType.MovementSpeed){baseValue = 5}}, target);
                modifier.AddComponent(target);
                modifier.AddComponent(new InitComponent(effect));
                modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10));
                var comboModifier = new ComboModifier(modifier,
                    new ComboRecipes(new ComboRecipe(new[] { "Dexterity", "Speed" })),
                    1);
                ModifierPrototypes.AddModifier(comboModifier);
            }
            {
                //Poison & bleed = infection
                var modifier = new Modifier("Infection");
                var target = new TargetComponent(LegalTarget.Self);
                var effect = new DamageComponent(new[]
                        { new DamageData(20, DamageType.Physical, new ElementData(ElementalType.Bleed | ElementalType.Poison, 30, 50)) },
                        target);
                modifier.AddComponent(target);
                modifier.AddComponent(new InitComponent(effect));
                modifier.AddComponent(new TimeComponent(effect, 2, true));
                modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10));
                var comboModifier = new ComboModifier(modifier,
                    new ComboRecipes(new ComboRecipe(
                        new[]{new ElementalRecipe(ElementalType.Poison, 5), new ElementalRecipe(ElementalType.Bleed, 5)})),
                    1);
                ModifierPrototypes.AddModifier(comboModifier);
            }*/
        }

        [CanBeNull]
        public IComboModifier GetItem(string key)
        {
            return (IComboModifier)ModifierPrototypes.Get(key);
        }

        public static HashSet<IComboModifier> CheckForComboRecipes(HashSet<string> modifierIds,
            Dictionary<string, float> comboModifierCooldowns, ElementController elementController, Stats stats)
        {
            HashSet<IComboModifier> modifierToAdd = new HashSet<IComboModifier>();
            if (_instance == null)
            {
                Log.Warning("ComboModifier instance is null, this is bad, unless this is a unit test");
                return modifierToAdd;
            }

            foreach (var comboModifier in _instance.ModifierPrototypes.Values)
            {
                if (comboModifierCooldowns.ContainsKey(comboModifier.Id)) //Skip if there's a cooldown on the comboModifier
                    continue;
                if (((IComboModifier)comboModifier).CheckRecipes(modifierIds, elementController, stats))
                    modifierToAdd.Add(_instance.GetItem(comboModifier.Id));
            }

            return modifierToAdd;
        }
    }
}
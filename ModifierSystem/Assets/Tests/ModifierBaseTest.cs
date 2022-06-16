using System;
using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public abstract class ModifierBaseTest
    {
        protected Being character;
        protected Being ally;
        protected Being enemy;
        protected Being[] enemyDummies;

        protected double initialHealthCharacter, initialHealthAlly, initialHealthEnemy;
        protected double initialDamageCharacter, initialDamageAlly, initialDamageEnemy;
        protected double initialManaCharacter;

        protected const double Delta = 0.01d;

        protected const float
            PermanentComboModifierCooldown = 60; //PermanentMods might be able to be stripped/removed later, does it matter?

        protected ModifierPrototypesTest modifierPrototypes;

        protected ComboModifierPrototypesTest comboModifierPrototypesTest;
        //protected ComboModifierPrototypesTest comboModifierPrototypes;

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            modifierPrototypes = new ModifierPrototypesTest();
            comboModifierPrototypesTest = new ComboModifierPrototypesTest();
            ComboModifierPrototypes.SetUnitTestInstance(comboModifierPrototypesTest);
            //comboModifierPrototypes = new ComboModifierPrototypesTest();
            //comboModifierPrototypes.AddTestModifiers();
        }

        [SetUp]
        public void Init()
        {
            character = new Being(new BeingProperties
            {
                Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
            });
            ally = new Being(new BeingProperties
            {
                Id = "ally", Health = 25, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                Mana = 50, ManaRegen = 1, UnitType = UnitType.Ally
            });
            enemy = new Being(new BeingProperties
            {
                Id = "enemy", Health = 30, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 2,
                Mana = 20, ManaRegen = 1, UnitType = UnitType.Enemy
            });
            initialHealthCharacter = character.Stats.Health.CurrentHealth;
            initialHealthAlly = ally.Stats.Health.CurrentHealth;
            initialHealthEnemy = enemy.Stats.Health.CurrentHealth;
            initialDamageCharacter = character.Stats.Damage.DamageSum();
            initialDamageAlly = ally.Stats.Damage.DamageSum();
            initialDamageEnemy = enemy.Stats.Damage.DamageSum();
            initialManaCharacter = character.Stats.Mana.CurrentMana;

            enemyDummies = new Being[5];
            for (int i = 0; i < 5; i++)
            {
                enemyDummies[0] = new Being(new BeingProperties()
                {
                    Id = "enemy", Health = 1, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 1,
                    Mana = 50, ManaRegen = 100, UnitType = UnitType.Enemy
                });
            }
        }

        [TearDown]
        public void CleanUp()
        {
            character = null;
            ally = null;
            enemy = null;
        }

        public class ModifierPrototypesTest : ModifierPrototypes<Modifier>
        {
            public ModifierPrototypesTest() : base(true)
            {
            }

            [CanBeNull]
            public new Modifier Get(string key)
            {
                if (key.Contains("Applier"))
                {
                    string subKey = key.Substring(0, key.IndexOf("Applier", StringComparison.Ordinal));
                    if (!subKey.EndsWith("Test"))
                    {
                        Log.Error("Invalid modifier Id, it has to end with 'Test' for unit tests");
                        return null;
                    }
                }
                else if (!key.EndsWith("Test"))
                {
                    Log.Error("Invalid modifier Id, it has to include 'Test' for unit tests");
                    return null;
                }

                return base.Get(key);
            }
        }

        public class ComboModifierPrototypesTest : ModifierPrototypes<ComboModifier>, IComboModifierPrototypes
        {
            private static ComboModifierPrototypesTest _instance;

            public ComboModifierPrototypesTest()
            {
                _instance = this;
                SetupModifierPrototypes();
            }

            private void SetupModifierPrototypes()
            {
                //Scope brackets so it's impossible to use a wrong component/modifier
                {
                    //Aspect of the cat
                    var properties = new ComboModifierGenerationProperties("ComboAspectOfTheCatTest", null);
                    properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { "MovementSpeedOfCatTest", "AttackSpeedOfCatTest" })));
                    properties.SetCooldown(1);

                    properties.AddEffect(new StatComponent(new[] { new Stat(StatType.MovementSpeed, 10) }));
                    properties.SetEffectOnInit();
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateComboModifier(properties);
                    AddModifier(modifier);
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

                    var modifier = ModifierGenerator.GenerateComboModifier(properties);
                    AddModifier(modifier);
                }
                {
                    //10k health = giant (physical res)
                    var properties = new ComboModifierGenerationProperties("ComboGiantTest", null);
                    properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { new Stat(StatType.Health, 10000) })));
                    properties.SetCooldown(PermanentComboModifierCooldown);

                    properties.AddEffect(new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }));
                    properties.SetEffectOnInit();

                    var modifier = ModifierGenerator.GenerateComboModifier(properties);
                    AddModifier(modifier);
                }
                {
                    //10k health = temporary giant (physical res)
                    var properties = new ComboModifierGenerationProperties("ComboTimedGiantTest", null);
                    properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { new Stat(StatType.Health, 10000) })));
                    properties.SetCooldown(PermanentComboModifierCooldown);
                    properties.SetRemovable(10);

                    properties.AddEffect(new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }));
                    properties.SetEffectOnInit();

                    var modifier = ModifierGenerator.GenerateComboModifier(properties);
                    AddModifier(modifier);
                }
            }


            [CanBeNull]
            public new ComboModifier Get(string key)
            {
                return base.Get(key);
            }

            public static HashSet<ComboModifier> CheckForComboRecipes(HashSet<string> modifierIds, ElementController elementController,
                Stats stats)
            {
                HashSet<ComboModifier> modifierToAdd = new HashSet<ComboModifier>();
                if (_instance == null)
                {
                    Log.Warning("ComboModifier instance is null, this is bad, unless this is a unit test");
                    return modifierToAdd;
                }

                foreach (var comboModifier in _instance.Values)
                {
                    if (comboModifier.CheckRecipes(modifierIds, elementController, stats))
                        modifierToAdd.Add(comboModifier);
                }

                return modifierToAdd;
            }
        }
    }
}
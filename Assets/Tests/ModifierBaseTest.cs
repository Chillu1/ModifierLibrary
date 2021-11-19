using BaseProject;
using NUnit.Framework;

namespace ComboSystem.Tests
{
    public abstract class ModifierBaseTest
    {
        protected ComboBeing character;
        protected ComboBeing enemy;

        protected ModifierPrototypesTest modifierPrototypes;
        protected ComboModifierPrototypesTest comboModifierPrototypes;

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            modifierPrototypes = new ModifierPrototypesTest();
            modifierPrototypes.AddTestModifiers();
            comboModifierPrototypes = new ComboModifierPrototypesTest();
            comboModifierPrototypes.AddTestModifiers();
        }

        [SetUp]
        public void Init()
        {
            character = new ComboBeing(new ComboBeingProperties() { Id = "player", Health = 50, Damage = 5, MovementSpeed = 3 });
            enemy = new ComboBeing(new ComboBeingProperties() { Id = "enemy", Health = 30, Damage = 3, MovementSpeed = 2 });
        }

        [TearDown]
        public void CleanUp()
        {
            character = null;
            enemy = null;
        }

        protected sealed class ModifierPrototypesTest : ModifierPrototypes
        {
            public void AddTestModifiers()
            {
                var physicalAttackDoTData = new DamageOverTimeData(new[]{new DamageData(2, DamageType.Physical)}, 1f, 5f);
                var physicalAttackDoT = new DamageOverTimeModifier("PhysicalDoTAttack", physicalAttackDoTData, ModifierProperties.Refreshable);
                SetupModifierApplier(physicalAttackDoT);

                var fireAttackData = new[]{new DamageData(3, DamageType.Fire)};
                var fireAttack = new DamageAttackModifier("TestFireDamage", fireAttackData);
                SetupModifierApplier(fireAttack);
                var coldAttackData = new[] { new DamageData(3, DamageType.Cold) };
                var coldAttack = new DamageAttackModifier("TestColdDamage", coldAttackData);
                SetupModifierApplier(coldAttack);

                var catMovementSpeedBuffData = new StatChangeModifierData(StatType.MovementSpeed, 3f);
                var catMovementSpeedBuff = new StatChangeModifier("TestMovementSpeedOfCat", catMovementSpeedBuffData);
                SetupModifier(catMovementSpeedBuff);
                var catAttackSpeedBuffData = new StatChangeModifierData(StatType.AttackSpeed, 3f);
                var catAttackSpeedBuff = new StatChangeModifier("TestAttackSpeedOfCat", catAttackSpeedBuffData);
                SetupModifier(catAttackSpeedBuff);
            }
        }

        protected sealed class ComboModifierPrototypesTest : ComboModifierPrototypes
        {
            public void AddTestModifiers()
            {
                var aspectOfTheCatData = new StatChangeModifierData(StatType.Attack, 10);
                var aspectOfTheCatRecipe = new ComboRecipe(new ComboRecipeProperties()
                    { Ids = new[] { "TestMovementSpeedOfCat", "TestAttackSpeedOfCat" } });
                StatChangeComboModifier aspectOfTheCat =
                    new StatChangeComboModifier("TestAspectOfTheCat", aspectOfTheCatData, aspectOfTheCatRecipe);
                SetupModifier(aspectOfTheCat);

                DamageData[] explosionData = new DamageData[]{new DamageData(10, DamageType.Explosion)};
                var explosionRecipe = new ComboRecipe(new ComboRecipeProperties()
                    { DamageTypes = new [] { DamageType.Fire | DamageType.Cold}});
                DamageAttackComboModifier explosion =
                    new DamageAttackComboModifier("TestExplosion", explosionData, explosionRecipe);
                SetupModifier(explosion);
            }
        }
    }
}
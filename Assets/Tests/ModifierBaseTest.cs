using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public abstract class ModifierBaseTest
    {
        protected Being character;
        protected Being enemy;

        protected ModifierPrototypesTest modifierPrototypes;
        //protected ComboModifierPrototypesTest comboModifierPrototypes;

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            modifierPrototypes = new ModifierPrototypesTest();
            //comboModifierPrototypes = new ComboModifierPrototypesTest();
            //comboModifierPrototypes.AddTestModifiers();
        }

        [SetUp]
        public void Init()
        {
            character = new Being(new BeingProperties() { Id = "player", Health = 50, Damage = 1, MovementSpeed = 3 });
            enemy = new Being(new BeingProperties() { Id = "enemy", Health = 30, Damage = 1, MovementSpeed = 2 });
        }

        [TearDown]
        public void CleanUp()
        {
            character = null;
            enemy = null;
        }

        protected sealed class ModifierPrototypesTest : ModifierPrototypesBase<Modifier>
        {
            public ModifierPrototypesTest()
            {
                SetupModifierPrototypes();
            }

            protected override void SetupModifierPrototypes()
            {
                //StackableSpiderPoison, removed after 10 seconds
                //-Each stack increases DoT damage by 2
                var spiderPoisonModifier = new Modifier("SpiderPoison");
                var spiderPoisonTarget = new TargetComponent(UnitType.Self);
                var damageData = new[] { new DamageData(5, DamageType.Poison) };
                var spiderPoisonEffect = new DamageComponent(damageData, spiderPoisonTarget);
                var spiderPoisonStack = new StackComponent(() => damageData[0].BaseDamage += 2, 10);
                var spiderPoisonApply = new ApplyComponent(spiderPoisonEffect, spiderPoisonTarget);
                spiderPoisonModifier.AddComponent(new InitComponent(spiderPoisonApply));//Apply first stack/damage on init
                spiderPoisonModifier.AddComponent(spiderPoisonTarget);
                spiderPoisonModifier.AddComponent(new TimeComponent(spiderPoisonEffect, 2, true));//Every 2 seconds, deal 5 damage
                spiderPoisonModifier.AddComponent(new TimeComponent(new RemoveComponent(spiderPoisonModifier), 10));//Remove after 10 secs
                spiderPoisonModifier.AddComponent(spiderPoisonStack);
                AddModifier(spiderPoisonModifier);

                SetupModifierApplier(spiderPoisonModifier, UnitType.DefaultOffensive);
                
                /*var physicalAttackDoTData = new DamageOverTimeData(new[]{new DamageData(2, DamageType.Physical)}, 1f, 5f);
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
                SetupModifier(catAttackSpeedBuff);*/
            }
        }

        /*protected sealed class ComboModifierPrototypesTest : ComboModifierPrototypes
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
        }*/
    }
}
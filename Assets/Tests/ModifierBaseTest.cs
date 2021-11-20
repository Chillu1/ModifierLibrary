using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public abstract class ModifierBaseTest
    {
        protected Being character;
        protected Being ally;
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
            character = new Being(new BeingProperties() { Id = "player", Health = 50, Damage = 1, MovementSpeed = 3, UnitType = UnitType.Ally});
            ally = new Being(new BeingProperties() { Id = "ally", Health = 25, Damage = 1, MovementSpeed = 3, UnitType = UnitType.Ally});
            enemy = new Being(new BeingProperties() { Id = "enemy", Health = 30, Damage = 1, MovementSpeed = 2, UnitType = UnitType.Enemy});
        }

        [TearDown]
        public void CleanUp()
        {
            character = null;
            ally = null;
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
                var iceBoltModifier = new Modifier("IceBolt");
                var iceBoltTarget = new TargetComponent(LegalTarget.Self);
                var iceBoltEffect = new DamageComponent(new []{new DamageData(10, DamageType.Physical)}, iceBoltTarget);
                var iceBoltApply = new ApplyComponent(iceBoltEffect, iceBoltTarget);
                iceBoltModifier.AddComponent(new InitComponent(iceBoltApply));
                iceBoltModifier.AddComponent(iceBoltTarget);
                AddModifier(iceBoltModifier);
                SetupModifierApplier(iceBoltModifier, LegalTarget.DefaultOffensive);

                //StackableSpiderPoison, removed after 10 seconds
                //-Each stack increases DoT damage by 2
                var spiderPoisonModifier = new Modifier("SpiderPoison");
                var spiderPoisonTarget = new TargetComponent(LegalTarget.Self);
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
                SetupModifierApplier(spiderPoisonModifier, LegalTarget.DefaultOffensive);

                var selfHealModifier = new Modifier("PassiveSelfHeal");
                var selfHealTarget = new TargetComponent(LegalTarget.Self);
                var selfHealEffect = new HealComponent(10, selfHealTarget);
                var selfHealApply = new ApplyComponent(selfHealEffect, selfHealTarget);
                selfHealModifier.AddComponent(new TimeComponent(selfHealEffect, 1, true));//Every 2 seconds, deal 5 damage
                selfHealModifier.AddComponent(new InitComponent(selfHealApply));
                selfHealModifier.AddComponent(selfHealTarget);
                AddModifier(selfHealModifier);

                var allyHealModifier = new Modifier("AllyHeal");
                var allyHealTarget = new TargetComponent(LegalTarget.Self);
                var allyHealEffect = new HealComponent(10, allyHealTarget);
                var allyHealApply = new ApplyComponent(allyHealEffect, allyHealTarget);
                allyHealModifier.AddComponent(new InitComponent(allyHealApply));
                allyHealModifier.AddComponent(allyHealTarget);
                allyHealModifier.AddComponent(new TimeComponent(new RemoveComponent(allyHealModifier)));
                AddModifier(allyHealModifier);
                //Forever buff (applier), not refreshable or stackable (for now)
                SetupModifierApplier(allyHealModifier, LegalTarget.DefaultFriendly);
                
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
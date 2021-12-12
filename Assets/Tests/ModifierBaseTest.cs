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

        protected double initialHealthCharacters, initialHealthAlly, initialHealthEnemy;

        protected const double Delta = 0.01d;

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
                Id = "player", Health = 50, DamageData = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                UnitType = UnitType.Ally
            });
            ally = new Being(new BeingProperties
            {
                Id = "ally", Health = 25, DamageData = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                UnitType = UnitType.Ally
            });
            enemy = new Being(new BeingProperties
            {
                Id = "enemy", Health = 30, DamageData = new DamageData(1, DamageType.Physical, null), MovementSpeed = 2,
                UnitType = UnitType.Enemy
            });
            initialHealthCharacters = character.CurrentHealth;
            initialHealthAlly = ally.CurrentHealth;
            initialHealthEnemy = enemy.CurrentHealth;
        }

        [TearDown]
        public void CleanUp()
        {
            character = null;
            ally = null;
            enemy = null;
        }

        public class ModifierPrototypesTest
        {
            private readonly ModifierPrototypesBase<Modifier> _modifierPrototypes;

            public ModifierPrototypesTest()
            {
                _modifierPrototypes = new ModifierPrototypesBase<Modifier>();
                SetupModifierPrototypes();
            }

            private void SetupModifierPrototypes()
            {
                //Super simple example modifier:
                //Single 100 damage ability on target, (lingers 0.5s, but no actual duration)
                //On Init, Apply It (deal damage), remove 0.5 seconds after from manager

                {
                    //IceBoltDebuff
                    var iceBoltModifier = new Modifier("IceBolt");
                    var iceBoltTarget = new TargetComponent();
                    var iceBoltEffect =
                        new DamageComponent(new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementalType.Cold, 20, 10)) },
                            iceBoltTarget);
                    var iceBoltApply = new ApplyComponent(iceBoltEffect, iceBoltTarget);
                    iceBoltModifier.AddComponent(new InitComponent(iceBoltApply));
                    iceBoltModifier.AddComponent(iceBoltTarget);
                    iceBoltModifier.AddComponent(new TimeComponent(new RemoveComponent(iceBoltModifier)));
                    _modifierPrototypes.AddModifier(iceBoltModifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    //Apply on attack
                    _modifierPrototypes.SetupModifierApplier(iceBoltModifier);
                }

                {
                    //StackableSpiderPoison, removed after 10 seconds
                    //-Each stack increases DoT damage by 2
                    //-Each stack increases current duration by 2, to max 10 stacks
                    var spiderPoisonModifier = new Modifier("SpiderPoison");
                    var spiderPoisonTarget = new TargetComponent();
                    var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var spiderPoisonEffect = new DamageComponent(damageData, spiderPoisonTarget);
                    var spiderPoisonStack = new StackComponent((data, value) => data[0].BaseDamage += value, 10);
                    var spiderPoisonApply = new ApplyComponent(spiderPoisonEffect, spiderPoisonTarget);
                    spiderPoisonModifier.AddComponent(new InitComponent(spiderPoisonApply)); //Apply first stack/damage on init
                    spiderPoisonModifier.AddComponent(spiderPoisonTarget);
                    spiderPoisonModifier.AddComponent(new TimeComponent(spiderPoisonEffect, 2, true)); //Every 2 seconds, deal 5 damage
                    spiderPoisonModifier.AddComponent(new TimeComponent(new RemoveComponent(spiderPoisonModifier),
                        10)); //Remove after 10 secs
                    spiderPoisonModifier.AddComponent(spiderPoisonStack);
                    _modifierPrototypes.AddModifier(spiderPoisonModifier);
                    _modifierPrototypes.SetupModifierApplier(spiderPoisonModifier);
                }

                {
                    //RefreshableCobraVenom, removed after 10 seconds
                    //-Refresh = refreshes duration (timer)
                    //TODO -Refresh = refreshes duration (timer) & increased duration by flat 10%
                    //TODO -Refresh = refreshes duration (timer) & intensify effect?
                    var cobraVenomModifier = new Modifier("CobraVenom");
                    var cobraVenomTarget = new TargetComponent();
                    var cobraVenomDamageData = new[]
                        { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 5, 20)) };
                    var cobraVenomEffect = new DamageComponent(cobraVenomDamageData, cobraVenomTarget);
                    var cobraVenomRemoveTime = new TimeComponent(new RemoveComponent(cobraVenomModifier), 10);
                    var cobraVenomRefresh = new RefreshComponent(cobraVenomRemoveTime);
                    var cobraVenomApply = new ApplyComponent(cobraVenomEffect, cobraVenomTarget);
                    cobraVenomModifier.AddComponent(new InitComponent(cobraVenomApply)); //Apply first stack/damage on init
                    cobraVenomModifier.AddComponent(cobraVenomTarget);
                    cobraVenomModifier.AddComponent(new TimeComponent(cobraVenomEffect, 2, true)); //Every 2 seconds, deal 5 damage
                    cobraVenomModifier.AddComponent(cobraVenomRemoveTime); //Remove after 10 secs
                    cobraVenomModifier.AddComponent(cobraVenomRefresh);
                    _modifierPrototypes.AddModifier(cobraVenomModifier);
                    _modifierPrototypes.SetupModifierApplier(cobraVenomModifier);
                }

                {
                    //PassiveSelfHeal
                    var selfHealModifier = new Modifier("PassiveSelfHeal");
                    var selfHealTarget = new TargetComponent();
                    var selfHealEffect = new HealComponent(10, selfHealTarget);
                    var selfHealApply = new ApplyComponent(selfHealEffect, selfHealTarget);
                    selfHealModifier.AddComponent(new InitComponent(selfHealApply));
                    selfHealModifier.AddComponent(selfHealTarget);
                    _modifierPrototypes.AddModifier(selfHealModifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    //SetupModifierApplier(selfHealModifier, LegalTarget.Self);
                }

                {
                    var allyHealModifier = new Modifier("AllyHeal");
                    var allyHealTarget = new TargetComponent();
                    var allyHealEffect = new HealComponent(10, allyHealTarget);
                    var allyHealApply = new ApplyComponent(allyHealEffect, allyHealTarget);
                    allyHealModifier.AddComponent(new InitComponent(allyHealApply));
                    allyHealModifier.AddComponent(allyHealTarget);
                    allyHealModifier.AddComponent(new TimeComponent(new RemoveComponent(allyHealModifier)));
                    _modifierPrototypes.AddModifier(allyHealModifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    _modifierPrototypes.SetupModifierApplier(allyHealModifier, LegalTarget.DefaultFriendly);
                }

                {
                    //BasicPoison, removed after 10 seconds
                    var poisonModifier = new Modifier("Poison");
                    var poisonTarget = new TargetComponent();
                    var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 20, 20)) };
                    var poisonEffect = new DamageComponent(damageData, poisonTarget);
                    var poisonApply = new ApplyComponent(poisonEffect, poisonTarget);
                    poisonModifier.AddComponent(new InitComponent(poisonApply)); //Apply damage on init
                    poisonModifier.AddComponent(poisonTarget);
                    poisonModifier.AddComponent(new TimeComponent(poisonEffect, 2, true)); //Every 2 seconds, deal 5 damage
                    poisonModifier.AddComponent(new TimeComponent(new RemoveComponent(poisonModifier), 10)); //Remove after 10 secs
                    _modifierPrototypes.AddModifier(poisonModifier);
                    _modifierPrototypes.SetupModifierApplier(poisonModifier);
                }
                
                {
                    //BasicBleed, removed after 10 seconds
                    var bleedModifier = new Modifier("Bleed");
                    var bleedTarget = new TargetComponent();
                    var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Bleed, 20, 20)) };
                    var bleedEffect = new DamageComponent(damageData, bleedTarget);
                    var bleedApply = new ApplyComponent(bleedEffect, bleedTarget);
                    bleedModifier.AddComponent(new InitComponent(bleedApply)); //Apply damage on init
                    bleedModifier.AddComponent(bleedTarget);
                    bleedModifier.AddComponent(new TimeComponent(bleedEffect, 2, true)); //Every 2 seconds, deal 5 damage
                    bleedModifier.AddComponent(new TimeComponent(new RemoveComponent(bleedModifier), 10)); //Remove after 10 secs
                    _modifierPrototypes.AddModifier(bleedModifier);
                    _modifierPrototypes.SetupModifierApplier(bleedModifier);
                }

                //On apply/init, add attackSpeed & speed buffs, after 5 seconds, remove buff.
                //var aspectOfTheCatModifier = new Modifier("AspectOfTheCat");
                //var aspectOfTheCatBuff = new StatComponent( /*5 speed & 5 attackSpeed*/);
                //aspectOfTheCatModifier.AddComponent(new TimeComponent(5, new RemoveComponent(aspectOfTheCatModifier)));
                //aspectOfTheCatModifier.AddComponent(new InitComponent(new ApplyComponent(aspectOfTheCatBuff)));


                //Graphics-, Audio-, Component, etc, whatever
            }

            [CanBeNull]
            public Modifier GetItem(string key)
            {
                return _modifierPrototypes.GetItem(key);
            }
        }

        public class ComboModifierPrototypesTest : IComboModifierPrototypes
        {
            private static ComboModifierPrototypesTest _instance;
            public ModifierPrototypesBase<ComboModifier> ModifierPrototypes { get; }

            public ComboModifierPrototypesTest()
            {
                _instance = this;
                ModifierPrototypes = new ModifierPrototypesBase<ComboModifier>();
                SetupModifierPrototypes();
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
                    var infectionEffect = new StatComponent(12, infectionTarget);
                    var infectionApply = new ApplyComponent(infectionEffect, infectionTarget);
                    infectionModifier.AddComponent(new InitComponent(infectionApply));
                    infectionModifier.AddComponent(infectionTarget);
                    infectionModifier.AddComponent(new TimeComponent(new RemoveComponent(infectionModifier), 10));
                    var infectionComboModifier = new ComboModifier(infectionModifier,
                        new ComboRecipes(new ComboRecipe(
                            new[] { new ElementalRecipe(ElementalType.Poison, 5), new ElementalRecipe(ElementalType.Bleed, 5) })),
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
                        modifierToAdd.Add(comboModifier);
                }

                return modifierToAdd;
            }
        }
    }
}
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

        protected double initialHealthCharacter, initialHealthAlly, initialHealthEnemy;

        protected const double Delta = 0.01d;
        protected const float PermanentComboModifierCooldown = 60;//PermanentMods might be able to be stripped/removed later, does it matter?

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
            initialHealthCharacter = character.Stats.Health.CurrentHealth;
            initialHealthAlly = ally.Stats.Health.CurrentHealth;
            initialHealthEnemy = enemy.Stats.Health.CurrentHealth;
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
                    var iceBoltModifier = new Modifier("IceBoltTest");
                    var target = new TargetComponent();
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementalType.Cold, 20, 10)) };
                    var effect = new DamageComponent(damageData, target);
                    var apply = new ApplyComponent(effect, target);
                    iceBoltModifier.AddComponent(new InitComponent(apply));
                    iceBoltModifier.AddComponent(target);
                    iceBoltModifier.AddComponent(new TimeComponent(new RemoveComponent(iceBoltModifier)));
                    iceBoltModifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(iceBoltModifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    //Apply on attack
                    _modifierPrototypes.SetupModifierApplier(iceBoltModifier);
                }
                {
                    //StackableSpiderPoison, removed after 10 seconds
                    //-Each stack increases DoT damage by 2
                    //-Each stack increases current duration by 2, to max 10 stacks
                    var spiderPoisonModifier = new Modifier("SpiderPoisonTest");
                    var target = new TargetComponent();
                    var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var effect = new DamageComponent(damageData, target);
                    //var stack = new StackComponent((data, value) => data[0].BaseDamage += value, 10);
                    var apply = new ApplyComponent(effect, target);
                    spiderPoisonModifier.AddComponent(new InitComponent(apply)); //Apply first stack/damage on init
                    spiderPoisonModifier.AddComponent(target);
                    spiderPoisonModifier.AddComponent(new TimeComponent(effect, 2, true)); //Every 2 seconds, deal 5 damage
                    spiderPoisonModifier.AddComponent(new TimeComponent(new RemoveComponent(spiderPoisonModifier),
                        10)); //Remove after 10 secs
                    //spiderPoisonModifier.AddComponent(stack);
                    spiderPoisonModifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(spiderPoisonModifier);
                    _modifierPrototypes.SetupModifierApplier(spiderPoisonModifier);
                }
                {
                    //RefreshableCobraVenom, removed after 10 seconds
                    //-Refresh = refreshes duration (timer)
                    //TODO -Refresh = refreshes duration (timer) & increased duration by flat 10%
                    //TODO -Refresh = refreshes duration (timer) & intensify effect?
                    var cobraVenomModifier = new Modifier("CobraVenomTest");
                    var target = new TargetComponent();
                    var damageData = new[]
                        { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 5, 20)) };
                    var effect = new DamageComponent(damageData, target);
                    var time = new TimeComponent(new RemoveComponent(cobraVenomModifier), 10);
                    var refresh = new RefreshComponent(time);
                    var apply = new ApplyComponent(effect, target);
                    cobraVenomModifier.AddComponent(new InitComponent(apply)); //Apply first stack/damage on init
                    cobraVenomModifier.AddComponent(target);
                    cobraVenomModifier.AddComponent(new TimeComponent(effect, 2, true)); //Every 2 seconds, deal 5 damage
                    cobraVenomModifier.AddComponent(time); //Remove after 10 secs
                    cobraVenomModifier.AddComponent(refresh);
                    cobraVenomModifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(cobraVenomModifier);
                    _modifierPrototypes.SetupModifierApplier(cobraVenomModifier);
                }
                {
                    //PassiveSelfHeal
                    var selfHealModifier = new Modifier("PassiveSelfHealTest");
                    var target = new TargetComponent();
                    var effect = new HealComponent(10, target);
                    var apply = new ApplyComponent(effect, target);
                    selfHealModifier.AddComponent(new InitComponent(apply));
                    selfHealModifier.AddComponent(target);
                    selfHealModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(selfHealModifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    //SetupModifierApplier(selfHealModifier, LegalTarget.Self);
                }
                {
                    var allyHealModifier = new Modifier("AllyHealTest");
                    var target = new TargetComponent();
                    var effect = new HealComponent(10, target);
                    var apply = new ApplyComponent(effect, target);
                    allyHealModifier.AddComponent(new InitComponent(apply));
                    allyHealModifier.AddComponent(target);
                    allyHealModifier.AddComponent(new TimeComponent(new RemoveComponent(allyHealModifier)));
                    allyHealModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(allyHealModifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    _modifierPrototypes.SetupModifierApplier(allyHealModifier, LegalTarget.DefaultFriendly);
                }
                {
                    //BasicPoison, removed after 10 seconds
                    var poisonModifier = new Modifier("PoisonTest");
                    var target = new TargetComponent();
                    var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Poison, 20, 20)) };
                    var effect = new DamageComponent(damageData, target);
                    var apply = new ApplyComponent(effect, target);
                    poisonModifier.AddComponent(new InitComponent(apply)); //Apply damage on init
                    poisonModifier.AddComponent(target);
                    poisonModifier.AddComponent(new TimeComponent(effect, 2, true)); //Every 2 seconds, deal 5 damage
                    poisonModifier.AddComponent(new TimeComponent(new RemoveComponent(poisonModifier), 10)); //Remove after 10 secs
                    poisonModifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(poisonModifier);
                    _modifierPrototypes.SetupModifierApplier(poisonModifier);
                }
                {
                    //BasicBleed, removed after 10 seconds
                    var bleedModifier = new Modifier("BleedTest");
                    var target = new TargetComponent();
                    var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Bleed, 20, 20)) };
                    var effect = new DamageComponent(damageData, target);
                    var apply = new ApplyComponent(effect, target);
                    bleedModifier.AddComponent(new InitComponent(apply)); //Apply damage on init
                    bleedModifier.AddComponent(target);
                    bleedModifier.AddComponent(new TimeComponent(effect, 2, true)); //Every 2 seconds, deal 5 damage
                    bleedModifier.AddComponent(new TimeComponent(new RemoveComponent(bleedModifier), 10)); //Remove after 10 secs
                    bleedModifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(bleedModifier);
                    _modifierPrototypes.SetupModifierApplier(bleedModifier);
                }
                {
                    //MovementSpeedOfCat, removed after 10 seconds
                    var movementSpeedOfCatModifier = new Modifier("MovementSpeedOfCatTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatComponent(new[] { new Stat(StatType.MovementSpeed){baseValue = 5} }, target);
                    var apply = new ApplyComponent(effect, target);
                    movementSpeedOfCatModifier.AddComponent(new InitComponent(apply)); //Apply stat on init
                    movementSpeedOfCatModifier.AddComponent(target);
                    movementSpeedOfCatModifier.AddComponent(new TimeComponent(new RemoveComponent(movementSpeedOfCatModifier), 10)); //Remove after 10 secs
                    movementSpeedOfCatModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(movementSpeedOfCatModifier);
                }
                {
                    //AttackSpeedOfCatTest, removed after 10 seconds
                    var attackSpeedOfCatModifier = new Modifier("AttackSpeedOfCatTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatComponent(new[] { new Stat(StatType.AttackSpeed){baseValue = 5} }, target);
                    var apply = new ApplyComponent(effect, target);
                    attackSpeedOfCatModifier.AddComponent(new InitComponent(apply)); //Apply stat on init
                    attackSpeedOfCatModifier.AddComponent(target);
                    attackSpeedOfCatModifier.AddComponent(new TimeComponent(new RemoveComponent(attackSpeedOfCatModifier), 10)); //Remove after 10 secs
                    attackSpeedOfCatModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(attackSpeedOfCatModifier);
                }
                {
                    //Disarm modifier
                    var disarmModifier = new Modifier("DisarmModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Disarm, 2f, target);
                    var apply = new ApplyComponent(effect, target);
                    disarmModifier.AddComponent(new InitComponent(apply));
                    disarmModifier.AddComponent(target);
                    disarmModifier.AddComponent(new TimeComponent(new RemoveComponent(disarmModifier)));
                    disarmModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(disarmModifier);
                    _modifierPrototypes.SetupModifierApplier(disarmModifier);
                }
                {
                    //Cast modifier
                    var silenceModifier = new Modifier("SilenceModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Silence, 2f, target);
                    var apply = new ApplyComponent(effect, target);
                    silenceModifier.AddComponent(new InitComponent(apply));
                    silenceModifier.AddComponent(target);
                    silenceModifier.AddComponent(new TimeComponent(new RemoveComponent(silenceModifier)));
                    silenceModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(silenceModifier);
                    _modifierPrototypes.SetupModifierApplier(silenceModifier);
                }
                {
                    //Root timed modifier (enigma Q)
                    var timedRootModifier = new Modifier("RootTimedModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Root, 0.1f, target);
                    var apply = new ApplyComponent(effect, target);
                    timedRootModifier.AddComponent(new InitComponent(apply));
                    timedRootModifier.AddComponent(target);
                    timedRootModifier.AddComponent(new TimeComponent(effect, 1, true));
                    timedRootModifier.AddComponent(new TimeComponent(new RemoveComponent(timedRootModifier), 10));
                    timedRootModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(timedRootModifier);
                    _modifierPrototypes.SetupModifierApplier(timedRootModifier);
                }
                {
                    //Delayed silence modifier
                    var delayedSilenceModifier = new Modifier("DelayedSilenceModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Silence, 1, target);
                    delayedSilenceModifier.AddComponent(target);
                    delayedSilenceModifier.AddComponent(new TimeComponent(effect, 1));
                    delayedSilenceModifier.AddComponent(new TimeComponent(new RemoveComponent(delayedSilenceModifier), 2));
                    delayedSilenceModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(delayedSilenceModifier);
                    _modifierPrototypes.SetupModifierApplier(delayedSilenceModifier);
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
                    var aspectOfTheCatModifier = new Modifier("AspectOfTheCatTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatComponent(new[] { new Stat(StatType.MovementSpeed) { baseValue = 10 }}, target);
                    var apply = new ApplyComponent(effect, target);
                    aspectOfTheCatModifier.AddComponent(new InitComponent(apply));
                    aspectOfTheCatModifier.AddComponent(target);
                    aspectOfTheCatModifier.AddComponent(new TimeComponent(new RemoveComponent(aspectOfTheCatModifier), 10));
                    aspectOfTheCatModifier.FinishSetup();
                    var aspectOfTheCatComboModifier = new ComboModifier(aspectOfTheCatModifier,
                        new ComboRecipes(new ComboRecipe(new[] { "MovementSpeedOfCatTest", "AttackSpeedOfCatTest" })),
                        1);
                    ModifierPrototypes.AddModifier(aspectOfTheCatComboModifier);
                }
                {
                    //Poison & bleed = infection
                    var infectionModifier = new Modifier("InfectionTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new DamageComponent(
                        new[]
                        {
                            new DamageData(10, DamageType.Physical, new ElementData(ElementalType.Bleed | ElementalType.Poison, 30, 50))
                        }, target);
                    var apply = new ApplyComponent(effect, target);
                    infectionModifier.AddComponent(new InitComponent(apply));
                    infectionModifier.AddComponent(target);
                    infectionModifier.AddComponent(new TimeComponent(effect, 2, true));
                    infectionModifier.AddComponent(new TimeComponent(new RemoveComponent(infectionModifier), 10));
                    infectionModifier.FinishSetup();
                    var infectionComboModifier = new ComboModifier(infectionModifier,
                        new ComboRecipes(new ComboRecipe(
                            new[]{new ElementalRecipe(ElementalType.Poison, 5), new ElementalRecipe(ElementalType.Bleed, 5)})),
                        1);
                    ModifierPrototypes.AddModifier(infectionComboModifier);
                }
                {
                    //10k health = giant
                    var giantModifier = new Modifier("GiantTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    //Physical resistances
                    var effect = new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }, target);
                    var apply = new ApplyComponent(effect, target);
                    giantModifier.AddComponent(new InitComponent(apply));
                    giantModifier.AddComponent(target);
                    giantModifier.FinishSetup();
                    var statsNeeded = new[] { new Stat(StatType.Health) };
                    statsNeeded[0].Init(10000);
                    var giantComboModifier = new ComboModifier(giantModifier, new ComboRecipes(new ComboRecipe(statsNeeded)),
                        PermanentComboModifierCooldown);
                    ModifierPrototypes.AddModifier(giantComboModifier);
                }
                {
                    //10k health = temporary giant
                    var timedGiantModifier = new Modifier("TimedGiantTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    //Physical resistances
                    var effect = new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }, target);
                    var apply = new ApplyComponent(effect, target);
                    timedGiantModifier.AddComponent(new InitComponent(apply));
                    timedGiantModifier.AddComponent(target);
                    timedGiantModifier.AddComponent(new TimeComponent(new RemoveComponent(timedGiantModifier), 10));
                    timedGiantModifier.FinishSetup();
                    var statsNeeded = new[] { new Stat(StatType.Health) };
                    statsNeeded[0].Init(10000);
                    var timedGiantComboModifier = new ComboModifier(timedGiantModifier, new ComboRecipes(new ComboRecipe(statsNeeded)),
                        PermanentComboModifierCooldown);
                    ModifierPrototypes.AddModifier(timedGiantComboModifier);
                }
            }

            [CanBeNull]
            public ComboModifier GetItem(string key)
            {
                return ModifierPrototypes.GetItem(key);
            }

            public static HashSet<ComboModifier> CheckForComboRecipes(HashSet<string> modifierIds, ElementController elementController, Stats stats)
            {
                HashSet<ComboModifier> modifierToAdd = new HashSet<ComboModifier>();
                if (_instance == null)
                {
                    Log.Warning("ComboModifier instance is null, this is bad, unless this is a unit test");
                    return modifierToAdd;
                }

                foreach (var comboModifier in _instance.ModifierPrototypes.Values)
                {
                    if (comboModifier.CheckRecipes(modifierIds, elementController, stats))
                        modifierToAdd.Add(comboModifier);
                }

                return modifierToAdd;
            }
        }
    }
}
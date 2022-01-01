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
            initialDamageCharacter = character.Stats.Damage.DamageSum();
            initialDamageAlly = ally.Stats.Damage.DamageSum();
            initialDamageEnemy = enemy.Stats.Damage.DamageSum();

            enemyDummies = new Being[5];
            for (int i = 0; i < 5; i++)
            {
                enemyDummies[0] = new Being(new BeingProperties()
                {
                    Id = "enemy", Health = 1, DamageData = new DamageData(1, DamageType.Physical, null), MovementSpeed = 1, UnitType = UnitType.Enemy
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

        public class ModifierPrototypesTest
        {
            private readonly ModifierPrototypesBase<IModifier> _modifierPrototypes;

            public ModifierPrototypesTest()
            {
                _modifierPrototypes = new ModifierPrototypesBase<IModifier>();
                SetupModifierPrototypes();
            }

            private void SetupModifierPrototypes()
            {
                {
                    //IceBoltDebuff
                    var modifier = new Modifier("IceBoltTest");
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementalType.Cold, 20, 10)) };
                    var target = new TargetComponent();
                    var effect = new DamageComponent(damageData, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier)));
                    modifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    var modifier = new Modifier("SpiderPoisonTest");
                    var target = new TargetComponent();
                    var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var effect = new DamageComponent(damageData, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(effect, 2, true));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10));
                    modifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //PassiveSelfHeal
                    var modifier = new Modifier("PassiveSelfHealTest");
                    var target = new TargetComponent();
                    var effect = new HealComponent(10, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    //SetupModifierApplier(selfHealModifier, LegalTarget.Self);
                }
                {
                    var modifier = new Modifier("AllyHealTest");
                    var target = new TargetComponent();
                    var effect = new HealComponent(10, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier)));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    _modifierPrototypes.SetupModifierApplier(modifier, LegalTarget.DefaultFriendly);
                }
                {
                    //BasicPoison, removed after 10 seconds
                    var modifier = new Modifier("PoisonTest");
                    var target = new TargetComponent();
                    var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Poison, 20, 20)) };
                    var effect = new DamageComponent(damageData, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect)); //Apply damage on init
                    modifier.AddComponent(new TimeComponent(effect, 2, true)); //Every 2 seconds, deal 5 damage
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10)); //Remove after 10 secs
                    modifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //BasicBleed, removed after 10 seconds
                    var modifier = new Modifier("BleedTest");
                    var target = new TargetComponent();
                    var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Bleed, 20, 20)) };
                    var effect = new DamageComponent(damageData, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect)); //Apply damage on init
                    modifier.AddComponent(new TimeComponent(effect, 2, true)); //Every 2 seconds, deal 5 damage
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10)); //Remove after 10 secs
                    modifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //MovementSpeedOfCat, removed after 10 seconds
                    var modifier = new Modifier("MovementSpeedOfCatTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatComponent(new[] { new Stat(StatType.MovementSpeed, 5) }, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect)); //Apply stat on init
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10)); //Remove after 10 secs
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //AttackSpeedOfCatTest, removed after 10 seconds
                    var modifier = new Modifier("AttackSpeedOfCatTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatComponent(new[] { new Stat(StatType.AttackSpeed, 5) }, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect)); //Apply stat on init
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10)); //Remove after 10 secs
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Disarm modifier
                    var modifier = new Modifier("DisarmModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Disarm, 2f, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier)));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Cast modifier
                    var modifier = new Modifier("SilenceModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Silence, 2f, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier)));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Root timed modifier (enigma Q)
                    var modifier = new Modifier("RootTimedModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Root, 0.1f, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(effect, 1, true));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Delayed silence modifier
                    var modifier = new Modifier("DelayedSilenceModifierTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatusComponent(StatusEffect.Silence, 1, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new TimeComponent(effect, 1));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 2));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //All possible tags modifiers
                    var modifier = new Modifier("ManyTagsTest");
                    var target = new TargetComponent();
                    var damageData = new[]
                    {
                        new DamageData(1, DamageType.Magical, new ElementData(ElementalType.Acid, 10, 20)),
                        new DamageData(1, DamageType.Pure, new ElementData(ElementalType.Bleed, 10, 20)),
                        new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Cold, 10, 20)),
                    };
                    var effect = new DamageComponent(damageData, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(effect, 2, true));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier), 10));
                    modifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Damage on kill
                    var modifier = new Modifier("DamageOnKillTest");
                    var conditionData = new ConditionData(ConditionTarget.Self, BeingConditionEvent.KillEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData);
                    var effect = new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }, target);
                    var apply = new ApplyComponent(effect, target, conditionData);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(apply));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Plain add damage permanently
                    var modifier = new Modifier("AddStatDamageTest");
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }, target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Damage on death
                    var modifier = new Modifier("DamageOnDeathTest");
                    var conditionData = new ConditionData(ConditionTarget.Acter, BeingConditionEvent.DeathEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData);
                    var effect = new DamageComponent(new []{new DamageData(double.MaxValue, DamageType.Magical)}, target);
                    var apply = new ApplyComponent(effect, target, conditionData);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(apply));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Timed damage on kill
                    var modifier = new Modifier("TimedDamageOnKillTest");
                    var conditionData = new ConditionData(ConditionTarget.Self, BeingConditionEvent.KillEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData);
                    var effect = new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }, target);
                    var apply = new ApplyComponent(effect, target, conditionData);
                    var cleanUp = new CleanUpComponent(apply);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(apply));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier, cleanUp), 5));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Thorns on hit
                    var modifier = new Modifier("ThornsOnHitTest");
                    var conditionData = new ConditionData(ConditionTarget.Acter, BeingConditionEvent.HitEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData);
                    var effect = new DamageComponent(new []{new DamageData(5, DamageType.Physical)}, target);
                    var apply = new ApplyComponent(effect, target, conditionData);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(apply));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //TODO We might come into trouble with multiple target components, since rn we rely on having only one in modifier
                    //Heal on death, once
                    var modifier = new Modifier("HealOnDeathTest");
                    var conditionData = new ConditionData(ConditionTarget.Self, BeingConditionEvent.DeathEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData);
                    var effect = new HealComponent(10, target);
                    var apply = new ApplyComponent(effect, target, conditionData);
                    var cleanUp = new CleanUpComponent(apply);
                    var removeEffect = new RemoveComponent(modifier, cleanUp);
                    var applyRemoval = new ApplyComponent(removeEffect, target, conditionData);
                    modifier.AddComponent(target);
                    modifier.AddComponent(apply);
                    modifier.AddComponent(new InitComponent(apply, applyRemoval));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Heal stat based
                    var modifier = new Modifier("HealStatBasedTest");
                    var target = new TargetComponent(LegalTarget.Beings);
                    var effect = new HealStatBasedComponent(target);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Heal yourself on healing someone else
                    var modifier = new Modifier("HealOnHealTest");
                    var conditionData = new ConditionData(ConditionTarget.SelfSelf, BeingConditionEvent.HealEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData);
                    var effect = new HealStatBasedComponent(target);
                    var apply = new ApplyComponent(effect, target, conditionData);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(apply));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Damage increased per stack dot
                    var modifier = new Modifier("DoTStackTest");
                    var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var target = new TargetComponent(LegalTarget.Beings);
                    var effect = new DamageComponent(damageData, target, DamageComponent.DamageComponentStackEffect.Add);
                    var timeRemove = new TimeComponent(new RemoveComponent(modifier), 10);
                    modifier.AddComponent(target);
                    modifier.AddComponent(timeRemove);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(effect, 2, true));
                    modifier.AddComponent(new StackComponent(new StackComponentProperties(effect) { Value = 2 }));
                    modifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Refresh duration DoT
                    var modifier = new Modifier("DoTRefreshTest");
                    var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var target = new TargetComponent(LegalTarget.Beings);
                    var effect = new DamageComponent(damageData, target);
                    var timeRemove = new TimeComponent(new RemoveComponent(modifier), 10);
                    modifier.AddComponent(target);
                    modifier.AddComponent(timeRemove);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new TimeComponent(effect, 2, true));
                    modifier.AddComponent(new RefreshComponent(timeRemove, RefreshEffectType.RefreshDuration));
                    modifier.FinishSetup(damageData);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Silence on 4 stacks
                    var modifier = new Modifier("SilenceXStacksTest");
                    var target = new TargetComponent(LegalTarget.Beings);
                    var effect = new StatusComponent(StatusEffect.Silence, 4, target, StatusComponent.StatusComponentStackEffect.Effect);
                    var timeRemove = new TimeComponent(new RemoveComponent(modifier), 10);
                    modifier.AddComponent(target);
                    modifier.AddComponent(timeRemove);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.AddComponent(new StackComponent(new StackComponentProperties(effect)
                        { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 4 }));
                    modifier.AddComponent(new RefreshComponent(timeRemove, RefreshEffectType.RefreshDuration));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Apply a new modifier that Stuns, on 3 stacks (effect is an example, it can be much more nuanced than that)
                    var stunModifier = new Modifier("GenericStunModifierTest");
                    var stunTarget = new TargetComponent();
                    var stunEffect = new StatusComponent(StatusEffect.Stun, 2, stunTarget);
                    stunModifier.AddComponent(stunTarget);
                    stunModifier.AddComponent(new InitComponent(stunEffect));
                    stunModifier.AddComponent(new TimeComponent(new RemoveComponent(stunModifier)));
                    stunModifier.FinishSetup();
                    _modifierPrototypes.AddModifier(stunModifier);

                    var modifier = new Modifier("ApplyStunModifierXStacksTestApplier", true);
                    var target = new TargetComponent(LegalTarget.Self, true);
                    var effect = new ApplierComponent(stunModifier, target, stackEffect: true);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new StackComponent(new StackComponentProperties(effect)
                        { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 3 }));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);//ApplierApplier
                }

                

                /*{
                    //Applier onDeath (lowers health), copies itself, infinite loop possible?
                    var modifier = new Modifier("DeathHealthTestApplier", true);
                    var conditionData = new ConditionData(ConditionTarget.Acter, BeingConditionEvent.DeathEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData, true);
                    var effect = new StatComponent(new[] { new Stat(StatType.Health, -15) }, target);
                    var applierEffect = new ApplierComponent(modifier, target);
                    var apply = new ApplyComponent(applierEffect, target, conditionData);
                    modifier.AddComponent(target);
                    modifier.AddComponent(apply);
                    modifier.AddComponent(new InitComponent(effect));
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }*/
            }

            [CanBeNull]
            public IModifier GetItem(string key)
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
            public ModifierPrototypesBase<IComboModifier> ModifierPrototypes { get; }

            public ComboModifierPrototypesTest()
            {
                _instance = this;
                ModifierPrototypes = new ModifierPrototypesBase<IComboModifier>();
                SetupModifierPrototypes();
            }

            private void SetupModifierPrototypes()
            {
                //Scope brackets so it's impossible to use a wrong component/modifier
                {
                    //Aspect of the cat
                    var comboModifier = new ComboModifier("ComboAspectOfTheCatTest",
                        new ComboRecipes(new ComboRecipe(new[] { "MovementSpeedOfCatTest", "AttackSpeedOfCatTest" })), 1);
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new StatComponent(new[] { new Stat(StatType.MovementSpeed, 10) }, target);
                    comboModifier.AddComponent(target);
                    comboModifier.AddComponent(new InitComponent(effect));
                    comboModifier.AddComponent(new TimeComponent(new RemoveComponent(comboModifier), 10));
                    comboModifier.FinishSetup();
                    ModifierPrototypes.AddModifier(comboModifier);
                }
                {
                    //Poison & bleed = infection
                    var comboModifier = new ComboModifier("ComboInfectionTest",
                        new ComboRecipes(new ComboRecipe(new[]
                            { new ElementalRecipe(ElementalType.Poison, 5), new ElementalRecipe(ElementalType.Bleed, 5) })), 1);
                    var target = new TargetComponent(LegalTarget.Self);
                    var effect = new DamageComponent(
                        new[]
                        {
                            new DamageData(10, DamageType.Physical, new ElementData(ElementalType.Bleed | ElementalType.Poison, 30, 50))
                        }, target);
                    comboModifier.AddComponent(target);
                    comboModifier.AddComponent(new InitComponent(effect));
                    comboModifier.AddComponent(new TimeComponent(effect, 2, true));
                    comboModifier.AddComponent(new TimeComponent(new RemoveComponent(comboModifier), 10));
                    comboModifier.FinishSetup();
                    ModifierPrototypes.AddModifier(comboModifier);
                }
                {
                    //10k health = giant
                    var statsNeeded = new[] { new Stat(StatType.Health, 10000) };
                    var comboModifier = new ComboModifier("ComboGiantTest", new ComboRecipes(new ComboRecipe(statsNeeded)),
                        PermanentComboModifierCooldown);
                    var target = new TargetComponent(LegalTarget.Self);
                    //Physical resistances
                    var effect = new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }, target);
                    comboModifier.AddComponent(target);
                    comboModifier.AddComponent(new InitComponent(effect));
                    comboModifier.FinishSetup();
                    ModifierPrototypes.AddModifier(comboModifier);
                }
                {
                    //10k health = temporary giant
                    var statsNeeded = new[] { new Stat(StatType.Health, 10000) };
                    var comboModifier = new ComboModifier("ComboTimedGiantTest", new ComboRecipes(new ComboRecipe(statsNeeded)),
                        PermanentComboModifierCooldown);
                    var target = new TargetComponent(LegalTarget.Self);
                    //Physical resistances
                    var effect = new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }, target);
                    comboModifier.AddComponent(target);
                    comboModifier.AddComponent(new InitComponent(effect));
                    comboModifier.AddComponent(new TimeComponent(new RemoveComponent(comboModifier), 10));
                    comboModifier.FinishSetup();
                    ModifierPrototypes.AddModifier(comboModifier);
                }
            }

            [CanBeNull]
            public IComboModifier GetItem(string key)
            {
                return ModifierPrototypes.GetItem(key);
            }

            public static HashSet<IComboModifier> CheckForComboRecipes(HashSet<string> modifierIds, ElementController elementController, Stats stats)
            {
                HashSet<IComboModifier> modifierToAdd = new HashSet<IComboModifier>();
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
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
                    //IceboltDebuff
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementalType.Cold, 20, 10)) };
                    var properties = new ModifierGenerationProperties("IceBoltTest");
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnInit();
                    properties.SetRemovable();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //SpiderPoison
                    var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var properties = new ModifierGenerationProperties("SpiderPoisonTest");
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(2, true);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //PassiveSelfHeal
                    var properties = new ModifierGenerationProperties("PassiveSelfHealTest");
                    properties.AddEffect(new HealComponent(10));
                    properties.SetEffectOnInit();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //AllyHealTest
                    var properties = new ModifierGenerationProperties("AllyHealTest");
                    properties.AddEffect(new HealComponent(10));
                    properties.SetEffectOnInit();
                    properties.SetRemovable();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    //Forever buff (applier), not refreshable or stackable (for now)
                    _modifierPrototypes.SetupModifierApplier(modifier, LegalTarget.DefaultFriendly);
                }
                {
                    //BasicPoison
                    var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Poison, 20, 20)) };
                    var properties = new ModifierGenerationProperties("PoisonTest");
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(2, true);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //BasicBleed
                    var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Bleed, 20, 20)) };
                    var properties = new ModifierGenerationProperties("BleedTest");
                    properties.SetEffectOnInit();
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnTime(2, true);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //MovementSpeedOfCat
                    var properties = new ModifierGenerationProperties("MovementSpeedOfCatTest");
                    properties.AddEffect(new StatComponent(new[] { new Stat(StatType.MovementSpeed, 5) }));
                    properties.SetEffectOnInit();
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //AttackSpeedOfCatTest
                    var properties = new ModifierGenerationProperties("AttackSpeedOfCatTest");
                    properties.AddEffect(new StatComponent(new[] { new Stat(StatType.AttackSpeed, 5) }));
                    properties.SetEffectOnInit();
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Disarm
                    var properties = new ModifierGenerationProperties("DisarmModifierTest");
                    properties.AddEffect(new StatusComponent(StatusEffect.Disarm, 2f));
                    properties.SetEffectOnInit();
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Cast
                    var properties = new ModifierGenerationProperties("SilenceModifierTest");
                    properties.AddEffect(new StatusComponent(StatusEffect.Silence, 2f));
                    properties.SetEffectOnInit();
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Root timed modifier (enigma Q)
                    var properties = new ModifierGenerationProperties("RootTimedModifierTest");
                    properties.AddEffect(new StatusComponent(StatusEffect.Root, 0.1f));
                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(1, true);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Delayed silence
                    var properties = new ModifierGenerationProperties("DelayedSilenceModifierTest");
                    properties.AddEffect(new StatusComponent(StatusEffect.Silence, 1));
                    properties.SetEffectOnTime(1, false);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //All possible tags
                    var damageData = new[]
                    {
                        new DamageData(1, DamageType.Magical, new ElementData(ElementalType.Acid, 10, 20)),
                        new DamageData(1, DamageType.Pure, new ElementData(ElementalType.Bleed, 10, 20)),
                        new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Cold, 10, 20)),
                    };
                    var properties = new ModifierGenerationProperties("ManyTagsTest");
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(2, true);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Damage on kill
                    var properties = new ModifierGenerationProperties("DamageOnKillTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.KillEvent));
                    properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Plain add damage permanently
                    var properties = new ModifierGenerationProperties("AddStatDamageTest");
                    properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));
                    properties.SetEffectOnInit();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Damage on death
                    var damageData = new []{new DamageData(double.MaxValue, DamageType.Magical)};
                    var properties = new ModifierGenerationProperties("DamageOnDeathTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDeathEvent));
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                /*{//TODOPRIO FIXME
                    //Timed damage on kill
                    var damageData = new[] { new DamageData(2, DamageType.Physical) };
                    var properties = new ModifierGenerationProperties("TimedDamageOnKillTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.KillEvent));
                    properties.AddEffect(new DamageStatComponent(damageData), damageData);
                    properties.SetEffectOnApply();
                    properties.SetRemovable(5);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }*/
                {
                    //Thorns on hit
                    var damageData = new[] { new DamageData(5, DamageType.Physical) };
                    var properties = new ModifierGenerationProperties("ThornsOnHitTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.ActerSelf, ConditionEvent.OnHitEvent));
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                // {
                //     //TODO Implement a generation of the modifier below
                //     //Heal on death, once
                //     var properties = new ModifierGenerationProperties("HealOnDeathTest", LegalTarget.Beings);
                //     properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.OnDeathEvent));
                //     properties.AddEffect(new HealComponent(10));
                //     properties.SetEffectOnApply();
                //
                //     var modifier = ModifierGenerator.GenerateModifier(properties);
                //     _modifierPrototypes.AddModifier(modifier);
                // }
                {
                    //TODO We might come into trouble with multiple target components, since rn we rely on having only one in modifier
                    //Heal on death, once
                    var modifier = new Modifier("HealOnDeathTest");
                    var conditionData = new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.OnDeathEvent);
                    var target = new TargetComponent(LegalTarget.Beings, conditionData);
                    var effect = new HealComponent(10);
                    effect.Setup(target);
                    var apply = new ConditionalApplyComponent(effect, target, conditionData);
                    var cleanUp = new CleanUpComponent(apply);
                    var removeEffect = new RemoveComponent(modifier, cleanUp);
                    var applyRemoval = new ConditionalApplyComponent(removeEffect, target, conditionData);
                    modifier.AddComponent(target);
                    modifier.AddComponent(apply);//TODO Add component in generator, whats the point?
                    modifier.AddComponent(new InitComponent(apply, applyRemoval));//TODO Separate data for each apply & effect? SetEffectOnApply(index 1)?
                    modifier.FinishSetup();
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Heal stat based
                    var properties = new ModifierGenerationProperties("HealStatBasedTest", LegalTarget.Beings);
                    properties.AddEffect(new HealStatBasedComponent());
                    properties.SetEffectOnInit();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Heal yourself on healing someone else
                    var properties = new ModifierGenerationProperties("HealOnHealTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfSelf, ConditionEvent.HealEvent));
                    properties.AddEffect(new HealStatBasedComponent());
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Damage increased per stack dot
                    var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var properties = new ModifierGenerationProperties("DoTStackTest", LegalTarget.Beings);
                    properties.AddEffect(new DamageComponent(damageData, DamageComponent.DamageComponentStackEffect.Add), damageData);
                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(2, true);
                    properties.SetEffectOnStack(new StackComponentProperties() { Value = 2 });
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Refresh duration DoT
                    var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                    var properties = new ModifierGenerationProperties("DoTRefreshTest", LegalTarget.Beings);
                    properties.AddEffect(new DamageComponent(damageData, DamageComponent.DamageComponentStackEffect.Add), damageData);
                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(2, true);
                    properties.SetRefreshable(RefreshEffectType.RefreshDuration);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Silence on 4 stacks
                    var properties = new ModifierGenerationProperties("SilenceXStacksTest", LegalTarget.Beings);
                    properties.AddEffect(new StatusComponent(StatusEffect.Silence, 4, StatusComponent.StatusComponentStackEffect.Effect));
                    properties.SetEffectOnInit();
                    properties.SetEffectOnStack(new StackComponentProperties()
                        { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 4 });
                    properties.SetRefreshable(RefreshEffectType.RefreshDuration);
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Apply a new modifier that Stuns, on 3 stacks (effect is an example, it can be much more nuanced than that)
                    var stunProperties = new ModifierGenerationProperties("GenericStunModifierTest");
                    stunProperties.AddEffect(new StatusComponent(StatusEffect.Stun, 2));
                    stunProperties.SetEffectOnInit();
                    stunProperties.SetRemovable();

                    var stunModifier = ModifierGenerator.GenerateModifier(stunProperties);
                    _modifierPrototypes.AddModifier(stunModifier);


                    var properties = new ModifierGenerationProperties("ApplyStunModifierXStacksTestApplier");
                    properties.SetApplier();
                    properties.AddEffect(new ApplierEffectComponent(stunModifier, isStackEffect: true));
                    properties.SetEffectOnStack(new StackComponentProperties()
                        { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 3 });

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                    _modifierPrototypes.SetupModifierApplier(modifier);
                }
                {
                    //Damage on low health
                    var damageData = new[] { new DamageData(50, DamageType.Physical) };
                    var properties = new ModifierGenerationProperties("DamageOnLowHealthTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent));
                    properties.AddEffect(new DamageStatComponent(damageData,
                        new ConditionCheckData(ConditionBeingStatus.HealthIsLow)), damageData);
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Flag
                    var flagDamageData = new[] { new DamageData(1, DamageType.Physical) };
                    var flagProperties = new ModifierGenerationProperties("FlagTest", LegalTarget.Beings);
                    flagProperties.AddEffect(new DamageStatComponent(flagDamageData), flagDamageData);
                    flagProperties.SetEffectOnInit();

                    var flagModifier = ModifierGenerator.GenerateModifier(flagProperties);
                    _modifierPrototypes.AddModifier(flagModifier);

                    //Damage on modifier id (flag)
                    var damageData = new[] { new DamageData(double.MaxValue, DamageType.Physical) };
                    var properties = new ModifierGenerationProperties("DamageOnModifierIdTest", LegalTarget.Beings);
                    properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData("FlagTest")), damageData);
                    properties.SetEffectOnInit();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //If enemy is on fire, deal damage to him, on hit
                    var damageData = new[] { new DamageData(10000, DamageType.Physical) };
                    var properties = new ModifierGenerationProperties("DealDamageOnElementalIntensityTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.ActerSelf, ConditionEvent.HitEvent));
                    properties.AddEffect(new DamageComponent(damageData,
                            conditionCheckData: new ConditionCheckData(ElementalType.Fire, ComparisonCheck.GreaterOrEqual,
                                Curves.ElementIntensity.Evaluate(900))), damageData);
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //TODO CleanUp? Unit test isnt up
                    //If health on IsLow, add 50 physical damage, if not, remove 50 physical damage
                    var damageData = new[] { new DamageData(50, DamageType.Physical) };
                    var properties = new ModifierGenerationProperties("DamageOnLowHealthRemoveTest", LegalTarget.Beings);
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent));
                    properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData(ConditionBeingStatus.HealthIsLow)),
                        damageData);
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //On hit, attack yourself
                    var properties = new ModifierGenerationProperties("AttackYourselfOnHitTest");
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfSelf, ConditionEvent.OnHitEvent));
                    properties.AddEffect(new AttackComponent());
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //On damaged, attack yourself
                    var properties = new ModifierGenerationProperties("AttackYourselfOnDamagedTest");
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfSelf, ConditionEvent.OnDamagedEvent));
                    properties.AddEffect(new AttackComponent());
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
                }
                {
                    //Hit, heal yourself
                    var properties = new ModifierGenerationProperties("HealYourselfHitTest");
                    properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfSelf, ConditionEvent.HitEvent));
                    properties.AddEffect(new HealStatBasedComponent());
                    properties.SetEffectOnApply();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                    _modifierPrototypes.AddModifier(modifier);
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
                    var properties = new ComboModifierGenerationProperties("ComboAspectOfTheCatTest");
                    properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { "MovementSpeedOfCatTest", "AttackSpeedOfCatTest" })));
                    properties.SetCooldown(1);

                    properties.SetEffectOnInit();
                    properties.AddEffect(new StatComponent(new[] { new Stat(StatType.MovementSpeed, 10) }));
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifierTest(properties);
                    ModifierPrototypes.AddModifier(modifier);
                }
                {
                    //Poison & bleed = infection
                    var damageData = new[]
                        { new DamageData(10, DamageType.Physical, new ElementData(ElementalType.Bleed | ElementalType.Poison, 30, 50)) };
                    var properties = new ComboModifierGenerationProperties("ComboInfectionTest");
                    properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[]
                        { new ElementalRecipe(ElementalType.Poison, 5), new ElementalRecipe(ElementalType.Bleed, 5) })));
                    properties.SetCooldown(1);

                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(2, true);
                    properties.AddEffect(new DamageComponent(damageData)/*, damageData*/);//TODO What to do with infection & such combined status res enums?
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifierTest(properties);
                    ModifierPrototypes.AddModifier(modifier);
                }
                {
                    //10k health = giant (physical res)
                    var properties = new ComboModifierGenerationProperties("ComboGiantTest");
                    properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { new Stat(StatType.Health, 10000) })));
                    properties.SetCooldown(PermanentComboModifierCooldown);

                    properties.SetEffectOnInit();
                    properties.AddEffect(new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }));

                    var modifier = ModifierGenerator.GenerateModifierTest(properties);
                    ModifierPrototypes.AddModifier(modifier);
                }
                {
                    //10k health = temporary giant (physical res)
                    var properties = new ComboModifierGenerationProperties("ComboTimedGiantTest");
                    properties.AddRecipes(new ComboRecipes(new ComboRecipe(new[] { new Stat(StatType.Health, 10000) })));
                    properties.SetCooldown(PermanentComboModifierCooldown);
                    properties.SetRemovable(10);

                    properties.SetEffectOnInit();
                    properties.AddEffect(new StatusResistanceComponent(new[] { new StatusTag(DamageType.Physical) }, new[] { 1000d }));

                    var modifier = ModifierGenerator.GenerateModifierTest(properties);
                    ModifierPrototypes.AddModifier(modifier);
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
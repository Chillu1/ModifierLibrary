using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;
using Random = System.Random;

namespace ModifierSystem
{
    public class ModifierPrototypes<TModifier> : BasePrototypeController<string, TModifier>
        where TModifier : Modifier
    {
        public ModifierPrototypes(bool includeTest = false)
        {
            if (includeTest)
                SetupTestModifiers();
        }

        public void AddModifier(TModifier modifier)
        {
            if (!modifier.ValidatePrototypeSetup())
                return;

            if (ContainsKey(modifier.Id))
            {
                Log.Error("A modifier with id: " + modifier.Id + " already exists", "modifiers");
                return;
            }

            AddItem(modifier.Id, modifier);
        }

        [CanBeNull]
        public TModifier Get(string key)
        {
            var modifier = GetItem(key);
            ValidateModifier(modifier);

            return modifier;
        }

        public TModifier GetApplier(string key)
        {
            if (!key.EndsWith("Applier"))
                key += "Applier";

            var modifier = GetItem(key);
            ValidateModifier(modifier);

            return modifier;
        }

        [CanBeNull]
        public TModifier GetRandom(Random random)
        {
            var modifier = GetRandomItem(random);
            ValidateModifier(modifier);

            return modifier;
        }

        [CanBeNull]
        public TModifier GetRandomApplier(Random random)
        {
            var modifier = GetRandomItem(random, localModifier => localModifier.IsApplierModifier);
            //Log.Info("GetRandomApplier: " + modifier, "modifiers");
            ValidateModifier(modifier);

            return modifier;
        }

        private void SetupTestModifiers()
        {
            {
                //IceboltDebuff
                var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //SpiderPoison
                var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("SpiderPoisonTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //PassiveSelfHeal
                var properties = new ModifierGenerationProperties("PassiveSelfHealTest", null);
                properties.AddEffect(new HealComponent(10));
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //AllyHealTest
                var properties = new ModifierGenerationProperties("AllyHealTest", null);
                properties.AddEffect(new HealComponent(10));
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                //Forever buff (applier), not refreshable or stackable (for now)
                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Cast, LegalTarget.DefaultFriendly);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //BasicPoison
                var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementType.Poison, 20, 20)) };
                var properties = new ModifierGenerationProperties("PoisonTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //BasicBleed
                var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementType.Bleed, 20, 20)) };
                var properties = new ModifierGenerationProperties("BleedTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //MovementSpeedOfCat
                var properties = new ModifierGenerationProperties("MovementSpeedOfCatTest", null);
                properties.AddEffect(new StatComponent(new[] { new Stat(StatType.MovementSpeed, 5) }));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //AttackSpeedOfCatTest
                var properties = new ModifierGenerationProperties("AttackSpeedOfCatTest", null);
                properties.AddEffect(new StatComponent(new[] { new Stat(StatType.AttackSpeed, 5) }));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Disarm
                var properties = new ModifierGenerationProperties("DisarmModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Disarm, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Cast
                var properties = new ModifierGenerationProperties("SilenceModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Cast);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Root timed modifier (enigma Q)
                var properties = new ModifierGenerationProperties("RootTimedModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Root, 0.1f));
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(1, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Delayed silence
                var properties = new ModifierGenerationProperties("DelayedSilenceModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 1));
                properties.SetEffectOnTime(1, false);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Cast);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //All possible tags
                var damageData = new[]
                {
                    new DamageData(1, DamageType.Magical, new ElementData(ElementType.Acid, 10, 20)),
                    new DamageData(1, DamageType.Pure, new ElementData(ElementType.Bleed, 10, 20)),
                    new DamageData(1, DamageType.Physical, new ElementData(ElementType.Cold, 10, 20)),
                };
                var properties = new ModifierGenerationProperties("ManyTagsTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Damage on kill
                var properties = new ModifierGenerationProperties("DamageOnKillTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.KillEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Plain add damage permanently
                var properties = new ModifierGenerationProperties("AddStatDamageTest", null);
                properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Damage on death
                var damageData = new[] { new DamageData(double.MaxValue, DamageType.Magical) };
                var properties = new ModifierGenerationProperties("DamageOnDeathTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDeathEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            /*{//TODOPRIO FIXME
                //Timed damage on kill
                var damageData = new[] { new DamageData(2, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("TimedDamageOnKillTest", LegalTarget.Beings);
                properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.KillEvent));
                properties.AddEffect(new DamageStatComponent(damageData), damageData);

                properties.SetRemovable(5);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }*/
            {
                //Thorns on hit
                var damageData = new[] { new DamageData(5, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("ThornsOnHitTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnHitEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            /*{
                //TODO Implement a generation of the modifier below
                //TODO We might come into trouble with multiple target components, since rn we rely on having only one in modifier
                //Heal on death, once
                var properties = new ModifierGenerationProperties("HealOnDeathTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnDeathEvent);
                properties.AddEffect(new HealComponent(10));


                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }*/
            {
                //TODO We might come into trouble with multiple target components, since rn we rely on having only one in modifier
                //Heal on death, once
                var modifier = new Modifier("HealOnDeathTest", null);
                var conditionData = new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.OnDeathEvent);
                var target = new TargetComponent(LegalTarget.Beings, conditionData.conditionEventTarget);
                var effect = new HealComponent(10);
                effect.Setup(target);
                var apply = new ConditionalApplyComponent(effect, target, conditionData.conditionEvent);
                var cleanUp = new CleanUpComponent(apply);
                var removeEffect = new RemoveComponent(modifier, cleanUp);
                var applyRemoval = new ConditionalApplyComponent(removeEffect, target, conditionData.conditionEvent);
                modifier.AddComponent(target);
                modifier.AddComponent(new InitComponent(apply,
                    applyRemoval)); //TODO Separate data for each apply & effect? SetEffectOnApply(index 1)?
                modifier.FinishSetup();
                AddModifier((TModifier)modifier);
            }
            {
                //Heal stat based
                var properties = new ModifierGenerationProperties("HealStatBasedTest", null, LegalTarget.Beings);
                properties.AddEffect(new HealStatBasedComponent());
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Heal yourself on healing someone else
                var properties = new ModifierGenerationProperties("HealOnHealTest", null, LegalTarget.Beings);
                properties.AddEffect(new HealStatBasedComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.HealEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Damage increased per stack dot
                var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("DoTStackTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData, DamageComponent.DamageComponentStackEffect.Add), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetEffectOnStack(new StackComponentProperties() { Value = 2, MaxStacks = 1000 });
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Refresh duration DoT
                var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("DoTRefreshTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData, DamageComponent.DamageComponentStackEffect.Add), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRefreshable(RefreshEffectType.RefreshDuration);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Silence on 4 stacks
                var properties = new ModifierGenerationProperties("SilenceXStacksTest", null, LegalTarget.Beings);
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 4, StatusComponent.StatusComponentStackEffect.Effect));
                properties.SetEffectOnInit();
                properties.SetEffectOnStack(new StackComponentProperties()
                    { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 4 });
                properties.SetRefreshable(RefreshEffectType.RefreshDuration);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Apply a new modifier that Stuns, on 3 stacks (effect is an example, it can be much more nuanced than that)
                var stunProperties = new ModifierGenerationProperties("GenericStunModifierTest", null);
                stunProperties.AddEffect(new StatusComponent(StatusEffect.Stun, 2));
                stunProperties.SetEffectOnInit();
                stunProperties.SetRemovable();

                var stunModifier = (TModifier)ModifierGenerator.GenerateModifier(stunProperties);
                AddModifier(stunModifier);


                var properties = new ModifierGenerationProperties("ApplyStunModifierXStacksTestApplier", null);
                properties.SetApplier(ApplierType.Attack);
                properties.AddEffect(new ApplierEffectComponent(stunModifier, isStackEffect: true));
                properties.SetEffectOnStack(new StackComponentProperties()
                    { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 3 });

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //Damage on low health
                var damageData = new[] { new DamageData(50, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnLowHealthTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData,
                    new ConditionCheckData(ConditionBeingStatus.HealthIsLow)), damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Flag
                var flagDamageData = new[] { new DamageData(1, DamageType.Physical) };
                var flagProperties = new ModifierGenerationProperties("FlagTest", null, LegalTarget.Beings);
                flagProperties.AddEffect(new DamageStatComponent(flagDamageData), flagDamageData);
                flagProperties.SetEffectOnInit();

                var flagModifier = (TModifier)ModifierGenerator.GenerateModifier(flagProperties);
                AddModifier(flagModifier);

                //Damage on modifier id (flag)
                var damageData = new[] { new DamageData(double.MaxValue, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnModifierIdTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData("FlagTest")), damageData);
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //If enemy is on fire, deal damage to him, on hit
                var damageData = new[] { new DamageData(10000, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DealDamageOnElementalIntensityTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData,
                    conditionCheckData: new ConditionCheckData(ElementType.Fire, ComparisonCheck.GreaterOrEqual,
                        BaseProject.Curves.ElementIntensity.Evaluate(900))), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.HitEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //TODO CleanUp? Unit test isnt up
                //If health on IsLow, add 50 physical damage, if not, remove 50 physical damage
                var damageData = new[] { new DamageData(50, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnLowHealthRemoveTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData(ConditionBeingStatus.HealthIsLow)),
                    damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //On hit, attack yourself
                var properties = new ModifierGenerationProperties("AttackYourselfOnHitTest", null);
                properties.AddEffect(new AttackComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.OnHitEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //On damaged, attack yourself
                var properties = new ModifierGenerationProperties("AttackYourselfOnDamagedTest", null);
                properties.AddEffect(new AttackComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.OnDamagedEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Hit, heal yourself
                var properties = new ModifierGenerationProperties("HealYourselfHitTest", null);
                properties.AddEffect(new HealStatBasedComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.HitEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Reflect back 20% of damage dealt
                var properties = new ModifierGenerationProperties("ReflectOnDamagedTest", null);
                properties.AddEffect(new DamageReflectComponent(0.2));
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDamagedEvent);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //More fixed damage per stack (on target being, probably it's own debuff modifier?) (Ursa E)
                //Workings: Attack enemy being, add timed (10s) modifier with 1 stack to enemy being.
                //  Next time we attack, we check for that modifier, call it's effect, add +1 stack, deal X damage * Y stacks

                //Stack flag modifier
                var damageData = new[] { new DamageData(2, DamageType.Physical) };
                var flagProperties = new ModifierGenerationProperties("DamagePerStackTest", null);
                flagProperties.AddEffect(
                    new DamageComponent(damageData,
                        DamageComponent.DamageComponentStackEffect.Effect |
                        DamageComponent.DamageComponentStackEffect.SetMultiplierStacksBased), damageData);
                flagProperties.SetEffectOnStack(new StackComponentProperties()
                    { WhenStackEffect = WhenStackEffect.Always, MaxStacks = 100 });
                flagProperties.SetRefreshable(RefreshEffectType.RefreshDuration);
                flagProperties.SetRemovable(10);

                var flagModifier = (TModifier)ModifierGenerator.GenerateModifier(flagProperties);
                AddModifier(flagModifier);


                var properties = new ModifierGenerationProperties("DamagePerStackTestApplier", null, LegalTarget.Beings);
                properties.SetApplier(ApplierType.Attack);
                properties.AddEffect(new ApplierEffectComponent(flagModifier));
                properties.AddConditionData(ConditionEventTarget.ActerSelf,
                    ConditionEvent.HitEvent); //This is optional, we can always apply on attack instead

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
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
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }*/

            {
                //IceboltDebuff that costs health
                var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltHealthCostTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                applierProperties.SetCost(CostType.Health, 10);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }

            {
                //IceboltDebuff cast that costs mana to cast
                var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltCastManaCostTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Cast);
                applierProperties.SetCost(CostType.Mana, 10);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //IceboltDebuff attack that costs mana to cast
                var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltAttackManaCostTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                applierProperties.SetCost(CostType.Mana, 10);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }

            {
                //IceboltDebuff that has a cooldown
                var damageData = new[] { new DamageData(2, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltCooldownTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                applierProperties.SetCooldown(5);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }

            {
                //IceboltDebuff that is automatically cast
                var damageData = new[] { new DamageData(2, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltAutomaticCastTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Cast);
                applierProperties.SetCooldown(1);
                applierProperties.SetAutomaticCast();
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }

            {
                //ThornsOnHit 0% Chance
                var damageData = new[] { new DamageData(1, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("ThornsOnHitChanceZeroTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnHitEvent);
                properties.SetChance(0);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //IncreaseDmgOnHit 50% Chance
                var damageData = new[] { new DamageData(1, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("IncreaseDmgHitHalfTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.HitEvent);
                properties.SetChance(0.5);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //HealOnHit Chance
                var properties = new ModifierGenerationProperties("HealOnHitHalfChanceTest", null);
                properties.AddEffect(new HealComponent(1));
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);
                properties.SetChance(0.5);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //IncreaseDmgOnHit 100% Chance
                var damageData = new[] { new DamageData(1, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("IncreaseDmgHitFullTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.HitEvent);
                properties.SetChance(1);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //FireAttack 50% chance to apply, 50% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitHalfTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(0.5);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                applierProperties.SetChance(0.5);

                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //FireAttack 0% chance to apply, 100% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitFullZeroTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(1);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                applierProperties.SetChance(0);

                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //FireAttack 100% chance to apply, 0% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitZeroFullTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(0);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                applierProperties.SetChance(1);

                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }
            {
                //FireAttack 100% chance to apply, 100% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitFullFullTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(1);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, ApplierType.Attack);
                applierProperties.SetChance(1);

                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }

            /*{
                //FireBall with init dmg and DoT
                var initDamageData = new[] { new DamageData(10, DamageType.Magical, new ElementData(ElementalType.Fire, 20, 10)) };
                var timeDamageData = new[] { new DamageData(3, DamageType.Magical, new ElementData(ElementalType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireBallTwoEffectTest", LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(initDamageData), initDamageData);//Init
                properties.SetEffectOnInit();
                properties.AddEffect(new DamageComponent(timeDamageData), timeDamageData);//DoT
                properties.SetEffectOnTime(1, true);
                properties.SetRemovable(5);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, ApplierType.Attack);
                var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(applierProperties);
                AddModifier(applierModifier);
            }*/

            {
                //Permanent Fire Resistance
                var properties = new ModifierGenerationProperties("ElementFireResistanceTest", null);
                properties.AddEffect(new ElementResistanceComponent(ElementType.Fire, 1000));
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Permanent Physical Resistance
                var properties = new ModifierGenerationProperties("DamagePhysicalResistanceTest", null);
                properties.AddEffect(new DamageResistanceComponent(DamageType.Physical, 1000));
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
        }

        private bool ValidateModifier(Modifier modifier)
        {
            if (modifier.TargetComponent.Target != null || modifier.TargetComponent.Owner != null)
            {
                Log.Error("Cloned prototype modifier has a Target or Owner");
                return false;
            }

            return true;
        }
    }
}
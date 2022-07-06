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

            Add(modifier.Id, modifier);
        }

        public TModifier AddModifier(ModifierGenerationProperties properties)
        {
            var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
            AddModifier(modifier);
            return modifier;
        }

        public TModifier AddModifier(ApplierModifierGenerationProperties properties)
        {
            var applierModifier = (TModifier)ModifierGenerator.GenerateApplierModifier(properties);
            AddModifier(applierModifier);
            return applierModifier;
        }

        public override TModifier Get(string key)
        {
            var modifier = base.Get(key);
            ValidateModifier(modifier);

            return modifier;
        }

        /// <summary>
        ///     Used for displaying stored modifiers UI
        /// </summary>
        public TModifier[] GetPrototypes(string[] prototypeIds)
        {
            var prototypes = new TModifier[prototypeIds.Length];
            for (int i = 0; i < prototypeIds.Length; i++)
            {
                string prototypeId = prototypeIds[i];
                var prototype = GetPrototype(prototypeId);
                if (prototype == null)
                    continue;

                prototypes[i] = prototype;
            }

            return prototypes;
        }

        [CanBeNull]
        public TModifier GetApplier(string key)
        {
            if (!key.EndsWith("Applier"))
                key += "Applier";

            var modifier = base.Get(key);
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

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //SpiderPoison
                var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("SpiderPoisonTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //PassiveSelfHeal
                var properties = new ModifierGenerationProperties("PassiveSelfHealTest", null);
                properties.AddEffect(new HealComponent(10));
                properties.SetEffectOnInit();

                AddModifier(properties);
            }
            {
                //AllyHealTest
                var properties = new ModifierGenerationProperties("AllyHealTest", null);
                properties.AddEffect(new HealComponent(10));
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = AddModifier(properties);

                //Forever buff (applier), not refreshable or stackable (for now)
                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, LegalTarget.Same);
                applierProperties.SetApplier(ApplierType.Cast);
                AddModifier(applierProperties);
            }
            {
                //BasicPoison
                var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementType.Poison, 20, 20)) };
                var properties = new ModifierGenerationProperties("PoisonTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //BasicBleed
                var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementType.Bleed, 20, 20)) };
                var properties = new ModifierGenerationProperties("BleedTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //MovementSpeedOfCat
                var properties = new ModifierGenerationProperties("MovementSpeedOfCatTest", null);
                properties.AddEffect(new StatComponent(StatType.MovementSpeed, 5));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                AddModifier(properties);
            }
            {
                //AttackSpeedOfCatTest
                var properties = new ModifierGenerationProperties("AttackSpeedOfCatTest", null);
                properties.AddEffect(new StatComponent(StatType.AttackSpeed, 5));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                AddModifier(properties);
            }
            {
                //Disarm
                var properties = new ModifierGenerationProperties("DisarmModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Disarm, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //Cast
                var properties = new ModifierGenerationProperties("SilenceModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Cast);
                AddModifier(applierProperties);
            }
            {
                //Root timed modifier (enigma Q)
                var properties = new ModifierGenerationProperties("RootTimedModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Root, 0.1f));
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(1, true);
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //Delayed silence
                var properties = new ModifierGenerationProperties("DelayedSilenceModifierTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 1));
                properties.SetEffectOnTime(1, false);
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Cast);
                AddModifier(applierProperties);
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

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //Damage on kill
                var properties = new ModifierGenerationProperties("DamageOnKillTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.KillEvent);

                AddModifier(properties);
            }
            {
                //Plain add damage permanently
                var properties = new ModifierGenerationProperties("AddStatDamageTest", null);
                properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));
                properties.SetEffectOnInit();

                AddModifier(properties);
            }
            {
                //Damage on death
                var damageData = new[] { new DamageData(double.MaxValue, DamageType.Magical) };
                var properties = new ModifierGenerationProperties("DamageOnDeathTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDeathEvent);

                AddModifier(properties);
            }
            /*{//TODOPRIO FIXME
                //Timed damage on kill
                var damageData = new[] { new DamageData(2, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("TimedDamageOnKillTest", LegalTarget.Beings);
                properties.AddConditionData(new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.KillEvent));
                properties.AddEffect(new DamageStatComponent(damageData), damageData);

                properties.SetRemovable(5);

                AddModifier(properties);
            }*/
            {
                //Thorns on hit
                var damageData = new[] { new DamageData(5, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("ThornsOnHitTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnHitEvent);

                AddModifier(properties);
            }
            /*{
                //TODO Implement a generation of the modifier below
                //TODO We might come into trouble with multiple target components, since rn we rely on having only one in modifier
                //Heal on death, once
                var properties = new ModifierGenerationProperties("HealOnDeathTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnDeathEvent);
                properties.AddEffect(new HealComponent(10));


                AddModifier(properties);
            }*/
            {
                //TODO We might come into trouble with multiple target components, since rn we rely on having only one in modifier
                //Heal on death, once
                var modifier = new Modifier("HealOnDeathTest", null, AddModifierParameters.OwnerIsTarget);
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

                AddModifier(properties);
            }
            {
                //Heal yourself on healing someone else
                var properties = new ModifierGenerationProperties("HealOnHealTest", null, LegalTarget.FriendlyBuff);
                properties.AddEffect(new HealStatBasedComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.HealEvent);

                AddModifier(properties);
            }
            {
                //Damage increased per stack dot
                var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("DoTStackTest", null);
                properties.AddEffect(new DamageComponent(damageData, DamageComponent.StackEffectType.Add), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetEffectOnStack(new StackComponentProperties() { Value = 2, MaxStacks = 1000 });
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //Refresh duration DoT
                var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("DoTRefreshTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData, DamageComponent.StackEffectType.Add), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRefreshable(RefreshEffectType.RefreshDuration);
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //Silence on 4 stacks
                var properties = new ModifierGenerationProperties("SilenceXStacksTest", null, LegalTarget.Beings);
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 4, StatusComponent.StackEffectType.Effect));
                properties.SetEffectOnInit();
                properties.SetEffectOnStack(new StackComponentProperties()
                    { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 4 });
                properties.SetRefreshable(RefreshEffectType.RefreshDuration);
                properties.SetRemovable(10);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //Apply a new buff modifier that Stuns, on 3 stacks (effect is an example, it can be much more nuanced than that)
                var stunProperties = new ModifierGenerationProperties("GenericStunModifierTest", null);
                stunProperties.AddEffect(new StatusComponent(StatusEffect.Stun, 2));
                stunProperties.SetEffectOnInit();
                stunProperties.SetRemovable();

                var stunModifier = AddModifier(stunProperties);

                var properties = new ModifierGenerationProperties("ApplyStunModifierXStacksTestApplier", null);
                properties.SetApplier(ApplierType.Attack);
                properties.AddEffect(new ApplierEffectComponent(stunModifier, isStackEffect: true));
                properties.SetEffectOnStack(new StackComponentProperties()
                    { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 3 });

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //Damage on low health
                var damageData = new[] { new DamageData(50, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnLowHealthTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData,
                    new ConditionCheckData(ConditionBeingStatus.HealthIsLow)), damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);

                AddModifier(properties);
            }
            {
                //Flag
                var flagDamageData = new[] { new DamageData(1, DamageType.Physical) };
                var flagProperties = new ModifierGenerationProperties("FlagTest", null, LegalTarget.Beings);
                flagProperties.AddEffect(new DamageStatComponent(flagDamageData), flagDamageData);
                flagProperties.SetEffectOnInit();

                var flagModifier = AddModifier(flagProperties);

                //Damage on modifier id (flag)
                var damageData = new[] { new DamageData(double.MaxValue, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnModifierIdTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData("FlagTest")), damageData);
                properties.SetEffectOnInit();

                AddModifier(properties);
            }
            {
                //If enemy is on fire, deal damage to him, on hit
                var damageData = new[] { new DamageData(10000, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DealDamageOnElementalIntensityTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData,
                    conditionCheckData: new ConditionCheckData(ElementType.Fire, ComparisonCheck.GreaterOrEqual,
                        BaseProject.Curves.ElementIntensity.Evaluate(900))), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.HitEvent);

                AddModifier(properties);
            }
            {
                //TODO CleanUp? Unit test isnt up
                //If health on IsLow, add 50 physical damage, if not, remove 50 physical damage
                var damageData = new[] { new DamageData(50, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnLowHealthRemoveTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData(ConditionBeingStatus.HealthIsLow)),
                    damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);

                AddModifier(properties);
            }
            {
                //On hit, attack yourself
                var properties = new ModifierGenerationProperties("AttackYourselfOnHitTest", null);
                properties.AddEffect(new AttackComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.OnHitEvent);

                AddModifier(properties);
            }
            {
                //On damaged, attack yourself
                var properties = new ModifierGenerationProperties("AttackYourselfOnDamagedTest", null);
                properties.AddEffect(new AttackComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.OnDamagedEvent);

                AddModifier(properties);
            }
            {
                //Hit, heal yourself
                var properties = new ModifierGenerationProperties("HealYourselfHitTest", null);
                properties.AddEffect(new HealStatBasedComponent());
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.HitEvent);

                AddModifier(properties);
            }
            {
                //Reflect back 20% of damage dealt
                var properties = new ModifierGenerationProperties("ReflectOnDamagedTest", null);
                properties.AddEffect(new DamageReflectComponent(0.2));
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDamagedEvent);

                AddModifier(properties);
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
                        DamageComponent.StackEffectType.Effect |
                        DamageComponent.StackEffectType.SetMultiplierStacksBased), damageData);
                flagProperties.SetEffectOnStack(new StackComponentProperties()
                    { WhenStackEffect = WhenStackEffect.Always, MaxStacks = 100 });
                flagProperties.SetRefreshable(RefreshEffectType.RefreshDuration);
                flagProperties.SetRemovable(10);

                var flagModifier = AddModifier(flagProperties);

                var properties = new ApplierModifierGenerationProperties(flagModifier, null, LegalTarget.Beings);
                properties.SetApplier(ApplierType.Attack);

                AddModifier(properties);
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

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                applierProperties.SetCost(PoolStatType.Health, 10);
                AddModifier(applierProperties);
            }

            {
                //IceboltDebuff cast that costs mana to cast
                var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltCastManaCostTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Cast);
                applierProperties.SetCost(PoolStatType.Mana, 10);
                AddModifier(applierProperties);
            }
            {
                //IceboltDebuff attack that costs mana to cast
                var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltAttackManaCostTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                applierProperties.SetCost(PoolStatType.Mana, 10);
                AddModifier(applierProperties);
            }

            {
                //IceboltDebuff that has a cooldown
                var damageData = new[] { new DamageData(2, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltCooldownTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                applierProperties.SetCooldown(5);
                AddModifier(applierProperties);
            }

            {
                //IceboltDebuff that is automatically cast
                var damageData = new[] { new DamageData(2, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltAutomaticCastTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Cast);
                applierProperties.SetCooldown(1);
                applierProperties.SetAutomaticCast();
                AddModifier(applierProperties);
            }

            {
                //ThornsOnHit 0% Chance
                var damageData = new[] { new DamageData(1, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("ThornsOnHitChanceZeroTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnHitEvent);
                properties.SetChance(0);

                AddModifier(properties);
            }
            {
                //IncreaseDmgOnHit 50% Chance
                var damageData = new[] { new DamageData(1, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("IncreaseDmgHitHalfTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.HitEvent);
                properties.SetChance(0.5);

                AddModifier(properties);
            }
            {
                //HealOnHit Chance
                var properties = new ModifierGenerationProperties("HealOnHitHalfChanceTest", null);
                properties.AddEffect(new HealComponent(1));
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);
                properties.SetChance(0.5);

                AddModifier(properties);
            }
            {
                //IncreaseDmgOnHit 100% Chance
                var damageData = new[] { new DamageData(1, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("IncreaseDmgHitFullTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData), damageData);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.HitEvent);
                properties.SetChance(1);

                AddModifier(properties);
            }
            {
                //FireAttack 50% chance to apply, 50% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitHalfTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(0.5);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                applierProperties.SetChance(0.5);

                AddModifier(applierProperties);
            }
            {
                //FireAttack 0% chance to apply, 100% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitFullZeroTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(1);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                applierProperties.SetChance(0);

                AddModifier(applierProperties);
            }
            {
                //FireAttack 100% chance to apply, 0% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitZeroFullTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(0);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                applierProperties.SetChance(1);

                AddModifier(applierProperties);
            }
            {
                //FireAttack 100% chance to apply, 100% chance to "hit"
                var damageData = new[] { new DamageData(1, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireAttackChanceToHitFullFullTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();
                properties.SetChance(1);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                applierProperties.SetChance(1);

                AddModifier(applierProperties);
            }

            {
                //FireBall with init dmg and DoT
                var initDamageData = new[] { new DamageData(10, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var timeDamageData = new[] { new DamageData(3, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireBallTwoEffectTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(initDamageData), initDamageData); //Init
                properties.SetEffectOnInit();
                properties.AddEffect(new DamageComponent(timeDamageData), timeDamageData); //DoT
                properties.SetEffectOnTime(1, true);
                properties.SetRemovable(5);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }
            {
                //FireBall with init dmg and init DoT
                var initDamageData = new[] { new DamageData(10, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var timeDamageData = new[] { new DamageData(3, DamageType.Magical, new ElementData(ElementType.Fire, 20, 10)) };
                var properties = new ModifierGenerationProperties("FireBallTwoEffectInitTest", null, LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(initDamageData), initDamageData); //Init
                properties.SetEffectOnInit();
                properties.AddEffect(new DamageComponent(timeDamageData), timeDamageData); //DoT
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(1, true);
                properties.SetRemovable(5);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Attack);
                AddModifier(applierProperties);
            }

            {
                //Permanent Fire Resistance
                var properties = new ModifierGenerationProperties("ElementFireResistanceTest", null);
                properties.AddEffect(new ElementResistanceComponent(ElementType.Fire, 1000));
                properties.SetEffectOnInit();

                AddModifier(properties);
            }
            {
                //Permanent Physical Resistance
                var properties = new ModifierGenerationProperties("DamagePhysicalResistanceTest", null);
                properties.AddEffect(new DamageResistanceComponent(DamageType.Physical, 1000));
                properties.SetEffectOnInit();

                AddModifier(properties);
            }

            {
                //Separate silence & disarm
                var properties = new ModifierGenerationProperties("SilenceDisarmTwoEffectTest", null, LegalTarget.Beings);
                properties.AddEffect(new StatusComponent(StatusEffect.Disarm, 1f));
                properties.SetEffectOnInit();
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable(2f);

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Cast);
                AddModifier(applierProperties);
            }

            {
                //Temporary resistance buff
                var properties = new ModifierGenerationProperties("TemporaryPhysicalResistanceTest", null, LegalTarget.Self);
                properties.AddEffect(new DamageResistanceComponent(DamageType.Physical, 100, isRevertible: true));
                properties.SetEffectOnInit();
                properties.SetRemovable(1d);

                AddModifier(properties);
            }
            {
                //Temporary damage buff
                var damageData = new[] { new DamageData(10, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("TemporaryDamageBuffTest", null, LegalTarget.Self);
                properties.AddEffect(new DamageStatComponent(damageData, isRevertible: true), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable(10d);

                AddModifier(properties);
            }

            {
                //Taunt
                var properties = new ModifierGenerationProperties("TauntTest", null);
                properties.AddEffect(new StatusComponent(StatusEffect.Taunt, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Cast);
                AddModifier(applierProperties);
            }
            {
                //Basic cold damage cast
                var damageData = new[] { new DamageData(5, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltCastTest", null);
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
                applierProperties.SetApplier(ApplierType.Cast);
                AddModifier(applierProperties);
            }

            {
                //Friendly Physical damage resistance, for aura testing
                var properties = new AuraEffectModifierGenerationProperties("FriendlyPhysicalDamageResistanceAuraTest", null);
                properties.AddEffect(new DamageResistanceComponent(DamageType.Physical, 100, isRevertible: true));

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, LegalTarget.FriendlyBuff);
                applierProperties.SetApplier(ApplierType.Aura);
                AddModifier(applierProperties);
            }
            {
                //Opposite Physical damage resistance, for aura testing
                var properties = new AuraEffectModifierGenerationProperties("OppositePhysicalDamageResistanceAuraTest", null);
                properties.AddEffect(new DamageResistanceComponent(DamageType.Physical, 100, isRevertible: true));

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, LegalTarget.Opposite);
                applierProperties.SetApplier(ApplierType.Aura);
                AddModifier(applierProperties);
            }
            {
                //Everyone Physical damage resistance, for aura testing
                var properties = new AuraEffectModifierGenerationProperties("EveryonePhysicalDamageResistanceAuraTest", null);
                properties.AddEffect(new DamageResistanceComponent(DamageType.Physical, 100, isRevertible: true));

                var modifier = AddModifier(properties);

                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, LegalTarget.Beings);
                applierProperties.SetApplier(ApplierType.Aura);
                AddModifier(applierProperties);
            }
            
            {
                //Temporary allied damage resistance buff
                var properties = new ModifierGenerationProperties("TemporaryAlliedDamageResistanceBuffTest", null, LegalTarget.Self);
                properties.AddEffect(new DamageResistanceComponent(DamageType.Physical, 100, isRevertible: true));
                properties.SetEffectOnInit();
                properties.SetRefreshable();
                properties.SetRemovable(10d);

                var modifier = AddModifier(properties);
                
                var applierProperties = new ApplierModifierGenerationProperties(modifier, null, LegalTarget.Same);
                applierProperties.SetApplier(ApplierType.Cast);
                AddModifier(applierProperties);
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
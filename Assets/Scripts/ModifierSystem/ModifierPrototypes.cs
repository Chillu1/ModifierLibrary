using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;
using Random = System.Random;

namespace ModifierSystem
{
    public class ModifierPrototypes<TModifier> : BasePrototypeController<string, TModifier>
        where TModifier : Modifier, IEventCopy<TModifier>
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

        //Generic non-removable (permanent) applier, for now
        public void SetupModifierApplier(TModifier appliedModifier, ApplierType applierType, LegalTarget legalTarget = LegalTarget.Beings)
        {
            var modifierApplier = new Modifier(appliedModifier.Id + "Applier", applierType);
            var target = new TargetComponent(legalTarget, true);
            var effect = new ApplierEffectComponent(appliedModifier);
            effect.Setup(target);
            var applier = new ApplierComponent(effect);
            modifierApplier.AddComponent(applier);
            modifierApplier.AddComponent(target);
            modifierApplier.FinishSetup(); //"No tags", for now?
            AddModifier((TModifier)modifierApplier);
        }

        [CanBeNull]
        public TModifier Get(string key)
        {
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
            ValidateModifier(modifier);

            return modifier;
        }

        private void SetupTestModifiers()
        {
            {
                //IceboltDebuff
                var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementalType.Cold, 20, 10)) };
                var properties = new ModifierGenerationProperties("IceBoltTest");
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //SpiderPoison
                var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("SpiderPoisonTest");
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //PassiveSelfHeal
                var properties = new ModifierGenerationProperties("PassiveSelfHealTest");
                properties.AddEffect(new HealComponent(10));
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //AllyHealTest
                var properties = new ModifierGenerationProperties("AllyHealTest");
                properties.AddEffect(new HealComponent(10));
                properties.SetEffectOnInit();
                properties.SetRemovable();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                //Forever buff (applier), not refreshable or stackable (for now)
                SetupModifierApplier(modifier, ApplierType.Attack, LegalTarget.DefaultFriendly);
            }
            {
                //BasicPoison
                var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Poison, 20, 20)) };
                var properties = new ModifierGenerationProperties("PoisonTest");
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //BasicBleed
                var damageData = new[] { new DamageData(2, DamageType.Physical, new ElementData(ElementalType.Bleed, 20, 20)) };
                var properties = new ModifierGenerationProperties("BleedTest");
                properties.SetEffectOnInit();
                properties.AddEffect(new DamageComponent(damageData), damageData);
                properties.SetEffectOnTime(2, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //MovementSpeedOfCat
                var properties = new ModifierGenerationProperties("MovementSpeedOfCatTest");
                properties.AddEffect(new StatComponent(new[] { new Stat(StatType.MovementSpeed, 5) }));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //AttackSpeedOfCatTest
                var properties = new ModifierGenerationProperties("AttackSpeedOfCatTest");
                properties.AddEffect(new StatComponent(new[] { new Stat(StatType.AttackSpeed, 5) }));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Disarm
                var properties = new ModifierGenerationProperties("DisarmModifierTest");
                properties.AddEffect(new StatusComponent(StatusEffect.Disarm, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //Cast
                var properties = new ModifierGenerationProperties("SilenceModifierTest");
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 2f));
                properties.SetEffectOnInit();
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //Root timed modifier (enigma Q)
                var properties = new ModifierGenerationProperties("RootTimedModifierTest");
                properties.AddEffect(new StatusComponent(StatusEffect.Root, 0.1f));
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(1, true);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //Delayed silence
                var properties = new ModifierGenerationProperties("DelayedSilenceModifierTest");
                properties.AddEffect(new StatusComponent(StatusEffect.Silence, 1));
                properties.SetEffectOnTime(1, false);
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
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

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //Damage on kill
                var properties = new ModifierGenerationProperties("DamageOnKillTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.KillEvent);
                properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Plain add damage permanently
                var properties = new ModifierGenerationProperties("AddStatDamageTest");
                properties.AddEffect(new DamageStatComponent(new[] { new DamageData(2, DamageType.Physical) }));
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Damage on death
                var damageData = new[] { new DamageData(double.MaxValue, DamageType.Magical) };
                var properties = new ModifierGenerationProperties("DamageOnDeathTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDeathEvent);
                properties.AddEffect(new DamageComponent(damageData), damageData);

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
                var properties = new ModifierGenerationProperties("ThornsOnHitTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnHitEvent);
                properties.AddEffect(new DamageComponent(damageData), damageData);

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
                var modifier = new Modifier("HealOnDeathTest");
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
                var properties = new ModifierGenerationProperties("HealStatBasedTest", LegalTarget.Beings);
                properties.AddEffect(new HealStatBasedComponent());
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Heal yourself on healing someone else
                var properties = new ModifierGenerationProperties("HealOnHealTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.HealEvent);
                properties.AddEffect(new HealStatBasedComponent());

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Damage increased per stack dot
                var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
                var properties = new ModifierGenerationProperties("DoTStackTest", LegalTarget.Beings);
                properties.AddEffect(new DamageComponent(damageData, DamageComponent.DamageComponentStackEffect.Add), damageData);
                properties.SetEffectOnInit();
                properties.SetEffectOnTime(2, true);
                properties.SetEffectOnStack(new StackComponentProperties() { Value = 2, MaxStacks = 1000 });
                properties.SetRemovable(10);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
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

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
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

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //Apply a new modifier that Stuns, on 3 stacks (effect is an example, it can be much more nuanced than that)
                var stunProperties = new ModifierGenerationProperties("GenericStunModifierTest");
                stunProperties.AddEffect(new StatusComponent(StatusEffect.Stun, 2));
                stunProperties.SetEffectOnInit();
                stunProperties.SetRemovable();

                var stunModifier = (TModifier)ModifierGenerator.GenerateModifier(stunProperties);
                AddModifier(stunModifier);


                var properties = new ModifierGenerationProperties("ApplyStunModifierXStacksTestApplier");
                properties.SetApplier(ApplierType.Attack);
                properties.AddEffect(new ApplierEffectComponent(stunModifier, isStackEffect: true));
                properties.SetEffectOnStack(new StackComponentProperties()
                    { WhenStackEffect = WhenStackEffect.EveryXStacks, OnXStacks = 3 });

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
                SetupModifierApplier(modifier, ApplierType.Attack);
            }
            {
                //Damage on low health
                var damageData = new[] { new DamageData(50, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnLowHealthTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);
                properties.AddEffect(new DamageStatComponent(damageData,
                    new ConditionCheckData(ConditionBeingStatus.HealthIsLow)), damageData);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Flag
                var flagDamageData = new[] { new DamageData(1, DamageType.Physical) };
                var flagProperties = new ModifierGenerationProperties("FlagTest", LegalTarget.Beings);
                flagProperties.AddEffect(new DamageStatComponent(flagDamageData), flagDamageData);
                flagProperties.SetEffectOnInit();

                var flagModifier = (TModifier)ModifierGenerator.GenerateModifier(flagProperties);
                AddModifier(flagModifier);

                //Damage on modifier id (flag)
                var damageData = new[] { new DamageData(double.MaxValue, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnModifierIdTest", LegalTarget.Beings);
                properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData("FlagTest")), damageData);
                properties.SetEffectOnInit();

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //If enemy is on fire, deal damage to him, on hit
                var damageData = new[] { new DamageData(10000, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DealDamageOnElementalIntensityTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.HitEvent);
                properties.AddEffect(new DamageComponent(damageData,
                    conditionCheckData: new ConditionCheckData(ElementalType.Fire, ComparisonCheck.GreaterOrEqual,
                        Curves.ElementIntensity.Evaluate(900))), damageData);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //TODO CleanUp? Unit test isnt up
                //If health on IsLow, add 50 physical damage, if not, remove 50 physical damage
                var damageData = new[] { new DamageData(50, DamageType.Physical) };
                var properties = new ModifierGenerationProperties("DamageOnLowHealthRemoveTest", LegalTarget.Beings);
                properties.AddConditionData(ConditionEventTarget.SelfActer, ConditionEvent.OnHitEvent);
                properties.AddEffect(new DamageStatComponent(damageData, new ConditionCheckData(ConditionBeingStatus.HealthIsLow)),
                    damageData);

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //On hit, attack yourself
                var properties = new ModifierGenerationProperties("AttackYourselfOnHitTest");
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.OnHitEvent);
                properties.AddEffect(new AttackComponent());

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //On damaged, attack yourself
                var properties = new ModifierGenerationProperties("AttackYourselfOnDamagedTest");
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.OnDamagedEvent);
                properties.AddEffect(new AttackComponent());

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Hit, heal yourself
                var properties = new ModifierGenerationProperties("HealYourselfHitTest");
                properties.AddConditionData(ConditionEventTarget.SelfSelf, ConditionEvent.HitEvent);
                properties.AddEffect(new HealStatBasedComponent());

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //Reflect back 20% of damage dealt
                var properties = new ModifierGenerationProperties("ReflectOnDamagedTest");
                properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDamagedEvent);
                properties.AddEffect(new DamageReflectComponent(0.2));

                var modifier = (TModifier)ModifierGenerator.GenerateModifier(properties);
                AddModifier(modifier);
            }
            {
                //More fixed damage per stack (on target being, probably it's own debuff modifier?) (Ursa E)
                //Workings: Attack enemy being, add timed (10s) modifier with 1 stack to enemy being.
                //  Next time we attack, we check for that modifier, call it's effect, add +1 stack, deal X damage * Y stacks

                //Stack flag modifier
                var damageData = new[] { new DamageData(2, DamageType.Physical) };
                var flagProperties = new ModifierGenerationProperties("DamagePerStackTest");
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


                var properties = new ModifierGenerationProperties("DamagePerStackTestApplier", LegalTarget.Beings);
                properties.SetApplier(ApplierType.Attack);
                properties.AddConditionData(ConditionEventTarget.ActerSelf,
                    ConditionEvent.HitEvent); //This is optional, we can always apply on attack instead
                properties.AddEffect(new ApplierEffectComponent(flagModifier));

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
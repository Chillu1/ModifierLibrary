using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Modifier templates to copy paste and change, for faster modifier creation
    /// </summary>
    internal class TemplateModifiers
    {
        private readonly ModifierPrototypes<Modifier> _modifierPrototypes;

        /// <summary>
        ///     Absolute Full Modifier Setup Template, every value and component used. ConditionDoTStackRefresh
        /// </summary>
        private void FullModifier()
        {
            var modifier = new Modifier("FullTest", null, AddModifierParameters.OwnerIsTarget);
            var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
            var conditionData = new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.AttackEvent);
            var target = new TargetComponent(LegalTarget.Beings, conditionData.conditionEventTarget);
            var effect = new DamageComponent(damageData, DamageComponent.DamageComponentStackEffect.Add);
            effect.Setup(target);
            var check = new CheckComponent(new IEffectComponent[] { effect });
            var apply = new ConditionalApplyComponent(effect, target, conditionData.conditionEvent);
            var cleanUp = new CleanUpComponent(apply);
            var remove = new RemoveComponent(modifier, cleanUp);
            var timeRemove = new TimeComponent(remove, 10);
            var applyRemoval = new ConditionalApplyComponent(remove, target, conditionData.conditionEvent);
            modifier.AddComponent(target);
            modifier.AddComponent(timeRemove);
            modifier.AddComponent(new InitComponent(apply, applyRemoval));
            modifier.AddComponent(new TimeComponent(check, 2, true));
            modifier.AddComponent(new StackComponent(effect, new StackComponentProperties() { Value = 5 }));
            modifier.AddComponent(new RefreshComponent(timeRemove, RefreshEffectType.RefreshDuration));
            modifier.FinishSetup(damageData);
            _modifierPrototypes.AddModifier(modifier);

            var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
            applierProperties.SetApplier(ApplierType.Attack);
            _modifierPrototypes.AddModifier(applierProperties);
        }

        /// <summary>
        ///     Simple DoT Modifier
        /// </summary>
        private void DoTModifier()
        {
            //BasicPoison
            var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
            var properties = new ModifierGenerationProperties("DoTTest", null);
            properties.AddEffect(new DamageComponent(damageData), damageData);
            properties.SetEffectOnInit();
            properties.SetEffectOnTime(2, true);
            properties.SetRemovable(10);

            var modifier = _modifierPrototypes.AddModifier(properties);

            var applierProperties = new ApplierModifierGenerationProperties(modifier, null);
            applierProperties.SetApplier(ApplierType.Attack);
            _modifierPrototypes.AddModifier(applierProperties);
        }

        /// <summary>
        ///     Simple condition modifier, damage on death
        /// </summary>
        private void ConditionModifier()
        {
            var damageData = new[] { new DamageData(double.MaxValue, DamageType.Magical) };
            var properties = new ModifierGenerationProperties("DamageOnDeathTest", null, LegalTarget.Beings);
            properties.AddEffect(new DamageComponent(damageData), damageData);
            properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDeathEvent);

            _modifierPrototypes.AddModifier(properties);
        }

        /// <summary>
        ///     Hit, if enemy has X intensity, deal 10000 damage
        /// </summary>
        private void ConditionConditionCheckModifier()
        {
            //If enemy is on fire, deal damage to him, on hit
            var damageData = new[] { new DamageData(10000, DamageType.Physical) };
            var properties = new ModifierGenerationProperties("DealDamageOnElementalIntensityTest", null, LegalTarget.Beings);
            properties.AddEffect(new DamageComponent(damageData,
                conditionCheckData: new ConditionCheckData(ElementType.Fire, ComparisonCheck.GreaterOrEqual,
                    BaseProject.Curves.ElementIntensity.Evaluate(900))), damageData);
            properties.AddConditionData(ConditionEventTarget.ActerSelf, ConditionEvent.HitEvent);

            _modifierPrototypes.AddModifier(properties);
        }
    }
}
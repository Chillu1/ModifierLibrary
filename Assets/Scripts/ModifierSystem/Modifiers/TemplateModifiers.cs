using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Modifier templates to copy paste and change, for faster modifier creation
    /// </summary>
    internal class TemplateModifiers
    {
        private readonly ModifierPrototypesBase<IModifier> _modifierPrototypes;

        /// <summary>
        ///     Absolute Full Modifier Setup Template, every value and component used. ConditionDoTStackRefresh
        /// </summary>
        private void FullModifier()
        {
            var modifier = new Modifier("Full");
            var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
            var conditionData = new ConditionEventData(ConditionEventTarget.SelfActer, ConditionEvent.AttackEvent);
            var target = new TargetComponent(LegalTarget.Beings, conditionData);
            var effect = new DamageComponent(damageData, target, DamageComponent.DamageComponentStackEffect.Add);
            var apply = new ApplyComponent(effect, target, conditionData);
            var cleanUp = new CleanUpComponent(apply);
            var remove = new RemoveComponent(modifier, cleanUp);
            var timeRemove = new TimeComponent(remove, 10);
            var applyRemoval = new ApplyComponent(remove, target, conditionData);
            modifier.AddComponent(target);
            modifier.AddComponent(timeRemove);
            modifier.AddComponent(new InitComponent(apply, applyRemoval));
            modifier.AddComponent(new TimeComponent(effect, 2, true));
            modifier.AddComponent(new StackComponent(new StackComponentProperties(effect) { Value = 5 }));
            modifier.AddComponent(new RefreshComponent(timeRemove, RefreshEffectType.RefreshDuration));
            modifier.FinishSetup(damageData);
            _modifierPrototypes.AddModifier(modifier);
            _modifierPrototypes.SetupModifierApplier(modifier);
        }

        /// <summary>
        ///     Simple DoT Modifier
        /// </summary>
        private void DoTModifier()
        {
            var modifier = new Modifier("DoT");
            var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
            var target = new TargetComponent();
            var effect = new DamageComponent(damageData, target);
            var timeRemove = new TimeComponent(new RemoveComponent(modifier), 10);
            modifier.AddComponent(target);
            modifier.AddComponent(timeRemove);
            modifier.AddComponent(new InitComponent(effect));
            modifier.AddComponent(new TimeComponent(effect, 2, true));
            modifier.FinishSetup(damageData);
            _modifierPrototypes.AddModifier(modifier);
            _modifierPrototypes.SetupModifierApplier(modifier);
        }

        /// <summary>
        ///     Simple condition modifier, damage on death
        /// </summary>
        private void ConditionModifier()
        {
            var modifier = new Modifier("DamageOnDeath");
            var conditionData = new ConditionEventData(ConditionEventTarget.ActerSelf, ConditionEvent.OnDeathEvent);
            var target = new TargetComponent(LegalTarget.Beings, conditionData);
            var effect = new DamageComponent(new []{new DamageData(double.MaxValue, DamageType.Magical)}, target);
            var apply = new ApplyComponent(effect, target, conditionData);
            modifier.AddComponent(target);
            modifier.AddComponent(new InitComponent(apply));
            modifier.FinishSetup();
            _modifierPrototypes.AddModifier(modifier);
        }

        /// <summary>
        ///     Hit, if enemy has X intensity, deal 10000 damage
        /// </summary>
        private void ConditionConditionCheckModifier()
        {
            //If enemy is on fire, deal damage to him, on hit
            var modifier = new Modifier("DealDamageOnElementalIntensityTest");
            var conditionData = new ConditionEventData(ConditionEventTarget.ActerSelf, ConditionEvent.HitEvent);
            //We check intensity on acter?
            var target = new TargetComponent(LegalTarget.Beings, conditionData);
            var effect = new DamageComponent(new[] { new DamageData(10000, DamageType.Physical) }, target,
                conditionCheckData: new ConditionCheckData(ElementalType.Fire, ComparisonCheck.GreaterOrEqual,
                    Curves.ElementIntensity.Evaluate(900)));
            var apply = new ApplyComponent(effect, target, conditionData);
            modifier.AddComponent(target);
            modifier.AddComponent(new InitComponent(apply));
            modifier.FinishSetup();
            _modifierPrototypes.AddModifier(modifier);
        }
    }
}
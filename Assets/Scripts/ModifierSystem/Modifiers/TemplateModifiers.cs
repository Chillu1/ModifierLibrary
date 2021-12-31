using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Modifier templates to copy paste and change, for faster modifier creation
    /// </summary>
    internal class TemplateModifiers
    {
        private readonly ModifierPrototypesBase<Modifier> _modifierPrototypes;

        /// <summary>
        ///     Absolute Full Modifier Setup Template, every value and component used. DoTStackRefresh
        /// </summary>
        private void FullModifier()
        {
            var modifier = new Modifier("Full");
            var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
            var conditionData = new ConditionData(ConditionTarget.Self, BeingConditionEvent.AttackEvent);
            var target = new TargetComponent(LegalTarget.Beings, conditionData);
            var effect = new DamageComponent(damageData, target, DamageComponent.DamageComponentStackEffect.Add);
            var apply = new ApplyComponent(effect, target, conditionData);
            var cleanUp = new CleanUpComponent(apply);
            var remove = new RemoveComponent(modifier, cleanUp);
            var timeRemove = new TimeComponent(remove, 10);
            modifier.AddComponent(target);
            modifier.AddComponent(timeRemove);
            modifier.AddComponent(new InitComponent(apply));
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
    }
}
namespace ModifierSystem
{
    public static class ModifierGenerator
    {
        public static ComboModifier GenerateComboModifier(ModifierGenerationProperties properties)
        {
            return (ComboModifier)GenerateModifier(properties);
        }

        public static Modifier GenerateModifier(ModifierGenerationProperties properties)
        {
            Modifier modifier;
            modifier = properties is ComboModifierGenerationProperties comboProperties
                ? new ComboModifier(comboProperties.Name, comboProperties.Recipes, comboProperties.Cooldown)
                : new Modifier(properties.Name, properties.Applier, properties.HasConditionData);

            //---Components---

            var targetComponent = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionEventTarget, properties.Applier)
                : new TargetComponent(properties.LegalTarget, properties.Applier);

            //---Effect---
            EffectComponent effectComponent = properties.EffectComponent;
            effectComponent.Setup(targetComponent);

            //---Apply---?

            //TODO, Specify if effect or remove component should be added to applyComponent
            //---Conditional Apply---
            ConditionalApplyComponent conditionalApplyComponent = null;
            if (properties.HasConditionData)
                conditionalApplyComponent = new ConditionalApplyComponent(effectComponent, targetComponent, properties.ConditionEvent);

            //---CleanUp---
            //TODO ApplyComponent in cleanup
            CleanUpComponent cleanUpComponent = null;
            if (properties.Removable && properties.EffectOn.HasFlag(EffectOn.Apply))
                cleanUpComponent = new CleanUpComponent(conditionalApplyComponent);

            //---Remove---
            //---TimeRemove--- //TODO & ConditionalApplyComponent(remove)
            TimeComponent removeTimeComponent = null;
            if (properties.Removable)
            {
                RemoveComponent removeComponent = new RemoveComponent(modifier, cleanUpComponent);
                removeTimeComponent = new TimeComponent(removeComponent, properties.RemoveDuration);
            }


            //---Finish Setup---
            //Add all components to modifier
            modifier.AddComponent(targetComponent);

            if (properties.Applier && effectComponent is ApplierEffectComponent applier)
            {
                var applierComponent = new ApplierComponent(applier);
                modifier.AddComponent(applierComponent);
            }

            if (removeTimeComponent != null)
                modifier.AddComponent(removeTimeComponent);

            //Make rest of the component that won't be used by anything else
            if (properties.EffectOn.HasFlag(EffectOn.Init))
                modifier.AddComponent(new InitComponent(effectComponent));
            if (properties.EffectOn.HasFlag(EffectOn.Apply) && conditionalApplyComponent != null)
                modifier.AddComponent(new InitComponent(conditionalApplyComponent));
            if (properties.EffectOn.HasFlag(EffectOn.Time))
                modifier.AddComponent(new TimeComponent(effectComponent, properties.EffectDuration, properties.ResetOnFinished));

            //---Stack---
            if (effectComponent is IStackEffectComponent stackEffectComponent && properties.StackComponentProperties != null)
                modifier.AddComponent(new StackComponent(stackEffectComponent, properties.StackComponentProperties));

            //---Refresh---
            if (properties.RefreshEffectType != RefreshEffectType.None)
                modifier.AddComponent(new RefreshComponent(removeTimeComponent, properties.RefreshEffectType));

            modifier.FinishSetup(properties.DamageData);
            //Debug.Log(modifier.GetType() + "_ " + typeof(TModifier));
            return modifier;
        }
    }
}
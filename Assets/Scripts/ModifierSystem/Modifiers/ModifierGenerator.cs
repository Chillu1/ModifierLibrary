using UnityEngine;

namespace ModifierSystem
{
    public static class ModifierGenerator
    {
        public static ComboModifier GenerateComboModifier(ComboModifierGenerationProperties properties)
        {
            return (ComboModifier)GenerateModifier(properties);
        }

        public static Modifier GenerateModifier(ModifierGenerationProperties properties)
        {
            Modifier modifier;
            modifier = properties is ComboModifierGenerationProperties comboProperties
                ? new ComboModifier(comboProperties.Name, comboProperties.Recipes, comboProperties.Cooldown)
                : new Modifier(properties.Name, properties.ApplierType, properties.HasConditionData);

            //---Components---

            var targetComponent = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionEventTarget, properties.IsApplier)
                : new TargetComponent(properties.LegalTarget, properties.IsApplier);

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

            if (properties.IsApplier && effectComponent is ApplierEffectComponent applier)
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

            //---Cost---
            if (properties.Cost.Item1 != CostType.None && properties.Cost.Item2 != 0)
                modifier.AddComponent(new CostComponent(properties.Cost.Item1, properties.Cost.Item2));

            modifier.FinishSetup(properties.DamageData);
            //Debug.Log(modifier.GetType() + "_ " + typeof(TModifier));
            return modifier;
        }

        public static Modifier GenerateApplierModifier(ApplierModifierGenerationProperties properties)
        {
            var modifier = new Modifier(properties.AppliedModifier.Id + "Applier", properties.ApplierType);

            var target = new TargetComponent(properties.LegalTarget, true);
            var effect = new ApplierEffectComponent(properties.AppliedModifier);
            effect.Setup(target);
            var applier = new ApplierComponent(effect);
            modifier.AddComponent(applier);
            modifier.AddComponent(target);

            if (properties.CostType != CostType.None && properties.CostAmount != 0)
                modifier.AddComponent(new CostComponent(properties.CostType, properties.CostAmount));
            if (properties.Cooldown != -1)
                modifier.AddComponent(new CooldownComponent(properties.Cooldown));

            if (properties.AutomaticCast)
                modifier.SetAutomaticCast();

            modifier.FinishSetup(); //"No tags", for now?

            return modifier;
        }
    }
}
using BaseProject;
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
            ValidateProperties(properties);

            Modifier modifier;
            modifier = properties is ComboModifierGenerationProperties comboProperties
                ? new ComboModifier(comboProperties.Id, properties.Info, comboProperties.Recipes, comboProperties.Cooldown,
                    comboProperties.Effect)
                : new Modifier(properties.Id, properties.Info, properties.ApplierType, properties.HasConditionData);

            //---Components---

            var targetComponent = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionEventTarget, properties.IsApplier)
                : new TargetComponent(properties.LegalTarget, properties.IsApplier);

            //---Effect---
            EffectComponent effectComponent = properties.EffectPropertyInfo[0].EffectComponent;
            effectComponent.Setup(targetComponent);

            EffectComponent effectComponentTwo = null;
            if (properties.EffectPropertyInfo.Count > 1)
            {
                effectComponentTwo = properties.EffectPropertyInfo[1].EffectComponent;
                effectComponentTwo.Setup(targetComponent);
            }

            //---Check---
            CheckComponent checkComponent = effectComponentTwo == null
                ? new CheckComponent(effectComponent
                    , properties.Cost.Item1 != CostType.None ? new CostComponent(properties.Cost.Item1, properties.Cost.Item2) : null
                    , null
                    , properties.Chance != -1 ? new ChanceComponent(properties.Chance) : null)
                : new CheckComponent(new IEffectComponent[] { effectComponent, effectComponentTwo }
                    , properties.Cost.Item1 != CostType.None ? new CostComponent(properties.Cost.Item1, properties.Cost.Item2) : null
                    , null
                    , properties.Chance != -1 ? new ChanceComponent(properties.Chance) : null);

            //---Apply---?

            //TODO, Specify if effect or remove component should be added to applyComponent
            //---Conditional Apply---
            ConditionalApplyComponent conditionalApplyComponent = null;
            if (properties.HasConditionData)
                conditionalApplyComponent =
                    new ConditionalApplyComponent(effectComponent, targetComponent, properties.ConditionEvent, checkComponent);

            //---CleanUp---
            //TODO ApplyComponent in cleanup
            CleanUpComponent cleanUpComponent = null;
            if (properties.Removable && properties.EffectPropertyInfo[0].EffectOn.HasFlag(EffectOn.Apply))
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
            modifier.AddComponent(checkComponent);

            if (properties.IsApplier && effectComponent is ApplierEffectComponent applier)
            {
                var applierComponent = new ApplierComponent(checkComponent);
                modifier.AddComponent(applierComponent);
            }

            if (removeTimeComponent != null)
                modifier.AddComponent(removeTimeComponent);

            //Make rest of the component that won't be used by anything else
            SetupEffectOn(modifier, properties.EffectPropertyInfo[0], checkComponent, conditionalApplyComponent);
            if(effectComponentTwo != null)
                SetupEffectOn(modifier, properties.EffectPropertyInfo[1], checkComponent, conditionalApplyComponent);

            //---Stack---
            if (effectComponent is IStackEffectComponent stackEffectComponent && properties.StackComponentProperties != null)
                modifier.AddComponent(new StackComponent(stackEffectComponent, properties.StackComponentProperties));

            //---Refresh---
            if (properties.RefreshEffectType != RefreshEffectType.None)
                modifier.AddComponent(new RefreshComponent(removeTimeComponent, properties.RefreshEffectType));

            modifier.FinishSetup(properties.DamageData);
            modifier.AddProperties(properties);
            //Debug.Log(modifier.GetType() + "_ " + typeof(TModifier));
            return modifier;
        }

        public static Modifier GenerateApplierModifier(ApplierModifierGenerationProperties properties)
        {
            var modifier = new Modifier(properties.AppliedModifier.Id + "Applier", properties.Info, properties.ApplierType,
                properties.HasConditionData);

            var target = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionEventTarget, properties.IsApplier)
                : new TargetComponent(properties.LegalTarget, properties.IsApplier);

            var effect = new ApplierEffectComponent(properties.AppliedModifier, properties.AddModifierParameters);
            effect.Setup(target);
            var check = new CheckComponent(effect
                , properties.CostType != CostType.None ? new CostComponent(properties.CostType, properties.CostAmount) : null
                , properties.Cooldown != -1 ? new CooldownComponent(properties.Cooldown) : null
                , properties.Chance != -1 ? new ChanceComponent(properties.Chance) : null);
            var applier = new ApplierComponent(check);

            ConditionalApplyComponent conditionalApplyComponent = null;
            if (properties.HasConditionData)
                conditionalApplyComponent = new ConditionalApplyComponent(effect, target, properties.ConditionEvent, check);

            //TODO CleanUp

            var propertyInfo = new EffectPropertyInfo(effect);
            propertyInfo.SetEffectOnApply();
            SetupEffectOn(modifier, propertyInfo, check, conditionalApplyComponent);

            modifier.AddComponent(target);
            modifier.AddComponent(check);
            modifier.AddComponent(applier);

            if (properties.AutomaticCast)
                modifier.SetAutomaticCast();

            modifier.FinishSetup(); //"No tags", for now?
            modifier.AddProperties(properties);

            return modifier;
        }

        private static void ValidateProperties(ModifierGenerationProperties properties)
        {
            if (properties.EffectPropertyInfo[0].EffectOn == EffectOn.None && !properties.HasConditionData && properties.StackComponentProperties == null)
                Log.Error("Properties have to have one of: EffectOn, ConditionData, StackComponentProperties");
        }

        private static void SetupEffectOn(Modifier modifier, EffectPropertyInfo propertyInfo, CheckComponent checkComponent,
            ConditionalApplyComponent conditionalApplyComponent)
        {
            if (propertyInfo.EffectOn.HasFlag(EffectOn.Init))
                modifier.AddComponent(new InitComponent(checkComponent));
            if (propertyInfo.EffectOn.HasFlag(EffectOn.Apply) && conditionalApplyComponent != null)
                modifier.AddComponent(new InitComponent(conditionalApplyComponent));
            if (propertyInfo.EffectOn.HasFlag(EffectOn.Time))
                modifier.AddComponent(new TimeComponent(checkComponent, propertyInfo.EffectDuration, propertyInfo.ResetOnFinished));
        }
    }
}
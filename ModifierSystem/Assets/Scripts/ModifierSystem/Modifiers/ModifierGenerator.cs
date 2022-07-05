using System;
using System.Collections.Generic;
using System.Linq;
using BaseProject;
using Force.DeepCloner;
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
                : new Modifier(properties.Id, properties.Info, properties.AddModifierParameters, properties.ApplierType,
                    properties.HasConditionData);

            //---Components---

            var targetComponent = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionEventTarget, properties.IsApplier)
                : new TargetComponent(properties.LegalTarget, properties.IsApplier);

            //---Effect---
            //TODO We have to deep clone effect so it can be reused without multiple targets holding the same effect, and overwriting each other
            //  Not ideal, best to have some sort of data-only in Properties, and then make a new effect class here
            EffectPropertyInfo effectPropertyInfo = properties.EffectPropertyInfo[0].DeepClone();
            effectPropertyInfo.EffectComponent.Setup(targetComponent);

            EffectPropertyInfo effectPropertyInfoTwo = null;
            if (properties.EffectPropertyInfo.Count > 1)
            {
                effectPropertyInfoTwo = properties.EffectPropertyInfo[1].DeepClone();
                effectPropertyInfoTwo.EffectComponent.Setup(targetComponent);
            }

            //---Check---
            var effects = new List<IEffectComponent>();
            var timeEffects = new List<IEffectComponent>();
            if (effectPropertyInfo.EffectOn.HasFlag(EffectOn.Time))
                timeEffects.Add(effectPropertyInfo.EffectComponent);
            if (effectPropertyInfo.EffectOn.HasFlag(EffectOn.Init) || effectPropertyInfo.EffectOn.HasFlag(EffectOn.Apply))
                effects.Add(effectPropertyInfo.EffectComponent);
            if (effectPropertyInfoTwo != null)
            {
                if (effectPropertyInfoTwo.EffectOn.HasFlag(EffectOn.Time))
                    timeEffects.Add(effectPropertyInfoTwo.EffectComponent);
                if (effectPropertyInfoTwo.EffectOn.HasFlag(EffectOn.Init) || effectPropertyInfoTwo.EffectOn.HasFlag(EffectOn.Apply))
                    effects.Add(effectPropertyInfoTwo.EffectComponent);
            }

            CheckComponent checkComponent = new CheckComponent(
                effects.ToArray(),
                timeEffects.ToArray(),
                properties.Cost.Item1 != CostType.None ? new CostComponent(properties.Cost.Item1, properties.Cost.Item2) : null,
                null,
                properties.Chance != -1 ? new ChanceComponent(properties.Chance) : null);

            //---Apply---?

            //TODO, Specify if effect or remove component should be added to applyComponent
            //---Conditional Apply---
            ConditionalApplyComponent conditionalApplyComponent = null;
            if (properties.HasConditionData)
                conditionalApplyComponent =
                    new ConditionalApplyComponent(effectPropertyInfo.EffectComponent, targetComponent, properties.ConditionEvent,
                        checkComponent);

            //---CleanUp---
            //TODO ApplyComponent in cleanup
            CleanUpComponent cleanUpComponent = null;
            if (properties.Removable && (effectPropertyInfo.EffectOn.HasFlag(EffectOn.Apply) ||
                                         effectPropertyInfo.EffectOn.HasFlag(EffectOn.Init)))
                cleanUpComponent =
                    new CleanUpComponent(conditionalApplyComponent,
                        effects.Cast<EffectComponent>().ToArray()); //EffectTime can't be remove based, for now?

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

            if (properties.IsApplier && effectPropertyInfo.EffectComponent is ApplierEffectComponent applier)
            {
                var applierComponent = new ApplierComponent(checkComponent);
                modifier.AddComponent(applierComponent);
            }

            if (removeTimeComponent != null)
                modifier.AddComponent(removeTimeComponent);

            //Make rest of the component that won't be used by anything else
            SetupEffectOn(modifier, effectPropertyInfo, checkComponent, conditionalApplyComponent);
            if (effectPropertyInfoTwo != null)
                SetupEffectOn(modifier, effectPropertyInfoTwo, checkComponent, conditionalApplyComponent);

            //---Stack---
            if (effectPropertyInfo.EffectComponent is IStackEffectComponent stackEffectComponent &&
                properties.StackComponentProperties != null)
                modifier.AddComponent(new StackComponent(stackEffectComponent, properties.StackComponentProperties.DeepClone()));

            //---Refresh---
            if (properties.RefreshEffectType != RefreshEffectType.None)
                modifier.AddComponent(new RefreshComponent(removeTimeComponent, properties.RefreshEffectType));

            modifier.FinishSetup(properties.DamageData.DeepClone());
            modifier.AddProperties(properties);
            //Debug.Log(modifier.GetType() + "_ " + typeof(TModifier));
            return modifier;
        }

        public static Modifier GenerateApplierModifier(ApplierModifierGenerationProperties properties)
        {
            var modifier = new Modifier(properties.AppliedModifier.Id + "Applier", properties.Info, properties.AddModifierParameters,
                properties.ApplierType, properties.HasConditionData);

            var target = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionEventTarget, properties.IsApplier)
                : new TargetComponent(properties.LegalTarget, properties.IsApplier);

            var effect = new ApplierEffectComponent(properties.AppliedModifier);
            effect.Setup(target);
            var check = new CheckComponent(
                new IEffectComponent[] { effect },
                null, //new IEffectComponent[] { effect },
                properties.CostType != CostType.None ? new CostComponent(properties.CostType, properties.CostAmount) : null,
                properties.Cooldown != -1 ? new CooldownComponent(properties.Cooldown) : null,
                properties.Chance != -1 ? new ChanceComponent(properties.Chance) : null);
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
            if (properties.EffectPropertyInfo[0].EffectOn == EffectOn.None && !properties.HasConditionData &&
                properties.StackComponentProperties == null)
                Log.Error("Properties have to have one of: EffectOn, ConditionData, StackComponentProperties");
        }

        private static void SetupEffectOn(Modifier modifier, EffectPropertyInfo propertyInfo, CheckComponent checkComponent,
            ConditionalApplyComponent conditionalApplyComponent)
        {
            if (propertyInfo.EffectOn.HasFlag(EffectOn.Init))
                modifier.AddComponent(new InitComponent(checkComponent)); //Gives a warning on multiple init components
            if (propertyInfo.EffectOn.HasFlag(EffectOn.Apply) && conditionalApplyComponent != null)
                modifier.AddComponent(new InitComponent(conditionalApplyComponent));
            if (propertyInfo.EffectOn.HasFlag(EffectOn.Time))
                modifier.AddComponent(new TimeComponent(checkComponent, propertyInfo.EffectDuration, propertyInfo.ResetOnFinished));
        }
    }
}
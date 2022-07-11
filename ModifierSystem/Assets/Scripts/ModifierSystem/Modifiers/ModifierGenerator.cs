using System.Collections.Generic;
using System.Linq;
using BaseProject;
using Force.DeepCloner;

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

            Modifier modifier = properties is ComboModifierGenerationProperties comboProperties
                ? new ComboModifier(comboProperties.Id, properties.Info, comboProperties.Recipes, comboProperties.Cooldown,
                    comboProperties.Effect)
                : new Modifier(properties.Id, properties.Info, properties.AddModifierParameters, properties.ApplierType,
                    properties.HasConditionData);

            //---Components---
            TargetComponent targetComponent = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionEventTarget, properties.IsApplier)
                : new TargetComponent(properties.LegalTarget, properties.IsApplier);

            //---Effect---
            //We have to deep clone effect so it can be reused without multiple targets holding the same effect, and overwriting each other, Hopefully TEMP
            EffectPropertyInfo[] effectPropertyInfo = new EffectPropertyInfo[properties.EffectPropertyInfo.Count];
            for (int i = 0; i < properties.EffectPropertyInfo.Count; i++)
                effectPropertyInfo[i] = properties.EffectPropertyInfo[i].DeepClone();

            foreach (var propertyInfo in effectPropertyInfo)
                propertyInfo.EffectComponent.Setup(targetComponent);

            //---Check---
            var effects = new List<IEffectComponent>();
            var timeEffects = new List<IEffectComponent>();
            foreach (var propertyInfo in effectPropertyInfo)
            {
                if (propertyInfo.EffectOn.HasFlag(EffectOn.Time))
                    timeEffects.Add(propertyInfo.EffectComponent);
                if (propertyInfo.EffectOn.HasFlag(EffectOn.Init) || propertyInfo.EffectOn.HasFlag(EffectOn.Apply))
                    effects.Add(propertyInfo.EffectComponent);
            }

            CheckComponent checkComponent = new CheckComponent(
                effects.ToArray(),
                timeEffects.ToArray(),
                properties.Cost.Type != PoolStatType.None ? new CostComponent(properties.Cost.Type, properties.Cost.Value) : null,
                null,
                properties.Chance != -1 ? new ChanceComponent(properties.Chance) : null);

            //---Apply---?

            //TODO, Specify if effect or remove component should be added to applyComponent
            //---Conditional Apply---
            ConditionalApplyComponent conditionalApplyComponent = null;
            if (properties.HasConditionData)
                conditionalApplyComponent = new ConditionalApplyComponent(effectPropertyInfo[0].EffectComponent,
                    targetComponent, properties.ConditionEvent, checkComponent);

            //---CleanUp---
            //TODO ApplyComponent in cleanup
            CleanUpComponent cleanUpComponent = null;
            if (properties.Removable && (effectPropertyInfo[0].EffectOn.HasFlag(EffectOn.Apply) ||
                                         effectPropertyInfo[0].EffectOn.HasFlag(EffectOn.Init)))
            {
                //EffectTime can't be remove based, for now?
                cleanUpComponent = new CleanUpComponent(conditionalApplyComponent, effects.Cast<EffectComponent>().ToArray());
            }

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

            if (properties.IsApplier && effectPropertyInfo[0].EffectComponent is ApplierEffectComponent applier)
            {
                var applierComponent = new ApplierComponent(checkComponent);
                modifier.AddComponent(applierComponent);
            }

            if (removeTimeComponent != null)
                modifier.AddComponent(removeTimeComponent);

            foreach (var propertyInfo in effectPropertyInfo)
                SetupEffectOn(modifier, propertyInfo, checkComponent, conditionalApplyComponent);

            //Make rest of the component that won't be used by anything else

            //---Stack---
            if (effectPropertyInfo[0].EffectComponent is IStackEffectComponent stackEffectComponent &&
                properties.StackComponentProperties != null)
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
                properties.Cost.Type != PoolStatType.None ? new CostComponent(properties.Cost.Type, properties.Cost.Value) : null,
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

            if (properties.AutomaticAct)
                modifier.SetAutomaticAct();

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
                modifier.AddComponent(new TimeComponent(checkComponent, propertyInfo.EffectDuration, propertyInfo.RepeatOnFinished));
        }
    }
}
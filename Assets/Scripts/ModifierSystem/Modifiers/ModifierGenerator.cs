using System;
using BaseProject;

namespace ModifierSystem
{
    public static class ModifierGenerator
    {
        public static Modifier GenerateModifier(ModifierGenerationProperties properties)
        {
            /*
                Modifier, Data.
                Target, Effect, Apply, CleanUp, Remove, TimeRemove.
                Init, TimeEffect, Stack, Refresh
             */

            Modifier modifier = new Modifier(properties.Name);

            //---Data---
            //damageData (set in properties)
            //conditionData (set in properties)

            //---Components---

            var targetComponent = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionData)
                : new TargetComponent(properties.LegalTarget);

            //---Effect---
            EffectComponent effectComponent = properties.EffectComponent;
            effectComponent.Setup(targetComponent);

            //---Apply---?

            //---CleanUp---
            //TODO ApplyComponent in cleanup
            CleanUpComponent cleanUpComponent = null;

            //---Remove---
            RemoveComponent removeComponent = new RemoveComponent(modifier, cleanUpComponent);

            //---TimeRemove & ConditionalRemove---
            TimeComponent removeTimeComponent = null;
            TimeComponent condtionalRemoveComponent = null;
            if (properties.RemoveOn != RemoveOn.None)
            {
                switch (properties.RemoveOn)
                {
                    case RemoveOn.ConditionApply:

                        break;
                    case RemoveOn.Time:
                        removeTimeComponent = new TimeComponent(removeComponent, properties.RemoveDuration);
                        break;
                }
            }

            //TODO, Specify if effect or remove component should be added to applyComponent
            //---Conditional Apply---
            ConditionalApplyComponent conditionalApplyComponent = null;
            if(properties.HasConditionData)
                conditionalApplyComponent = new ConditionalApplyComponent(effectComponent, targetComponent, properties.ConditionData);


            //---Finish Setup---
            //Add all components to modifier
            modifier.AddComponent(targetComponent);
            if (removeTimeComponent != null)
                modifier.AddComponent(removeTimeComponent);
            //modifier.AddComponent(new InitComponent(applyComponent, removalApplyComponent));
            //modifier.AddComponent(new TimeComponent(effectComponent, ));

            //Make rest of the component that won't be used by anything else
            if (properties.EffectOn.HasFlag(EffectOn.Init))
                modifier.AddComponent(new InitComponent(effectComponent));
            if (properties.EffectOn.HasFlag(EffectOn.Apply) && conditionalApplyComponent != null)
                modifier.AddComponent(new InitComponent(conditionalApplyComponent));
            if (properties.EffectOn.HasFlag(EffectOn.Time))
                modifier.AddComponent(new TimeComponent(effectComponent, properties.EffectDuration, properties.ResetOnFinished));

            modifier.FinishSetup(properties.DamageData);
            return modifier;
        }

        public static ComboModifier GenerateModifierTest(ComboModifierGenerationProperties properties)//TODOPRIO
        {
            /*
                Modifier, Data.
                Target, Effect, Apply, CleanUp, Remove, TimeRemove.
                Init, TimeEffect, Stack, Refresh
             */

            ComboModifier modifier = new ComboModifier(properties.Name, properties.Recipes, properties.Cooldown);

            //---Data---
            //damageData (set in properties)
            //conditionData (set in properties)

            //---Components---

            TargetComponent targetComponent;
            targetComponent = properties.HasConditionData
                ? new TargetComponent(properties.LegalTarget, properties.ConditionData)
                : new TargetComponent(properties.LegalTarget);

            //---Effect---
            EffectComponent effectComponent = properties.EffectComponent;
            effectComponent.Setup(targetComponent);

            //---Apply---?

            //---CleanUp---
            //TODO ApplyComponent in cleanup
            CleanUpComponent cleanUpComponent = null;

            //---Remove---
            RemoveComponent removeComponent = new RemoveComponent(modifier, cleanUpComponent);

            //---TimeRemove & ConditionalRemove---
            TimeComponent removeTimeComponent = null;
            TimeComponent condtionalRemoveComponent = null;
            if (properties.RemoveOn != RemoveOn.None)
            {
                switch (properties.RemoveOn)
                {
                    case RemoveOn.ConditionApply:

                        break;
                    case RemoveOn.Time:
                        removeTimeComponent = new TimeComponent(removeComponent, properties.RemoveDuration);
                        break;
                }
            }

            //---Conditional Apply---
            ConditionalApplyComponent conditionalApplyComponent = new ConditionalApplyComponent(removeComponent, targetComponent, properties.ConditionData);

            //---Finish Setup---
            //Add all components to modifier
            modifier.AddComponent(targetComponent);
            if (removeTimeComponent != null)
                modifier.AddComponent(removeTimeComponent);
            //modifier.AddComponent(new InitComponent(applyComponent, removalApplyComponent));
            //modifier.AddComponent(new TimeComponent(effectComponent, ));

            //Make rest of the component that won't be used by anything else
            if (properties.EffectOn.HasFlag(EffectOn.Init))
                modifier.AddComponent(new InitComponent(effectComponent));
            //if (properties.EffectOn.HasFlag(EffectOn.Apply))
            //    modifier.AddComponent(new InitComponent(conditionalApplyComponent));
            if (properties.EffectOn.HasFlag(EffectOn.Time))
                modifier.AddComponent(new TimeComponent(effectComponent, properties.EffectDuration, properties.ResetOnFinished));

            modifier.FinishSetup(properties.DamageData);
            return modifier;
        }
    }
}
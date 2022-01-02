using System.Collections.Generic;
using BaseProject;
using BaseProject.Utils;

namespace ModifierSystem
{
    public class InitComponent : Component, IInitComponent
    {
        private bool ConditionBased { get; }

        private readonly IEffectComponent[] _effectComponents;
        private readonly IApplyComponent[] _applyComponents;

        public InitComponent(params IEffectComponent[] effectComponents)
        {
            _effectComponents = effectComponents;
        }
        /// <summary>
        ///     Condition based Init
        /// </summary>
        public InitComponent(params IApplyComponent[] applyComponents)
        {
            ConditionBased = true;
            _applyComponents = applyComponents;
        }

        public void Init()
        {
            if(ConditionBased)
            {
                foreach (var applyComponent in _applyComponents)
                    applyComponent.Apply();
            }
            else
            {
                foreach (var effectComponent in _effectComponents)
                    effectComponent.SimpleEffect();
            }
        }

        public HashSet<StatusTag> GetStatusTags()
        {
            HashSet<StatusTag> tempStatusTags = new HashSet<StatusTag>();
            if (EffectComponentIsOfType<StatusComponent>())
                tempStatusTags.Add(new StatusTag(StatusType.Stun));
            if (EffectComponentIsOfType<StatusResistanceComponent>())
                tempStatusTags.Add(new StatusTag(StatusType.Resistance));//Res? Recursion?
            //if (EffectComponentIsOfType<SlowComponent>())
            //    tempStatusTags.Add(new StatusTag(StatusType.Slow));
            return tempStatusTags;
        }

        private bool EffectComponentIsOfType<T>() where T : IEffectComponent
        {
            foreach (var effectComponent in _effectComponents.EmptyIfNull())
            {
                if (effectComponent.GetType() == typeof(T))
                    return true;
            }

            return false;
        }
    }
}
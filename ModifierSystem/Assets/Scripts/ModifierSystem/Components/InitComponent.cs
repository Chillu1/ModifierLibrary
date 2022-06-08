using System.Collections.Generic;
using BaseProject;
using BaseProject.Utils;

namespace ModifierSystem
{
    public class InitComponent : Component, IInitComponent
    {
        private bool ConditionBased { get; }

        private readonly ICheckComponent[] _checkComponent;
        private readonly IConditionalApplyComponent[] _applyComponents;

        public InitComponent(params ICheckComponent[] checkComponent)
        {
            _checkComponent = checkComponent;
        }

        /// <summary>
        ///     Condition based Init
        /// </summary>
        public InitComponent(params IConditionalApplyComponent[] applyComponents)
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
                foreach (var checkComponent in _checkComponent)
                    checkComponent.Effect();
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
            foreach (var checkComponent in _checkComponent.EmptyIfNull())
            {
                if (checkComponent.EffectComponentIsOfType<T>())
                    return true;
            }

            return false;
        }
    }
}
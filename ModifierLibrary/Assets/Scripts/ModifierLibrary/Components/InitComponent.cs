using System.Collections.Generic;
using UnitLibrary;
using UnitLibrary.Utils;

namespace ModifierLibrary
{
    public class InitComponent : Component, IInitComponent
    {
        private bool ConditionBased { get; }

        private readonly ICheckComponent _checkComponent;
        private readonly IConditionalApplyComponent[] _applyComponents;

        public InitComponent(ICheckComponent checkComponent)
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
            if (ConditionBased)
            {
                foreach (var applyComponent in _applyComponents)
                    applyComponent.Apply();
            }
            else
            {
                _checkComponent.Effect();
            }
        }

        public HashSet<StatusTag> GetStatusTags()
        {
            HashSet<StatusTag> tempStatusTags = new HashSet<StatusTag>();
            if (EffectComponentIsOfType<StatusComponent>())
                tempStatusTags.Add(new StatusTag(StatusType.Stun));
            if (EffectComponentIsOfType<StatusResistanceComponent>())
                tempStatusTags.Add(new StatusTag(StatusType.Resistance)); //Res? Recursion?
            //if (EffectComponentIsOfType<SlowComponent>())
            //    tempStatusTags.Add(new StatusTag(StatusType.Slow));
            return tempStatusTags;
        }

        private bool EffectComponentIsOfType<T>() where T : IEffectComponent
        {
            return _checkComponent != null && _checkComponent.EffectComponentIsOfType<T>();
        }
    }
}
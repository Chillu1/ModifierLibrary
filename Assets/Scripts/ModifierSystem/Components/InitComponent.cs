using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    public class InitComponent : Component, IInitComponent
    {
        private readonly IApplyComponent _applyComponent;

        public InitComponent(IApplyComponent applyComponent)
        {
            _applyComponent = applyComponent;
        }

        public void Init()
        {
            _applyComponent.Apply();
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
            return _applyComponent.GetType() == typeof(T);
        }
    }
}
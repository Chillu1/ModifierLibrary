using System;
using System.Collections.Generic;
using BaseProject;
using BaseProject.Utils;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class Modifier : IModifier, IEventCopy<Modifier>
    {
        public string Id { get; private set; }
        public bool ApplierModifier { get; }
        public TargetComponent TargetComponent { get; private set; }
        [CanBeNull] private IInitComponent InitComponent { get; set; }
        [CanBeNull] private IApplyComponent ApplyComponent { get; set; }
        [CanBeNull] private List<ITimeComponent> TimeComponents { get; set; }
        [CanBeNull] private StatusTag[] StatusTags { get; set; }
        [CanBeNull] private IStackComponent StackComponent { get; set; }
        [CanBeNull] private IRefreshComponent RefreshComponent { get; set; }

        private bool _setupFinished;

        public Modifier(string id, bool applierModifier = false)
        {
            Id = id;
            ApplierModifier = applierModifier;
        }

        public void Init(ModifierController modifierController)
        {
            InitComponent?.Init();
            if (TimeComponents != null)
                foreach (var timeComponent in TimeComponents)
                {
                    timeComponent.Init(modifierController);
                }
        }

        public StatusTag[] FinishSetup([CanBeNull] DamageData[] damageData = null)
        {
            if (_setupFinished)
                Log.Error("Setup already finished, overwriting");

            var tempStatusTags = new List<StatusTag>();

            if (damageData != null)
                foreach (var data in damageData)
                {
                    if (data.DamageType != DamageType.None)
                        tempStatusTags.Add(new StatusTag(data.DamageType));
                    if (data.ElementData != null && data.ElementData.ElementalType != ElementalType.None)
                    {
                        if(data.ElementData.EffectValue != 0)
                            tempStatusTags.Add(new StatusTag(StatusType.Element));
                        tempStatusTags.Add(new StatusTag(data.ElementData.ElementalType));
                    }
                }

            if (InitComponent != null)
            {
                if (InitComponent.EffectComponentIsOfType<StatusComponent>())
                    tempStatusTags.Add(new StatusTag(StatusType.Stun));
                //if (InitComponent.EffectComponentIsOfType<SlowComponent>())
                //    tempStatusTags.Add(new StatusTag(StatusType.Slow));
            }

            if (TimeComponents != null)
                foreach (var timeComponent in TimeComponents)
                {
                    if (timeComponent.EffectComponentIsOfType<DamageComponent>(true))
                        tempStatusTags.Add(new StatusTag(StatusType.DoT));
                    //if (timeComponent.EffectComponentIsOfType<SlowComponent>(true))not timecomponent?
                    //    tempStatusTags.Add(new StatusTag(StatusType.Slow));
                    //if (timeComponent.EffectComponentIsOfType<StunComponent>(true))
                    //    tempStatusTags.Add(new StatusTag(StatusType.Stun));
                }

            StatusTags = tempStatusTags.ToArray();
            _setupFinished = true;
            return StatusTags;
        }

        public void Update(float deltaTime, StatusResistances ownerStatusResistances)
        {
            //Log.Info(TimeComponents?.Count +" ID: "+Id);
            for (int i = 0; i < TimeComponents?.Count; i++)
            {
                var timeComponent = TimeComponents[i];
                double multiplier = 1d;

                //Only apply status res to buff/debuff duration, for now
                //TODO We probably shouldn't this every frame (status)
                if (timeComponent.EffectComponentIsOfType<RemoveComponent>(false))
                    multiplier = ownerStatusResistances.GetStatusMultiplier(StatusTags);

                timeComponent.Update(deltaTime, multiplier);
            }
        }

        public void AddComponent(IInitComponent initComponent)
        {
            if (InitComponent != null)
            {
                Log.Error(Id+ " already has a init component", "modifiers");
                return;
            }

            InitComponent = initComponent;
        }

        public void AddComponent(IApplyComponent applyComponent)
        {
            if (ApplyComponent != null)
            {
                Log.Error(Id+ " already has a apply component", "modifiers");
                return;
            }

            ApplyComponent = applyComponent;
        }

        public void AddComponent(TargetComponent targetComponent)
        {
            if (TargetComponent != null)
            {
                Log.Error(Id+ " already has a target component", "modifiers");
                return;
            }

            TargetComponent = targetComponent;
        }

        public void AddComponent(ITimeComponent timeComponent)
        {
            if (TimeComponents == null)
                TimeComponents = new List<ITimeComponent>(2);

            TimeComponents.Add(timeComponent);
        }

        public void AddComponent(IStackComponent stackComponent)
        {
            if (StackComponent != null)
            {
                Log.Error(Id+ " already has a stack component", "modifiers");
                return;
            }

            StackComponent = stackComponent;
        }

        public void AddComponent(IRefreshComponent refreshComponent)
        {
            if (StackComponent != null)
            {
                Log.Error(Id+ " already has a refresh component", "modifiers");
                return;
            }

            RefreshComponent = refreshComponent;
        }

        public void TryApply(Being target)
        {
            bool validTarget = TargetComponent.SetTarget(target);
            if(validTarget)
                Apply();
        }

        private void Apply()
        {
            if (ApplyComponent == null)
            {
                Log.Error("No apply component", "modifiers");
                return;
            }

            ApplyComponent.Apply();
        }

        public bool Stack()
        {
            if (StackComponent == null)
                return false;

            StackComponent.Stack();
            return true;
        }

        public bool Refresh()
        {
            if (RefreshComponent == null)
                return false;

            RefreshComponent.Refresh();
            return true;
        }

        public void CopyEvents(Modifier prototype)
        {
            if(prototype.StackComponent != null)
            {
                //Log.Info("Clone "+Id);
                StackComponent = new StackComponent((StackComponent)prototype.StackComponent);
            }
            //this.event = prototype.event //or we will need to copy it over properly, with a new reference
        }

        public bool ValidatePrototypeSetup()
        {
            bool success = true;

            if (TargetComponent == null)
            {
                Log.Error("Modifier needs a target component", "modifiers");
                success = false;
            }

            if (ApplierModifier || Id.Contains("Applier"))
            {
                if (ApplyComponent == null)
                {
                    Log.Error("ModifierApplier needs an ApplyComponent", "modifiers");
                    success = false;
                }
            }
            //Not applier, check for other components
            else if ((TimeComponents == null || TimeComponents.Count == 0) && InitComponent == null)
            {
                Log.Error("Modifier needs either an init or time component to work (unless maybe its a flag modifier?)", "modifiers");
                success = false;
            }

            if (Id.Contains("Applier") && !ApplierModifier)
            {
                Log.Error("Id contains applier, but the flag isn't set", "modifiers");
                success = false;
            }
            if (!Id.Contains("Applier") && ApplierModifier)
            {
                Log.Error("Id doesn't contain applier, and the applier flag is set", "modifiers");
                success = false;
            }

            if (!_setupFinished)
            {
                Log.Error("Setup has not been properly finished", "modifiers");
                success = false;
            }

            return success;
        }

        public object Clone()
        {
            return this.Copy(); // MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }
    }
}
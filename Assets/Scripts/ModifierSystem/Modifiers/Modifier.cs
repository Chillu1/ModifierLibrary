using System;
using System.Collections.Generic;
using System.Linq;
using BaseProject;
using BaseProject.Utils;
using Force.DeepCloner;
using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     Buff/Debuff on beings, can do anything, slow, over time/delayed stun, change stats, deal damage, resurrect
    /// </summary>
    public class Modifier : IEntity<string>, ICloneable, IEventCopy<Modifier>
    {
        public string Id { get; private set; }
        public bool IsApplierModifier => ApplierType != ApplierType.None;
        public ApplierType ApplierType { get; }
        public bool IsConditionModifier { get; }
        public bool ToRemove { get; private set; }
        public TargetComponent TargetComponent { get; private set; }
        public StatusTag[] StatusTags { get; private set; }

        private bool IsAutomaticCasting { get; set; }
        [CanBeNull] private IInitComponent InitComponent { get; set; }
        [CanBeNull] private IApplyComponent ApplyComponent { get; set; }
        [CanBeNull] private List<ITimeComponent> TimeComponents { get; set; }
        [CanBeNull] private CleanUpComponent CleanUpComponent { get; set; }
        [CanBeNull] private IStackComponent StackComponent { get; set; }
        [CanBeNull] private IRefreshComponent RefreshComponent { get; set; }
        [CanBeNull] private ICooldownComponent CooldownComponent { get; set; }
        [CanBeNull] private ICostComponent CostComponent { get; set; }

        private bool _setupFinished;

        public Modifier(string id, ApplierType applierType = ApplierType.None, bool isConditionModifier = false)
        {
            Id = id;
            if (applierType != ApplierType.None)
                ApplierType = applierType;

            IsConditionModifier = isConditionModifier;
        }

        public void Init()
        {
            InitComponent?.Init();
        }

        public void FinishSetup(DamageData[] damageData = null)
        {
            if (_setupFinished)
                Log.Error("Setup already finished, overwriting. Should never happen");

            var tempStatusTags = new HashSet<StatusTag>();

            foreach (var data in damageData.EmptyIfNull())
                tempStatusTags.UnionWith(data.GetStatusTags());

            if (InitComponent != null)
                tempStatusTags.UnionWith(InitComponent.GetStatusTags());

            foreach (var timeComponent in TimeComponents.EmptyIfNull())
                tempStatusTags.UnionWith(timeComponent.GetStatusTags());

            StatusTags = tempStatusTags.ToArray();
            _setupFinished = true;
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
                if (timeComponent.IsRemove)
                    multiplier = ownerStatusResistances.GetStatusMultiplier(StatusTags);

                timeComponent.Update(deltaTime, multiplier);
            }

            CooldownComponent?.Update(deltaTime);
        }

        public void AddComponent(IInitComponent initComponent)
        {
            if (InitComponent != null)
            {
                Log.Error(Id + " already has a init component", "modifiers");
                return;
            }

            InitComponent = initComponent;
        }

        public void AddComponent(IApplyComponent applyComponent)
        {
            if (ApplyComponent != null)
            {
                Log.Error(Id + " already has a apply component", "modifiers");
                return;
            }

            if (applyComponent is IConditionalApplyComponent cond)
            {
                SetupCleanUpComponent();
                CleanUpComponent!.AddComponent((ConditionalApplyComponent)cond);
            }

            ApplyComponent = applyComponent;
        }

        public void AddComponent(TargetComponent targetComponent)
        {
            if (TargetComponent != null)
            {
                Log.Error(Id + " already has a target component", "modifiers");
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
                Log.Error(Id + " already has a stack component", "modifiers");
                return;
            }

            StackComponent = stackComponent;
        }

        public void AddComponent(IRefreshComponent refreshComponent)
        {
            if (RefreshComponent != null)
            {
                Log.Error(Id + " already has a refresh component", "modifiers");
                return;
            }

            RefreshComponent = refreshComponent;
        }

        public void AddComponent(ICooldownComponent cooldownComponent)
        {
            if (CooldownComponent != null)
            {
                Log.Error(Id + " already has a cooldown component", "modifiers");
                return;
            }

            CooldownComponent = cooldownComponent;
        }

        public void AddComponent(ICostComponent costComponent)
        {
            if (CostComponent != null)
            {
                Log.Error(Id + " already has a cost component", "modifiers");
                return;
            }

            CostComponent = costComponent;
        }

        public void SetupOwner(Being owner)
        {
            TargetComponent.SetupOwner(owner);
            CostComponent?.SetupOwner(owner);
        }

        public void SetAutomaticCast(bool automaticCast = true)
        {
            IsAutomaticCasting = automaticCast;
        }

        public bool TryCast(Being target, bool automaticCast = false)
        {
            if (target == null && !TargetComponent.LegalTarget.HasFlag(LegalTarget.Ground))
            {
                if (automaticCast) //No target, delay casting
                    return false;

                Log.Error("Can't cast a modifier on a null target", "modifiers");
                return false;
            }

            return TryApply(target);
        }

        public bool TryApply(Being target)
        {
            bool validTarget = TargetComponent.SetTarget(target);
            bool validCooldown = CooldownComponent == null || CooldownComponent.IsReady();
            bool validCost = CostComponent == null || CostComponent.ContainsCost();

            if (validTarget && validCooldown && validCost)
            {
                Apply();
                CooldownComponent?.ResetTimer();
                CostComponent?.ApplyCost();
                return true;
            }

            return false;
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

        public void SetForRemoval()
        {
            ToRemove = true;
        }

        public void CopyEvents(Modifier prototype)
        {
        }

        public virtual bool ValidatePrototypeSetup()
        {
            bool valid = true;

            if (TargetComponent == null)
            {
                Log.Error("Modifier needs a target component", "modifiers");
                valid = false;
            }

            if (IsApplierModifier || Id.Contains("Applier"))
            {
                if (ApplyComponent == null && StackComponent == null)
                {
                    Log.Error("ModifierApplier needs an ApplyComponent or StackComponent", "modifiers");
                    valid = false;
                }
            }
            //Not applier, check for other components
            else if ((TimeComponents == null || TimeComponents.Count == 0) && InitComponent == null)
            {
                Log.Error("Modifier needs either an init or time component to work (unless maybe its a flag modifier?)", "modifiers");
                valid = false;
            }

            if (Id.Contains("Applier") && !IsApplierModifier)
            {
                Log.Error("Id contains applier, but the flag isn't set", "modifiers");
                valid = false;
            }

            if (!Id.Contains("Applier") && IsApplierModifier)
            {
                Log.Error("Id doesn't contain applier, and the applier flag is set", "modifiers");
                valid = false;
            }

            if (!_setupFinished || StatusTags == null)
            {
                Log.Error("Setup has not been properly finished", "modifiers");
                StatusTags = new StatusTag[0];
                valid = false;
            }

            return valid;
        }

        private void SetupCleanUpComponent()
        {
            CleanUpComponent ??= new CleanUpComponent();
        }

        public virtual object Clone()
        {
            return this.DeepClone();
            //return this.Copy();
        }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }
    }
}
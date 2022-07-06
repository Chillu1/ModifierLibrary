using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseProject;
using BaseProject.Utils;
using Force.DeepCloner;
using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     Buff/Debuff on beings, can do anything, slow, over time/delayed stun, change stats, deal damage, resurrect
    /// </summary>
    public class Modifier : IEntity<string>, ICloneable, IFullDisplay
    {
        public string Id { get; }
        [CanBeNull] public ModifierInfo Info { get; }
        public AddModifierParameters Parameters { get; }
        public bool IsApplierModifier => ApplierType != ApplierType.None;
        public ApplierType ApplierType { get; }
        public bool IsConditionModifier { get; }
        //public bool IsComboPart { get; private set; }
        public bool ToRemove { get; private set; }
        public TargetComponent TargetComponent { get; private set; }
        public StatusTag[] StatusTags { get; private set; }
        public bool IsAutomaticCasting { get; private set; }

        [CanBeNull] private IInitComponent InitComponent { get; set; }
        [CanBeNull] private IApplyComponent ApplyComponent { get; set; }
        [CanBeNull] private List<ITimeComponent> TimeComponents { get; set; }
        [CanBeNull] private CleanUpComponent CleanUpComponent { get; set; }
        [CanBeNull] private IStackComponent StackComponent { get; set; }
        [CanBeNull] private IRefreshComponent RefreshComponent { get; set; }
        [CanBeNull] private ICheckComponent CheckComponent { get; set; }

        private bool _setupFinished;
        protected IModifierGenerationProperties Properties { get; private set; }

        public Modifier(string id, ModifierInfo info, AddModifierParameters parameters,
            ApplierType applierType = ApplierType.None, bool isConditionModifier = false)
        {
            Id = id;
            Info = info;
            Parameters = parameters;
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

        public void AddProperties(IModifierGenerationProperties properties)
        {
            Properties = properties;
        }

        public void Update(float deltaTime, StatusResistances ownerStatusResistances)
        {
            //Log.Info(Id);
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

            CheckComponent?.CooldownComponent?.Update(deltaTime);
            Info?.Update(deltaTime);
        }

        public void AddComponent(IInitComponent initComponent)
        {
            if (InitComponent != null)
            {
                Log.Warning(Id + " already has an init component, skipping", "modifiers");
                return;
            }

            InitComponent = initComponent;
        }

        public void AddComponent(IApplyComponent applyComponent)
        {
            if (ApplyComponent != null)
            {
                Log.Error(Id + " already has an apply component", "modifiers");
                return;
            }

            if (applyComponent is IConditionalApplyComponent cond)
            {
                CleanUpComponent ??= new CleanUpComponent((ConditionalApplyComponent)cond);
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

        public void AddComponent(ICheckComponent checkComponent)
        {
            if (CheckComponent != null)
            {
                Log.Error(Id + " already has a check component", "modifiers");
                return;
            }

            Info?.Setup(checkComponent);

            CheckComponent = checkComponent;
        }

        public void SetupOwner(Unit owner)
        {
            TargetComponent.SetupOwner(owner);
            CheckComponent?.CostComponent?.SetupOwner(owner);
        }

        public void SetupApplierOwner(Unit owner)
        {
            TargetComponent.SetupApplierOwner(owner);
        }

        public void SetAutomaticCast(bool automaticCast = true)
        {
            IsAutomaticCasting = automaticCast;
        }

        public bool TryCast(Unit target, bool automaticCast = false)
        {
            //Log.Info($"{Id} is trying to cast", "modifiers");
            if (target == null && !TargetComponent.LegalTarget.HasFlag(LegalTarget.Ground))
            {
                if (automaticCast) //No target, delay casting
                    return false;

                Log.Error("Can't cast a modifier on a null target", "modifiers");
                return false;
            }

            return TryApply(target);
        }

        /// <summary>
        ///     All appliers (attack, cast). NOT conditional, etc
        /// </summary>
        public bool TryApply(Unit target)
        {
            bool validApply = true;

            if (!TargetComponent.SetTarget(target))
            {
                validApply = false;
                //Log.Error(Id + " can't apply to " + target.Id, "modifiers");
            }
            if (CheckComponent != null && !CheckComponent.Check())
            {
                validApply = false;
                //Log.Error(Id + " failed check", "modifiers");
            }

            if (validApply)
            {
                Apply();
                CheckComponent?.Apply();
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

        public virtual bool ValidatePrototypeSetup()
        {
            bool valid = true;

            if (TargetComponent == null)
            {
                Log.Error("Modifier needs a target component", "modifiers");
                valid = false;
            }

            if (IsApplierModifier || Id.EndsWith("Applier"))
            {
                if (ApplyComponent == null && StackComponent == null && !IsConditionModifier)
                {
                    Log.Info(Id+"_"+IsConditionModifier);
                    Log.Error("ModifierApplier needs an ApplyComponent, StackComponent or has to be condition based", "modifiers");
                    valid = false;
                }
            }
            //Not applier, check for other components
            else if ((TimeComponents == null || TimeComponents.Count == 0) && InitComponent == null)
            {
                Log.Error("Modifier needs either an init or time component to work (unless maybe its a flag modifier?)", "modifiers");
                valid = false;
            }

            if (Id.EndsWith("Applier") && !IsApplierModifier && !IsConditionModifier)
            {
                Log.Error("Id contains applier, but the flag isn't set, and it's not a condition modifier", "modifiers");
                valid = false;
            }

            if (!Id.EndsWith("Applier") && IsApplierModifier)
            {
                Log.Error("Id doesn't contain applier, and the applier flag is set", "modifiers");
                valid = false;
            }

            if (!Id.Contains("Test") && Info == null)
            {
                Log.Error("Non-test modifier need info", "modifiers");
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

        public string DisplayText()
        {
            string info = Info == null ? ToString() : Info.DisplayName;
            info += "\n";
            info += Info?.CheckInfo;

            if (TimeComponents != null && TimeComponents.Count > 0)
            {
                info += TimeComponents[0].DisplayText();

                if (TimeComponents.Count > 1)
                    info += TimeComponents[1].DisplayText();
            }

            return info;
        }

        public string FullText()
        {
            string info = Info == null ? ToString() : Info.GetInfo();
            info += Info?.CheckInfo;

            if (TimeComponents != null && TimeComponents.Count > 0)
            {
                info += TimeComponents[0].DisplayText();

                if (TimeComponents.Count > 1)
                    info += TimeComponents[1].DisplayText();
            }

            return info;
        }

        public Modifier PropertyClone()
        {
            if (Properties == null)
            {
                Log.Error($"{Id} has no properties, falling back to deep clone", "modifiers");
                return this.DeepClone();
            }

            if (Properties is ComboModifierGenerationProperties comboProperties)
                return ModifierGenerator.GenerateComboModifier(comboProperties);
            if (Properties is AuraEffectModifierGenerationProperties auraProperties)
                return ModifierGenerator.GenerateModifier(auraProperties);
            if (Properties is ModifierGenerationProperties properties)
                return ModifierGenerator.GenerateModifier(properties);
            if (Properties is ApplierModifierGenerationProperties applierProperties)
                return ModifierGenerator.GenerateApplierModifier(applierProperties);

            Log.Error($"{Id} has unknown properties kind, falling back to deep clone", "modifiers");
            return this.DeepClone();
        }

        public virtual object Clone()
        {
            return PropertyClone();
        }

        public override string ToString()
        {
            return $"Id: {Id}";
        }
    }
}
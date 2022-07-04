using System;
using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    [Flags]
    public enum EffectOn
    {
        None = 0,
        Init = 1,
        Apply = 2,
        Time = 4,

        /// <summary>
        ///     Is called on Init- and TimeComponent. Usually used for DoT, Stack, etc
        /// </summary>
        /// <example>DoT</example>
        InitTime = Init | Time,
    }

    public class ModifierGenerationProperties : IModifierGenerationProperties
    {
        public string Id { get; }
        public ModifierInfo Info { get; }
        public LegalTarget LegalTarget { get; }
        public bool IsApplier => ApplierType != ApplierType.None;
        public ApplierType ApplierType { get; private set; }
        public bool HasConditionData { get; private set; }
        public ConditionEventTarget ConditionEventTarget { get; private set; }
        public ConditionEvent ConditionEvent { get; private set; }

        public DamageData[] DamageData { get; protected set; }

        public bool Removable { get; private set; }
        public double RemoveDuration { get; private set; }

        public List<EffectPropertyInfo> EffectPropertyInfo { get; private set; }
        protected EffectPropertyInfo currentEffectPropertyInfo;
        public StackComponentProperties StackComponentProperties { get; private set; }
        public RefreshEffectType RefreshEffectType { get; private set; }
        public (CostType, float) Cost { get; private set; }
        public double Chance { get; private set; } = -1;

        public ModifierGenerationProperties(string id, ModifierInfo info, LegalTarget legalTarget = LegalTarget.Self)
        {
            Id = id;
            Info = info;
            LegalTarget = legalTarget;
            EffectPropertyInfo = new List<EffectPropertyInfo>(2);
        }

        public void AddConditionData(ConditionEventTarget conditionEventTarget, ConditionEvent conditionEvent)
        {
            if (conditionEventTarget == ConditionEventTarget.None)
                Log.Error("Wrong ConditionTarget, None");
            if (conditionEvent == ConditionEvent.None)
                Log.Error("Wrong BeingConditionEvent, None");

            HasConditionData = true;
            ConditionEventTarget = conditionEventTarget;
            ConditionEvent = conditionEvent;
            SetEffectOnApply(); //Always true?
        }

        public void SetApplier(ApplierType applierType)
        {
            ApplierType = applierType;
        }

        public void AddEffect(EffectComponent effectComponent, DamageData[] damageData = null)
        {
            currentEffectPropertyInfo = new EffectPropertyInfo(effectComponent);
            EffectPropertyInfo.Add(currentEffectPropertyInfo);
            if (damageData != null)
                DamageData = damageData;
        }

        public void SetEffectOnInit()
        {
            currentEffectPropertyInfo.SetEffectOnInit();
        }

        /// <param name="resetOnFinished">Resets the timer after duration is finished (interval)</param>
        public void SetEffectOnTime(double duration, bool resetOnFinished)
        {
            currentEffectPropertyInfo.SetEffectOnTime(duration, resetOnFinished);
        }

        /// <summary>
        ///     Should apply be passed to init (conditional effect, ex: OnDeath)
        /// </summary>
        private void SetEffectOnApply()
        {
            currentEffectPropertyInfo.SetEffectOnApply();
        }

        /// <summary>
        ///     Is removed after <paramref name="removeDuration"/>
        /// </summary>
        /// <remarks>Default = linger</remarks>
        public void SetRemovable(double removeDuration = 0.5d)
        {
            Removable = true;
            RemoveDuration = removeDuration;
        }

        public void SetEffectOnStack(StackComponentProperties stackComponentProperties)
        {
            StackComponentProperties = stackComponentProperties;
        }

        public void SetRefreshable(RefreshEffectType refreshEffectType = RefreshEffectType.RefreshDuration)
        {
            RefreshEffectType = refreshEffectType;
        }

        public void SetCost(CostType costType, int amount)
        {
            Cost = (costType, amount);
        }

        public void SetChance(double chance)
        {
            Chance = chance;
        }
    }
}
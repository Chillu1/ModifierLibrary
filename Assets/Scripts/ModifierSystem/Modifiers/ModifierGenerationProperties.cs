using System;
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

    public class ModifierGenerationProperties
    {
        public string Name { get; }
        public LegalTarget LegalTarget { get; }
        public bool IsApplier => ApplierType != ApplierType.None;
        public ApplierType ApplierType { get; private set; }
        public bool HasConditionData { get; private set; }
        public ConditionEventTarget ConditionEventTarget { get; private set; }
        public ConditionEvent ConditionEvent { get; private set; }

        public DamageData[] DamageData { get; private set; }

        public bool Removable { get; private set; }
        public double RemoveDuration { get; private set; }

        public EffectComponent EffectComponent { get; private set; }

        public EffectOn EffectOn { get; private set; }
        public bool ResetOnFinished { get; private set; }
        public double EffectDuration { get; private set; }

        public StackComponentProperties StackComponentProperties { get; private set; }
        public RefreshEffectType RefreshEffectType { get; private set; }

        public ModifierGenerationProperties(string name, LegalTarget legalTarget = LegalTarget.Self)
        {
            Name = name;
            LegalTarget = legalTarget;
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
            EffectComponent = effectComponent;
            if (damageData != null)
                DamageData = damageData;
        }

        public void SetEffectOnInit()
        {
            EffectOn |= EffectOn.Init;
        }

        /// <param name="resetOnFinished">Resets the timer after duration is finished (interval)</param>
        public void SetEffectOnTime(double duration, bool resetOnFinished)
        {
            EffectOn |= EffectOn.Time;
            EffectDuration = duration;
            ResetOnFinished = resetOnFinished;
        }

        /// <summary>
        ///     Should apply be passed to init (conditional effect, ex: OnDeath)
        /// </summary>
        private void SetEffectOnApply()
        {
            EffectOn |= EffectOn.Apply;
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

        public void SetRefreshable(RefreshEffectType refreshDuration)
        {
            RefreshEffectType = refreshDuration;
        }
    }

    public class ComboModifierGenerationProperties : ModifierGenerationProperties
    {
        public ComboRecipes Recipes { get; private set; }
        public float Cooldown { get; private set; }

        public ComboModifierGenerationProperties(string name, LegalTarget legalTarget = LegalTarget.Self) : base(name, legalTarget)
        {
        }

        public void AddRecipes(ComboRecipes comboRecipes)
        {
            Recipes = comboRecipes;
        }

        public void SetCooldown(float cooldown)
        {
            Cooldown = cooldown;
        }
    }
}
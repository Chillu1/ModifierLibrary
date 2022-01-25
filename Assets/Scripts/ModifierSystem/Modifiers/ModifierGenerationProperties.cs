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

    public enum RemoveOn
    {
        None = 0,
        ConditionApply,
        Time,
    }

    public class ModifierGenerationProperties
    {
        public string Name { get; }
        public LegalTarget LegalTarget { get; }


        public bool HasConditionData { get; private set; }
        public ConditionEventData ConditionData { get; private set; }


        public bool IsDamageEffect { get; private set; }
        public DamageData[] DamageData { get; private set; }
        public DamageComponent.DamageComponentStackEffect DamageStackEffect { get; private set; } = DamageComponent.DamageComponentStackEffect.None;

        public RemoveOn RemoveOn { get; private set; }
        //if RemoveOn not none:
        public double RemoveDuration { get; private set; }

        public bool HasStackEffect { get; private set; }
        //TODO public StackComponentProperties _stackComponentProperties; needs to not need StackEffectComponent on constructor
        //TODO StackEffect in EffectComponent
        //TODO StackComponent, Value(s)

        //TODO Refresh

        public IEffectComponent EffectComponent { get; private set; }

        public EffectOn EffectOn { get; private set; }
        public bool ResetOnFinished { get; private set; }
        public double EffectDuration { get; private set; }

        public ModifierGenerationProperties(string name, LegalTarget legalTarget = LegalTarget.Self)
        {
            Name = name;
            LegalTarget = legalTarget;
        }

        public void AddConditionData(ConditionEventData conditionData)
        {
            HasConditionData = true;
            ConditionData = conditionData;
        }

        public void AddEffect(EffectComponent effectComponent)
        {
        }

        /// <summary>
        ///     Damage that will affect target (by dealing damage to them)
        /// </summary>
        public void AddDamageData(DamageData[] data,
            DamageComponent.DamageComponentStackEffect stackEffect = DamageComponent.DamageComponentStackEffect.None)
        {
            IsDamageEffect = true;
            DamageData = data;
            DamageStackEffect = stackEffect;
        }

        public void SetEffectOnInit() => EffectOn |= EffectOn.Init;

        /// <param name="resetOnFinished">Resets the timer after duration is finished (interval)</param>
        public void SetEffectOnTime(double duration, bool resetOnFinished)
        {
            EffectOn |= EffectOn.Time;
            EffectDuration = duration;
            ResetOnFinished = resetOnFinished;
        }


        /// <summary>
        ///     Is removed after <paramref name="removeDuration"/>
        /// </summary>
        /// <remarks>Default = linger</remarks>
        public void SetRemovable(double removeDuration = 0.5d, RemoveOn removeOn = RemoveOn.Time)
        {
            RemoveOn = removeOn;
            RemoveDuration = removeDuration;
        }
    }
}
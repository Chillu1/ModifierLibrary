using System;
using JetBrains.Annotations;

namespace ComboSystem
{
    //TODO list:
    //A buff that applies a debuff
    //Single use buff, duration
    //DoT debuff
    //DoT buff that applies a DoT debuff
    //Combo prototyping

    //Types of modifiers:
    //  Base mods: Duration, EffectEveryX, Applier,
    //OneUseBuff permanent, OneUseBuff duration
    //EffectOverTimeEveryXSecond (Duration)

    //Design:
    //How to apply debuffs from a character?
    //How do I differentiate between attack, cast, ally, etc modifiers for ModifierApplier?
    //Clean interaction between applied buffs on unit
    //All buffs in a single collection, ticking
    //Calculate base stats, then add percentages, every time our stats change, recalculate
    //A buff should apply a separate debuff, instead of having internal different state

    //TODO:
    //Effect stacks+max effect stacks. IsEffectStackable. IsDurationStackable. IsDurationRefreshable?. IsForever.
    public abstract class Modifier : ICloneable
    {
        public event Action<Modifier> Removed;
        [CanBeNull] public Character Target { get; protected set; }

        /// <summary>
        ///     Called once, when modifier is added to the collection
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        ///     Called when modifier is applied
        /// </summary>
        protected virtual bool Apply()
        {
            if (Target == null)
            {
                Log.Error("We tried to apply modifier without a target");
                return false;
            }

            return true;
        }

        protected virtual void Remove()
        {
            Removed?.Invoke(this);
        }

        public virtual void Update(float deltaTime)
        {
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool SetTarget(Character target)
        {
            if (!target.IsValidTarget(this))
                return false;

            if (Target != null)
            {
                Log.Error("Target isn't null");
                return false;
            }

            Target = target;
            return true;
        }
    }

    public abstract class Modifier<TDataType> : Modifier
    {
        public TDataType Data { get; protected set; }
    }

    // public class ModifierFactory<TDataType, TModifierType> where TModifierType : Modifier<TDataType>, new()
    // {
    //     public TDataType data;
    //
    //     public Modifier GetModifier(ICharacter Target)
    //     {
    //         return new TModifierType {Data = this.data, Target = Target};
    //     }
    // }
}
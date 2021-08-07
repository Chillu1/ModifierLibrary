using System;
using JetBrains.Annotations;

namespace ComboSystem
{
    //TODO list:
    /*
     *
Constructor for id, with optional modpriperties
     *
     */
    //Equality of memberwise clone/list contains
    //Data should be structs that arent inherited, but composed instead?
    //Refreshable modifier
    //Stackable modifier
    //Combo prototyping
    //Stats
    //Damage

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
    public abstract class Modifier : ICloneable, IEquatable<Modifier>
    {
        public event Action<Modifier> Removed;
        public string Id { get; protected set; }
        public ModifierProperties ModifierProperties { get; protected set; }
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

            bool targetHasModifier = Target.ModifierController.HasModifier(this);
            if (!targetHasModifier)
                return true;

            switch (ModifierProperties)
            {
                //If not stackable or refreshable, and Target already has the modifier, don't apply
                case ModifierProperties.None:
                    return false;
                case ModifierProperties.Stackable:
                    Stack();
                    break;
                case ModifierProperties.Refreshable:
                    Refresh();
                    break;
                default:
                    Log.Error("ModifierProperty " + ModifierProperties + " isn't implemented");
                    return false;
            }

            return true;
        }

        protected virtual void Remove()
        {
            Removed?.Invoke(this);
        }

        protected virtual void Stack()
        {
        }

        protected virtual void Refresh()
        {
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

        public bool Equals(Modifier other)
        {
            return Equals(this, other);
        }
        // public virtual bool Equals(Modifier other)
        // {
        //     if (ReferenceEquals(null, other)) return false;
        //     if (ReferenceEquals(this, other)) return true;
        //     return ModifierProperties == other.ModifierProperties && this.GetType() == other.GetType();
        // }
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
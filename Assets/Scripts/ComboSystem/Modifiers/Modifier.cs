using System;
using ComboSystem.Utils;
using JetBrains.Annotations;

namespace ComboSystem
{
    //TODO list:
    //Combo prototyping, how do we want to store the combo "recipes"? With ModifierProtoypes/NewProtoypeclass? Should they have their own data who they combo with? Maybe better not?
    //Adds a buff to collection, after adding we check for any recipes, if there is one that fits the criteria & its not already active, add & apply it
        //Combo buffs:
            //X specific stat buffs (movement speed buff, attack speed, evasion = special "cat" buff)
            //X specific buffs together
            //Elemental combos
    //What kind of stackable behaviours do we want?:
    //  Stacks increases value/power
    //  Stacks increase speed/interval of DoT/effect,
    //A stackable DoT modifier
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
    public abstract class Modifier : ICloneable//, IEquatable<Modifier>
    {
        public event Action<Modifier> Removed;
        public string Id { get; protected set; }
        public ModifierProperties ModifierProperties { get; protected set; }
        [CanBeNull] public Character Target { get; protected set; }

        protected Modifier(string id, ModifierProperties modifierProperties = default)
        {
            Id = id;
            ModifierProperties = modifierProperties;
        }

        // protected Modifier(Modifier other)
        // {
        //     Id = other.Id;
        //     ModifierProperties = other.ModifierProperties;
        //     //Prob cont copy target, it shouldn't have one when we copy (from prototypes), but dont do it anyway
        // }

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
            if (!ApplyIsValid())
                return false;

            //Dont Refresh/Stack on apply? But on adding the modifier instead?
            /*bool targetHasModifier = Target.ModifierController.HasModifier(this);
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
            }*/

            return true;
        }

        protected virtual bool ApplyIsValid()
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

        public virtual void Stack()
        {
            Log.Verbose("Stacked: " + this);
        }

        public virtual void Refresh()
        {
            Log.Verbose("Refreshed: " + this);
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual object Clone()
        {
            return this.Copy();
        }

        public Modifier ShallowCopy()
        {
            return (Modifier)MemberwiseClone();
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

        public override string ToString()
        {
            return $"{Id}, target: {(Target != null ? Target : null)}";
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
        public TDataType Data { get; }

        protected Modifier(string id, TDataType data, ModifierProperties modifierProperties = default) : base(id, modifierProperties)
        {
            Data = data;
        }

        // protected Modifier(Modifier<TDataType> other) : base(other)
        // {
        //     Data = other.Data;
        // }
    }
}
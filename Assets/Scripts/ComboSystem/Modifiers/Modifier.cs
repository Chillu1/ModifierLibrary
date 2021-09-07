using System;
using BaseProject;
using BaseProject.Utils;
using JetBrains.Annotations;

namespace ComboSystem
{
    //TODO list:
    //Resurrection
        //Single use, conditional cast on death, either makes new being, or removed all modifiers & resets health & stats
    //Conditional: on cast, on kill, on death. While life leach, while life leach over x %, While stunned, frozen, on fire
    //Combo buffs:
        //X specific stat buffs (movement speed buff, attack speed, evasion = special "cat" buff)
        //X specific buffs together
    //What kind of stackable behaviours do we want?:
    //  Stacks increases value/power
    //  Stacks increase speed/interval of DoT/effect,
    //A stackable DoT modifier
    //"Vaal" skill, aka getting X amount of kills to activate an effect
    //Stats
    //Damage
    //Resistances

    //Important Info:
    //Every modifier that doesn't go directly on it's owner, should go through the "ModifierApplier", to get the correct target

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
    public abstract class Modifier : IEntity<string>, IEventCopy<Modifier>, ICloneable
    {
        public event Action<Modifier> Removed;
        public string Id { get; protected set; }
        public ModifierProperties ModifierProperties { get; protected set; }
        [CanBeNull] public Being Target { get; protected set; }

        protected Func<Modifier, bool> Condition { get; set; } = arg => true;

        protected Modifier(string id, ModifierProperties modifierProperties = default)
        {
            Id = id;
            ModifierProperties = modifierProperties;
        }

        public void AddCondition(Func<Modifier, bool> condition)//TODO Test & mby think of another way
        {
            if (condition == null)
                return;
            if (Condition == condition)
                return;

            var trueDelegate = new Func<Modifier, bool>(delegate { return true; });
            if (condition == trueDelegate)
            {
                Log.Error("Tried to set a true delegate", "modifiers");
                return;
            }

            Condition = condition;
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
        ///     Called when modifier is applied to target
        /// </summary>
        protected virtual bool Apply()
        {
            if (!ApplyIsValid())
                return false;

            if (!ConditionIsMet())
                return false;

            Effect();

            return true;
        }

        protected virtual bool ApplyIsValid()
        {
            if (Target == null)
            {
                Log.Error("We tried to apply modifier without a target", "modifiers");
                return false;
            }

            return true;
        }

        protected virtual bool ConditionIsMet()
        {
            if (Condition == null)
                return true;

            return Condition(this);
        }

        /// <summary>
        ///     Does the actual effect of the modifier
        /// </summary>
        protected abstract void Effect();

        protected virtual void Remove()
        {
            Removed?.Invoke(this);
        }

        public virtual void Stack()
        {
            Log.Verbose("Stacked: " + this, "modifiers");
        }

        public virtual void Refresh()
        {
            Log.Verbose("Refreshed: " + this, "modifiers");
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

        public bool SetTarget(Being target)
        {
            if (!target.IsValidTarget(this))
                return false;

            if (Target == target)
                return true;

            if (Target != null)
            {
                Log.Error($"Info:{this}. Target {Target.Id} isn't null, tried to set to {target.Id}", "modifiers");
                return false;
            }

            Target = target;
            return true;
        }

        public virtual void CopyEvents(Modifier prototype)
        {
            Removed = prototype.Removed;
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
using System;

namespace ComboSystem
{
    //TODO list:
    //A buff that applies a debuff
    //Single use buff, duration
    //DoT debuff
    //DoT buff that applies a DoT debuff
    //Combo prototyping

    //Design:
    //Clean interaction between applied buffs on unit
    //All buffs in a single collection, ticking
    //Calculate base stats, then add percentages, every time our stats change, recalculate
    //A buff should apply a separate debuff, instead of having internal different state

    //TODO:
    //Effect stacks+max effect stacks. IsEffectStackable. IsDurationStackable. IsDurationRefreshable?. IsForever.
    public abstract class Modifier
    {
        public event Action<Modifier> Removed;

        protected abstract void Apply();

        protected virtual void Remove()
        {
            Removed?.Invoke(this);
        }

        public virtual void Update(float deltaTime)
        {
        }
    }

    public abstract class Modifier<TDataType> : Modifier
    {
        public TDataType Data { get; protected set; }
        public ICharacter Target { get; protected set; }
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
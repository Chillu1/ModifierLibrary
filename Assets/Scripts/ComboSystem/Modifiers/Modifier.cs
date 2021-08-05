namespace ComboSystem
{
    //Design:
    //Clean interaction between applied buffs on unit
    //All buffs in a single collection, ticking
    //Calculate base stats, then add percentages, every time our stats change, recalculate
    //A buff should apply a separate debuff, instead of having internal different state

    //TODO:
    //Effect stacks+max effect stacks. IsEffectStackable. IsDurationStackable. IsDurationRefreshable?. IsForever.
    public abstract class Modifier
    {
        protected abstract void Apply();

        protected virtual void Remove()
        {
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
    //     public Modifier GetModifier(ICharacter target)
    //     {
    //         return new TModifierType {Data = this.data, Target = target};
    //     }
    // }
}
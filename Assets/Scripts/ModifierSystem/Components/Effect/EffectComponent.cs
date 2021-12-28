namespace ModifierSystem
{
    public abstract class EffectComponent : Component, IEffectComponent
    {
        public virtual void Effect()
        {
        }
    }
}
namespace ComboSystem
{
    public abstract class DurationModifier<TDurationDataType> : Modifier<TDurationDataType> where TDurationDataType : DurationModifierData
    {
        protected float timer;

        protected DurationModifier(string id, ModifierProperties modifierProperties = default) : base(id, modifierProperties)
        {
        }

        public override void Update(float deltaTime)
        {
            timer += deltaTime;

            if (timer >= Data.Duration)
            {
                Remove();
                return;
            }

            base.Update(deltaTime);
        }
    }
}
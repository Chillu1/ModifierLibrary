namespace ComboSystem
{
    public abstract class DurationModifier<TDurationDataType> : Modifier<TDurationDataType> where TDurationDataType : DurationModifierData
    {
        protected float timer;

        public override void Update(float deltaTime)
        {
            timer += deltaTime;

            if (timer >= Data.Duration)
                Remove();

            base.Update(deltaTime);
        }
    }
}